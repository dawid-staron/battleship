using System.Collections.Generic;
using GameComponents.Playground.BuildingBlocks;

namespace TestingUtilities
{
    public static class Factories
    {
        public static List<Coordinate> CreateCoordinates(params Coordinate[] coordinates) => new(coordinates);

        public static Coordinate CreateCoordinate(int row = 1, int column = 1) => new(row, column);

        public static List<Position> CreatePositions(params Position[] positions) => new(positions);

        public static Position CreatePosition(int row = 1, int column = 1, PositionStatus status = null)
        {
            var position = new Position(CreateCoordinate(row, column));
            if (status != null)
            {
                position.ChangeStatus(status);
            }

            return position;
        }

        public static List<Position> FormBattlegroundPositions(int rows, int columns)
        {
            var positionsWithInitialState = CreatePositions();
            for (int rowIdx = 1; rowIdx <= rows; rowIdx++)
            {
                for (int columnIdx = 1; columnIdx <= columns; columnIdx++)
                {
                    positionsWithInitialState.Add(CreatePosition(rowIdx, columnIdx));
                }
            }

            return positionsWithInitialState;
        }
    }
}