using System;

namespace GameComponents.Playground.BuildingBlocks
{
    public class Position : IPosition
    {
        public Position(Coordinate coordinate)
        {
            Coordinate = coordinate ?? throw new ArgumentNullException(nameof(coordinate));
            Status = PositionStatus.Untouched;
        }

        public Coordinate Coordinate { get; }

        public PositionStatus Status { get; private set; }

        public void ChangeStatus(PositionStatus newStatus)
        {
            Status = newStatus ?? throw new ArgumentNullException(nameof(newStatus));
        }
    }
}