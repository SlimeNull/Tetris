namespace Tetris
{
    /// <summary>
    /// 形状基类
    /// </summary>
    abstract class Shape
    {
        /// <summary>
        /// 名称
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// 形状的位置
        /// </summary>
        public Coordinate Position { get; set; }

        /// <summary>
        /// 形状所有的样式
        /// </summary>
        protected abstract ShapeStyle[] ShapeStyles { get; }

        /// <summary>
        /// 当前使用的样式索引
        /// </summary>
        private int _currentStyleIndex = 0;

        /// <summary>
        /// 从坐标构建一个新形状
        /// </summary>
        /// <param name="position"></param>
        public Shape(Coordinate position)
        {
            Position = position;
        }

        /// <summary>
        /// 获取当前形状的当前所有方块 (相对坐标)
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Coordinate> GetBlocks()
        {
            return ShapeStyles[_currentStyleIndex].Coordinates;
        }

        /// <summary>
        /// 获取当前形状下一个样式的所有方块 (相对坐标)
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Coordinate> GetNextStyleBlocks()
        {
            return ShapeStyles[(_currentStyleIndex + 1) % ShapeStyles.Length].Coordinates;
        }

        /// <summary>
        /// 改变样式
        /// </summary>
        public void ChangeStyle()
        {
            _currentStyleIndex = (_currentStyleIndex + 1) % ShapeStyles.Length;
        }
    }
}