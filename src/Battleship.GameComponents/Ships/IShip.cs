using System.Collections.Generic;
using GameComponents.Playground.BuildingBlocks;

namespace GameComponents.Ships
{
    internal interface IShip
    {
        bool Destroyed { get; }

        bool ProtectionZoneViolate(IEnumerable<Coordinate> positions);

        void AttackIncoming(Coordinate onTarget);
    }
}