using System;
using System.Collections.Generic;
using System.Linq;
using GameComponents.Playground.BuildingBlocks;

namespace GameComponents.Playground.Implementations
{
    public class Battleground : IBattlegroundComputing
    {
        private readonly Random _random;
        private readonly IEnumerable<Position> _positions;

        public Battleground(IEnumerable<Position> positions)
        {
            _positions = new List<Position>(positions ?? throw new ArgumentNullException(nameof(positions)));
            var positionStatistics = _positions
                .Select(position => position.Coordinate)
                .Aggregate(
                    new PositionStatistics(),
                    (current, next) => current.Accumulate(current, next),
                    accumulated => accumulated.Compute());

            FirstPosition = positionStatistics.First;
            LastPosition = positionStatistics.Last;
            _random = new Random();
        }

        public IPosition GetPositionByCoordinate(Coordinate coordinate) =>
            _positions.SingleOrDefault(position => position.Coordinate.Identifier == coordinate.Identifier)
            ?? throw new ArgumentOutOfRangeException(nameof(coordinate), "Provided coordinate is invalid");

        public Coordinate LastPosition { get; }

        public Coordinate FirstPosition { get; }

        private class PositionStatistics
        {
            public Coordinate First { get; private set; } = new(int.MaxValue, int.MaxValue);

            public Coordinate Last { get; private set; } = new(1, 1);

            public PositionStatistics Accumulate(PositionStatistics current, Coordinate next)
            {
                if (current.First.Row > next.Row ||
                    current.First.Row == next.Row && current.First.Column > next.Column)
                {
                    First = next;
                }

                if (current.Last.Row < next.Row ||
                    current.Last.Row == next.Row && current.Last.Column < next.Column)
                {
                    Last = next;
                }

                return this;
            }

            public PositionStatistics Compute() => this;
        }

        public virtual Coordinate GetRandom =>
            new(_random.Next(FirstPosition.Row, LastPosition.Row + 1),
                _random.Next(FirstPosition.Column, LastPosition.Column + 1));

        public IReadOnlyCollection<Position> GetAllExcept(IEnumerable<Position> except) =>
            _positions
                .Concat(except)
                .GroupBy(position => position.Coordinate.Identifier)
                .Select(groupPositions => new
                {
                    Position = groupPositions.First(),
                    Count = groupPositions.Count()
                })
                .Where(groupPositions => groupPositions.Count == 1)
                .Select(groupPositions => groupPositions.Position)
                .ToList();

        public virtual IReadOnlyCollection<Position> GetRandomSiblingsPositions(int onDistance, Coordinate startingFrom)
        {
            Dictionary<int, (Func<bool>, Func<int, Position>)> possibleDirections = new()
            {
                { 0, (CanGoEast, move => UpgradeTo(GetPositionByCoordinate(new Coordinate(startingFrom.Row, startingFrom.Column + move)))) },
                { 1, (CanGoWest, move => UpgradeTo(GetPositionByCoordinate(new Coordinate(startingFrom.Row, startingFrom.Column - move)))) },
                { 2, (CanGoNorth, move => UpgradeTo(GetPositionByCoordinate(new Coordinate(startingFrom.Row - move, startingFrom.Column))))},
                { 3, (CanGoSouth, move => UpgradeTo(GetPositionByCoordinate(new Coordinate(startingFrom.Row + move, startingFrom.Column)))) }
            };

            foreach (var directionNumber in new[] { 0, 1, 2, 3 }.Shuffle(_random))
            {
                var (directionAchievable, calculateSiblingPositions) = possibleDirections[directionNumber];
                if (directionAchievable())
                {
                    return new List<Position>(ShipLocationOnDistance(calculateSiblingPositions));
                }
            }

            return new List<Position>(0);

            bool CanGoEast() => startingFrom.Column + (onDistance - 1) <= LastPosition.Column;
            bool CanGoWest() => startingFrom.Column - (onDistance - 1) >= FirstPosition.Column;
            bool CanGoNorth() => startingFrom.Row - (onDistance - 1) >= FirstPosition.Row;
            bool CanGoSouth() => startingFrom.Row + (onDistance - 1) <= LastPosition.Row;

            IEnumerable<Position> ShipLocationOnDistance(Func<int, Position> calculateSiblingPosition)
            {
                var positions = new List<Position>();
                for (var idx = 0; idx < onDistance; idx++)
                {
                    positions.Add(calculateSiblingPosition(idx));
                }

                return positions;
            }
        }

        protected Position UpgradeTo(IPosition readonlyPosition) =>
            (Position)readonlyPosition;
    }
}