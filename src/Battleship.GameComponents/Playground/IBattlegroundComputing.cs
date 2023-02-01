using System.Collections.Generic;
using GameComponents.Playground.BuildingBlocks;

namespace GameComponents.Playground
{
    public interface IBattlegroundComputing : IBattlegroundScanning
    {
        Coordinate GetRandom { get; }

        IReadOnlyCollection<Position> GetAllExcept(IEnumerable<Position> except);

        IReadOnlyCollection<Position> GetRandomSiblingsPositions(int onDistance, Coordinate startingFrom);
    }
}