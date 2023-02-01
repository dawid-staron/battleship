using System;
using System.Collections.Generic;
using System.Linq;
using GameComponents.Playground.BuildingBlocks;

namespace GameComponents.Ships.Implementations
{
    internal class Ship : IShip
    {
        private readonly List<Position> _location;
        private readonly IEnumerable<Coordinate> _protectionZone;

        public Ship(IEnumerable<Position> location)
        {
            _location = new List<Position>(location ?? throw new ArgumentNullException(nameof(location)));
            _protectionZone = location
                .SelectMany(x => EstablishProtectionZone(x.Coordinate))
                .DistinctBy(coordinate => coordinate.Identifier)
                .ToList();

            IEnumerable<Coordinate> EstablishProtectionZone(Coordinate shipCoordinate)
            {
                return new[]
                {
                    shipCoordinate,
                    South(),
                    SouthWest(),
                    West(),
                    NorthWest(),
                    North(),
                    NorthEast(),
                    East(),
                    SouthEast()
                };

                Coordinate South() => new(shipCoordinate.Row + 1, shipCoordinate.Column);
                Coordinate SouthWest() => new(shipCoordinate.Row + 1, ZeroProtection(shipCoordinate.Column - 1));
                Coordinate West() => new(shipCoordinate.Row, ZeroProtection(shipCoordinate.Column - 1));
                Coordinate NorthWest() => new(ZeroProtection(shipCoordinate.Row - 1), ZeroProtection(shipCoordinate.Column - 1));
                Coordinate North() => new(ZeroProtection(shipCoordinate.Row - 1), shipCoordinate.Column);
                Coordinate NorthEast() => new(ZeroProtection(shipCoordinate.Row - 1), shipCoordinate.Column + 1);
                Coordinate East() => new(shipCoordinate.Row, shipCoordinate.Column + 1);
                Coordinate SouthEast() => new(shipCoordinate.Row + 1, shipCoordinate.Column + 1);
                int ZeroProtection(int value) => value > 0 ? value : 1;
            }
        }

        public bool Destroyed => _location.All(x => x.Status == PositionStatus.Sank);

        public bool ProtectionZoneViolate(IEnumerable<Coordinate> positions) =>
            positions.Any(x =>
                _protectionZone.Any(protectedCoordinate => protectedCoordinate.Identifier == x.Identifier));

        public void AttackIncoming(Coordinate onTarget)
        {
            var hitOrMissed = _location.SingleOrDefault(x => x.Coordinate.Identifier == onTarget.Identifier);
            if (hitOrMissed != null && hitOrMissed.Status != PositionStatus.Sank)
            {
                hitOrMissed.ChangeStatus(PositionStatus.OnFire);
                ShipDestroyed();
            }

            void ShipDestroyed()
            {
                if (_location.All(x => x.Status == PositionStatus.OnFire))
                {
                    _location.ForEach(x => x.ChangeStatus(PositionStatus.Sank));
                }
            }
        }
    }
}