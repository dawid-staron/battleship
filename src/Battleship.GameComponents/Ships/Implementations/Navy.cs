using System;
using System.Collections.Generic;
using System.Linq;
using GameComponents.Playground.BuildingBlocks;

namespace GameComponents.Ships.Implementations
{
    internal class Navy : IShipGroup
    {
        private readonly List<IShip> _ships = new();

        public bool Destroyed => _ships.All(ship => ship.Destroyed);

        public bool ProtectionZoneViolate(IEnumerable<Coordinate> positions) =>
            _ships.Any(ship => ship.ProtectionZoneViolate(positions));

        public void AttackIncoming(Coordinate onTarget) =>
            _ships.ForEach(ship => ship.AttackIncoming(onTarget));

        public void Join(IShip ship) =>
            _ships.Add(ship ?? throw new ArgumentNullException(nameof(ship)));
    }
}