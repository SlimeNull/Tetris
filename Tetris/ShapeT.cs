namespace Tetris
{
    class ShapeT : Shape
    {
        public ShapeT(Coordinate position) : base(position)
        {
        }

        public override string Name => "T";

        protected override ShapeStyle[] ShapeStyles { get; } = new ShapeStyle[]
        {
            new ShapeStyle(
                new Coordinate[]
                {
                    new Coordinate(-1, 0),
                    new Coordinate(0, 0),
                    new Coordinate(1, 0),
                    new Coordinate(0, 1),
                }),
            new ShapeStyle(
                new Coordinate[]
                {
                    new Coordinate(-1, 0),
                    new Coordinate(0, -1),
                    new Coordinate(0, 0),
                    new Coordinate(0, 1),
                }),
            new ShapeStyle(
                new Coordinate[]
                {
                    new Coordinate(-1, 0),
                    new Coordinate(0, 0),
                    new Coordinate(1, 0),
                    new Coordinate(0, -1),
                }),
            new ShapeStyle(
                new Coordinate[]
                {
                    new Coordinate(1, 0),
                    new Coordinate(0, -1),
                    new Coordinate(0, 0),
                    new Coordinate(0, 1),
                }),
        };
    }
}