using GameComponents.Playground.BuildingBlocks;

namespace GameComponents.Playground
{
    public interface IBattlegroundScanning
    {
        public Coordinate LastPosition { get; }

        public Coordinate FirstPosition { get; }

        IPosition GetPositionByCoordinate(Coordinate coordinate);
    }
}