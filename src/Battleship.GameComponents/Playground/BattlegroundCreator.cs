using System.Collections.Generic;
using GameComponents.Playground.BuildingBlocks;
using GameComponents.Playground.Implementations;

namespace GameComponents.Playground
{
    public static class BattlegroundCreator
    {
        public static IBattlegroundComputing Create(int rows, int columns)
        {
            var positionsWithInitialState = new List<Position>();
            for (int rowIdx = 1; rowIdx <= rows; rowIdx++)
            {
                for (int columnIdx = 1; columnIdx <= columns; columnIdx++)
                {
                    positionsWithInitialState.Add(new Position(new Coordinate(rowIdx, columnIdx)));
                }
            }

            return new Battleground(positionsWithInitialState);
        }
    }
}