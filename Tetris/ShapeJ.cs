namespace Tetris
{
    class ShapeJ : Shape
    {
        public ShapeJ(Coordinate position) : base(position)
        {
        }

        public override string Name => "J";

        protected override ShapeStyle[] ShapeStyles { get; } = new ShapeStyle[]
        {
            new ShapeStyle(
                new Coordinate[]
                {
                    new Coordinate(0, -1),
                    new Coordinate(0, 0),
                    new Coordinate(0, 1),
                    new Coordinate(-1, 1),
                }),
            new ShapeStyle(
                new Coordinate[]
                {
                    new Coordinate(-1, -1),
                    new Coordinate(-1, 0),
                    new Coordinate(0, 0),
                    new Coordinate(1, 0),
                }),
            new ShapeStyle(
                new Coordinate[]
                {
                    new Coordinate(0, -1),
                    new Coordinate(0, 0),
                    new Coordinate(0, 1),
                    new Coordinate(1, -1),
                }),
            new ShapeStyle(
                new Coordinate[]
                {
                    new Coordinate(1, 1),
                    new Coordinate(-1, 0),
                    new Coordinate(0, 0),
                    new Coordinate(1, 0),
                }),
        };
    }
}