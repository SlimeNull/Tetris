namespace Tetris
{
    class ShapeI : Shape
    {
        public ShapeI(Coordinate position) : base(position)
        {
        }

        public override string Name => "I";

        protected override ShapeStyle[] ShapeStyles { get; } = new ShapeStyle[]
        {
            new ShapeStyle(
                new Coordinate[]
                {
                    new Coordinate(0, -1),
                    new Coordinate(0, 0),
                    new Coordinate(0, 1),
                    new Coordinate(0, 2),
                }),
            new ShapeStyle(
                new Coordinate[]
                {
                    new Coordinate(-1, 0),
                    new Coordinate(0, 0),
                    new Coordinate(1, 0),
                    new Coordinate(2, 0),
                }),
            new ShapeStyle(
                new Coordinate[]
                {
                    new Coordinate(1, -1),
                    new Coordinate(1, 0),
                    new Coordinate(1, 1),
                    new Coordinate(1, 2),
                }),
            new ShapeStyle(
                new Coordinate[]
                {
                    new Coordinate(-1,1),
                    new Coordinate(0, 1),
                    new Coordinate(1, 1),
                    new Coordinate(2, 1),
                }),
        };
    }
}