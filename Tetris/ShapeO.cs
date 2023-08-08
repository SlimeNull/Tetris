namespace Tetris
{
    class ShapeO : Shape
    {
        public ShapeO(Coordinate position) : base(position)
        {
        }

        public override string Name => "O";

        protected override ShapeStyle[] ShapeStyles { get; } = new ShapeStyle[]
        {
            new ShapeStyle(
                new Coordinate[]
                {
                    new Coordinate(0, 0),
                    new Coordinate(0, 1),
                    new Coordinate(1, 1),
                    new Coordinate(1, 0),
                })
        };
    }
}