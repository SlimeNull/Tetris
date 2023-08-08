namespace Tetris
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            TetrisGame game = new TetrisGame(30, 30);

            Console.WriteLine("按任意键继续");
            Console.ReadKey();

            // 后台循环, 每隔 700ms 调用一次 NextTurn 进行一次回合判断
            Task.Run(async () =>
            {
                while (true)
                {
                    // 延时
                    await Task.Delay(700);

                    // 下一回合
                    game.NextTurn();

                    // 渲染
                    game.Render();
                }
            });

            // 主循环接受用户输入
            while (true)
            {
                var key = Console.ReadKey();

                switch (key.Key)
                {
                    // A 键或左箭头向右移动方块
                    case ConsoleKey.A:
                    case ConsoleKey.LeftArrow:
                        game.MoveLeft();
                        break;

                    // D 键或右箭头向右移动方块
                    case ConsoleKey.D:
                    case ConsoleKey.RightArrow:
                        game.MoveRight();
                        break;

                    // W 键或上箭头改变方块样式
                    case ConsoleKey.W:
                    case ConsoleKey.UpArrow:
                        game.ChangeShapeStyle();
                        break;

                    // S 键或下箭头将方块降落到底部
                    case ConsoleKey.S:
                    case ConsoleKey.DownArrow:
                        game.Fall();
                        break;
                }

                // 每一次进行操作, 都重新绘制到界面上
                game.Render();
            }
        }
    }
}