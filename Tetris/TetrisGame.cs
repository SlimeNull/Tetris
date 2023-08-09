using System.Text;

namespace Tetris
{
    class TetrisGame
    {
        /// <summary>
        /// x, y
        /// </summary>
        private readonly bool[,] map;

        private readonly Random random = new Random();


        public TetrisGame(int width, int height)
        {
            map = new bool[width, height];

            Width = width;
            Height = height;
        }

        public Shape? CurrentShape { get; set; }

        public int Width { get; }
        public int Height { get; }

        /// <summary>
        /// 判断是否可以移动 (移动后是否会与已有方块重合, 或者超出边界)
        /// </summary>
        /// <param name="xOffset"></param>
        /// <param name="yOffset"></param>
        /// <returns></returns>
        private bool CanMove(int xOffset, int yOffset)
        {
            // 如果当前没形状, 返回 false
            if (CurrentShape == null)
                return false;

            foreach (var block in CurrentShape.GetBlocks())
            {
                Coordinate coord = Coordinate.GetAbstract(CurrentShape.Position, block);

                coord.X += xOffset;
                coord.Y += yOffset;

                // 如果移动后方块坐标超出界限, 不能移动
                if (coord.X < 0 || coord.X >= Width ||
                    coord.Y < 0 || coord.Y >= Height)
                    return false;

                // 如果移动后方块会与地图现有方块重合, 则不能移动
                if (map[coord.X, coord.Y])
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 判断是否可以改变形状 (改变形状后是否会和已有方块重合, 或者超出边界)
        /// </summary>
        /// <returns></returns>
        private bool CanChangeShape()
        {
            // 如果当前没形状, 当然不能切换样式
            if (CurrentShape == null)
                return false;

            // 获取下一个样式的所有方块
            foreach (var block in CurrentShape.GetNextStyleBlocks())
            {
                Coordinate coord = Coordinate.GetAbstract(CurrentShape.Position, block);

                // 如果超出界限, 不能切换
                if (coord.X < 0 || coord.X >= Width ||
                    coord.Y < 0 || coord.Y >= Height)
                    return false;

                // 如果与现有方块重合, 不能切换
                if (map[coord.X, coord.Y])
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 将当前形状存储到地图中
        /// </summary>
        private void StorageShapeToMap()
        {
            // 没形状, 存寂寞
            if (CurrentShape == null)
                return;

            // 所有方块遍历一下
            foreach (var block in CurrentShape.GetBlocks())
            {
                // 转为绝对坐标
                Coordinate coord = Coordinate.GetAbstract(CurrentShape.Position, block);

                // 超出界限则跳过
                if (coord.X < 0 || coord.X >= Width ||
                    coord.Y < 0 || coord.Y >= Height)
                    continue;

                // 存地图里
                map[coord.X, coord.Y] = true;
            }

            // 当前形状设为 null
            CurrentShape = null;
        }

        /// <summary>
        /// 生成一个新形状
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        private void GenerateShape()
        {
            int shapeCount = 7;
            int randint = random.Next(shapeCount);

            Coordinate initCoord = new Coordinate(Width / 2, 0);

            Shape newShape = randint switch
            {
                0 => new ShapeI(initCoord),
                1 => new ShapeJ(initCoord),
                2 => new ShapeL(initCoord),
                3 => new ShapeO(initCoord),
                4 => new ShapeS(initCoord),
                5 => new ShapeT(initCoord),
                6 => new ShapeZ(initCoord),

                _ => throw new InvalidOperationException()
            };

            CurrentShape = newShape;
        }

        /// <summary>
        /// 扫描, 消除掉可消除的行
        /// </summary>
        private void Scan()
        {
            for (int y = 0; y < Height; y++)
            {
                // 设置当前行是整行
                bool ok = true;

                // 循环当前行的所有方块, 如果方块为 false, ok 就会被设为 false
                for (int x = 0; x < Width; x++)
                    ok &= map[x, y];

                // 如果当前行确实是整行
                if (ok)
                {
                    // 所有行全部往下移动
                    for (int _y = y; _y > 0; _y--)
                        for (int x = 0; x < Width; x++)
                            map[x, _y] = map[x, _y - 1];

                    // 最顶行全设为空
                    for (int x = 0; x < Width; x++)
                        map[x, 0] = false;
                }
            }
        }

        /// <summary>
        /// 根据指定偏移, 进行移动
        /// </summary>
        /// <param name="xOffset"></param>
        /// <param name="yOffset"></param>
        public void Move(int xOffset, int yOffset)
        {
            lock (this)
            {
                if (CurrentShape == null)
                    return;

                if (CanMove(xOffset, yOffset))
                {
                    var newCoord = CurrentShape.Position;
                    newCoord.X += xOffset;
                    newCoord.Y += yOffset;

                    CurrentShape.Position = newCoord;
                }
            }
        }

        /// <summary>
        /// 向左移动
        /// </summary>
        public void MoveLeft()
        {
            Move(-1, 0);
        }

        /// <summary>
        /// 向右移动
        /// </summary>
        public void MoveRight()
        {
            Move(1, 0);
        }

        /// <summary>
        /// 向下移动
        /// </summary>
        public void MoveDown()
        {
            Move(0, 1);
        }

        /// <summary>
        /// 改变形状样式
        /// </summary>
        public void ChangeShapeStyle()
        {
            lock (this)
            {
                if (CurrentShape == null)
                    return;

                if (CanChangeShape())
                    CurrentShape.ChangeStyle();
            }
        }

        /// <summary>
        /// 降落到底部
        /// </summary>
        public void Fall()
        {
            lock (this)
            {
                while (CanMove(0, 1))
                {
                    Move(0, 1);
                }
            }
        }

        /// <summary>
        /// 下一个回合
        /// </summary>
        public void NextTurn()
        {
            lock (this)
            {
                // 如果当前没有存在的形状, 则生成一个新的, 并返回
                if (CurrentShape == null)
                {
                    GenerateShape();
                    return;
                }

                // 如果可以向下移动
                if (CanMove(0, 1))
                {
                    // 直接改变当前形状的坐标
                    var newCoord = CurrentShape.Position;
                    newCoord.Y += 1;

                    CurrentShape.Position = newCoord;
                }
                else
                {
                    // 将当前的形状保存到地图中
                    StorageShapeToMap();
                }

                // 扫描, 判断某些行可以被消除
                Scan();
            }
        }

        public void Render()
        {
            StringBuilder sb = new StringBuilder();

            bool[,] mapCpy = new bool[Width, Height];
            Array.Copy(map, mapCpy, mapCpy.Length);

            if (CurrentShape != null)
            {
                foreach (var block in CurrentShape.GetBlocks())
                {
                    Coordinate coord = Coordinate.GetAbstract(CurrentShape.Position, block);
                    if (coord.X < 0 || coord.X >= Width ||
                        coord.Y < 0 || coord.Y >= Height)
                        continue;

                    mapCpy[coord.X, coord.Y] = true;
                }
            }

            sb.AppendLine("┌" + new string('─', Width * 2) + "┐");
            for (int y = 0; y < Height; y++)
            {
                sb.Append("|");

                for (int x = 0; x < Width; x++)
                {
                    sb.Append(mapCpy[x, y] ? "##" : "  ");
                }

                sb.Append("|");

                sb.AppendLine();
            }

            sb.AppendLine("└" + new string('─', Width * 2) + "┘");

            lock (this)
            {
                Console.SetCursorPosition(0, 0);
                Console.Write(sb.ToString());
            }
        }
    }
}