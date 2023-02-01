using System;

namespace GameComponents.Playground.BuildingBlocks
{
    public class Coordinate
    {
        public Coordinate(int row, int column)
        {
            if (Invalid(row))
            {
                throw new ArgumentOutOfRangeException(nameof(row));
            }

            if (Invalid(column))
            {
                throw new ArgumentOutOfRangeException(nameof(column));
            }

            Row = row;
            Column = column;
            Identifier = $"{Row}:{column}";

            bool Invalid(int value) => value <= 0;
        }

        public string Identifier { get; }

        public int Row { get; }

        public int Column { get; }

        public override string ToString() => Identifier;
    }
}