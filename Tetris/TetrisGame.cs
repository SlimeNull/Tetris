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
            if (CurrentShape == null)
                return false;

            foreach (var block in CurrentShape.GetBlocks())
            {
                Coordinate coord = Coordinate.GetAbstract(CurrentShape.Position, block);

                coord.X += xOffset;
                coord.Y += yOffset;

                if (coord.X < 0 || coord.X >= Width ||
                    coord.Y < 0 || coord.Y >= Height)
                    return false;

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
            if (CurrentShape == null)
                return false;

            foreach (var block in CurrentShape.GetNextStyleBlocks())
            {
                Coordinate coord = Coordinate.GetAbstract(CurrentShape.Position, block);

                if (coord.X < 0 || coord.X >= Width ||
                    coord.Y < 0 || coord.Y >= Height)
                    return false;

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
            if (CurrentShape == null)
                return;

            foreach (var block in CurrentShape.GetBlocks())
            {
                Coordinate coord = Coordinate.GetAbstract(CurrentShape.Position, block);

                if (coord.X < 0 || coord.X >= Width ||
                    coord.Y < 0 || coord.Y >= Height)
                    continue;

                map[coord.X, coord.Y] = true;
            }

            CurrentShape = null;
        }

        /// <summary>
        /// 生成一个新形状
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        private void GenerateShape()
        {
            int shapeCount = 7;
            int randint = Random.Shared.Next(shapeCount);

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
            for (int y = 0;  y < Height; y++)
            {
                bool ok = true;
                for (int x = 0; x < Width; x++)
                    ok &= map[x, y];

                if (ok)
                {
                    for (int _y = y; _y > 0; _y--)
                        for (int x = 0; x < Width; x++)
                            map[x, _y] = map[x, _y - 1];

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

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    sb.Append(mapCpy[x, y] ? "MM" : "  ");
                }

                sb.AppendLine();
            }

            Console.SetCursorPosition(0, 0);
            Console.Write(sb.ToString());
        }
    }
}