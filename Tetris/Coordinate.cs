namespace Tetris
{
    /// <summary>
    /// 表示一个坐标
    /// </summary>
    /// <param name="X"></param>
    /// <param name="Y"></param>
    record struct Coordinate(int X, int Y)
    {
        /// <summary>
        /// 根据基坐标和相对坐标, 获取一个绝对坐标
        /// </summary>
        /// <param name="baseCoord"></param>
        /// <param name="relativeCoord"></param>
        /// <returns></returns>
        public static Coordinate GetAbstract(Coordinate baseCoord, Coordinate relativeCoord)
        {
            return new Coordinate(baseCoord.X + relativeCoord.X, baseCoord.Y + relativeCoord.Y);
        }
    }
}