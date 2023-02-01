using System;
using System.Collections.Generic;
using System.Linq;
using GameComponents.Playground.BuildingBlocks;

namespace GameComponents.Ships.Implementations
{
    internal class GhostShip : IShip
    {
        private readonly List<Position> _location;

        public GhostShip(IEnumerable<Position> location)
        {
            _location = new List<Position>(location ?? throw new ArgumentNullException(nameof(location)));
        }

        public bool Destroyed => true;

        public bool ProtectionZoneViolate(IEnumerable<Coordinate> positions) => false;

        public void AttackIncoming(Coordinate onTarget) =>
            _location
                .SingleOrDefault(x => x.Coordinate.Identifier == onTarget.Identifier)
                ?.ChangeStatus(PositionStatus.Empty);
    }
}