using System.Collections.Generic;
using System.Linq;
using GameComponents.Playground.BuildingBlocks;
using GameComponents.Playground.Implementations;

namespace TestingUtilities
{
    public class BattlegroundWithSemiManualShipLocationStub : Battleground
    {
        private readonly List<List<Position>> _shipsLocation;

        public BattlegroundWithSemiManualShipLocationStub(
            IEnumerable<Position> positions,
            params List<Position>[] shipsLocation)
            : base(positions)
        {
            _shipsLocation = shipsLocation.ToList();
        }

        public override IReadOnlyCollection<Position> GetRandomSiblingsPositions(int onDistance, Coordinate startingFrom)
        {
            var shipLocation = _shipsLocation.FirstOrDefault();
            if (shipLocation == null)
            {
                return base.GetRandomSiblingsPositions(onDistance, startingFrom);
            }

            _shipsLocation.RemoveAt(0);

            return shipLocation
                .Select(toReplaceWithOriginal => UpgradeTo(GetPositionByCoordinate(toReplaceWithOriginal.Coordinate)))
                .ToList();
        }
    }
}