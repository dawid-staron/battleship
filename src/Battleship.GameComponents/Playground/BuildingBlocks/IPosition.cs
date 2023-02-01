namespace GameComponents.Playground.BuildingBlocks;

public interface IPosition
{
    Coordinate Coordinate { get; }

    PositionStatus Status { get; }
}