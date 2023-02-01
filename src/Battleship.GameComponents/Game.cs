using System;
using System.Collections.Generic;
using System.Linq;
using GameComponents.Playground;
using GameComponents.Playground.BuildingBlocks;
using GameComponents.Ships;
using GameComponents.Ships.Implementations;

namespace GameComponents
{
    public class Game
    {
        private readonly Action _battleCompleted;
        private readonly IBattlegroundComputing _battleground;
        private readonly IShipGroup _navy = new Navy();

        private Game(IEnumerable<int> shipSizes, IBattlegroundComputing battleground, Action battleCompleted)
        {
            _battleCompleted = battleCompleted ?? throw new ArgumentNullException(nameof(battleCompleted));
            _battleground = battleground ?? throw new ArgumentNullException(nameof(battleground));
            AllocateShips(shipSizes);
        }

        public IBattlegroundScanning Battleground => _battleground;

        public static Game Start(int[] shipSizes, IBattlegroundComputing battleground, Action battleCompleted) =>
            new(shipSizes, battleground, battleCompleted);

        public IBattlegroundScanning MakeAttack(Coordinate coordinatesForAttack)
        {
            _navy.AttackIncoming(coordinatesForAttack);
            if (_navy.Destroyed)
            {
                _battleCompleted();
            }
            return _battleground;
        }

        private void AllocateShips(IEnumerable<int> shipSizes)
        {
            var shipLocations = new List<Position>();
            foreach (var shipSize in shipSizes)
            {
                bool collisionDetected;
                do
                {
                    IReadOnlyCollection<Position> shipLocation = null;
                    do
                    {
                        shipLocation = _battleground.GetRandomSiblingsPositions(shipSize, _battleground.GetRandom);
                    } while (!shipLocation.Any());

                    collisionDetected = _navy.ProtectionZoneViolate(
                        shipLocation.Select(position => position.Coordinate));
                    if (!collisionDetected)
                    {
                        _navy.Join(new Ship(shipLocation));
                        shipLocations.AddRange(shipLocation);
                    }

                } while (collisionDetected);
            }

            _navy.Join(new GhostShip(_battleground.GetAllExcept(shipLocations)));
        }
    }
}