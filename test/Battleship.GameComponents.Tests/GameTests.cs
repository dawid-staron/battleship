using System.Linq;
using FluentAssertions;
using GameComponents.Playground.BuildingBlocks;
using GameComponents.Playground.Implementations;
using TestingUtilities;
using Xunit;

namespace GameComponents.Tests
{
    public class GameTests
    {
        private Game _sut;

        [Fact]
        public void Ensure_EmptyPositionsMargin_AroundAllShips()
        {
            var expectedBattlegroundState = Factories.CreatePositions(
                Factories.CreatePosition(1, 1, PositionStatus.Empty),
                Factories.CreatePosition(2, 1, PositionStatus.Empty),
                Factories.CreatePosition(1, 2, PositionStatus.Sank),
                Factories.CreatePosition(2, 2, PositionStatus.Sank),
                Factories.CreatePosition(1, 3, PositionStatus.Empty),
                Factories.CreatePosition(2, 3, PositionStatus.Empty),
                Factories.CreatePosition(1, 4, PositionStatus.Sank),
                Factories.CreatePosition(2, 4, PositionStatus.Sank));
            var battleground = new BattlegroundWithSemiManualShipLocationStub(
                Factories.FormBattlegroundPositions(2, 4),
                Factories.CreatePositions(
                    Factories.CreatePosition(1, 2),
                    Factories.CreatePosition(2, 2)));
            _sut = Game.Start(new[] { 2, 2 }, battleground, battleCompleted: () => { });

            AttackAllPositions(_sut);
            for (int rowIdx = 1; rowIdx <= _sut.Battleground.LastPosition.Row; rowIdx++)
            {
                for (int columnIdx = 1; columnIdx <= _sut.Battleground.LastPosition.Column; columnIdx++)
                {
                    var expectedPosition = expectedBattlegroundState.Single(x =>
                        x.Coordinate.Identifier == Factories.CreateCoordinate(rowIdx, columnIdx).Identifier);
                    var actualPosition = _sut.Battleground.GetPositionByCoordinate(expectedPosition.Coordinate);
                    actualPosition.Should().BeEquivalentTo(expectedPosition);
                }
            }
        }

        [Fact]
        public void WhenAllShipsDestroyed_ThenGameCompleted()
        {
            var gameCompletedWhenAllShipsDestroyed = false;
            _sut = Game.Start(
                new[] { 5, 4, 4 }, 
                new Battleground(Factories.FormBattlegroundPositions(10, 10)),
                battleCompleted: () => { gameCompletedWhenAllShipsDestroyed = true; });

            AttackAllPositions(_sut);
            
            gameCompletedWhenAllShipsDestroyed.Should().BeTrue();
        }

        private void AttackAllPositions(Game game)
        {
            for (int rowIdx = 1; rowIdx <= game.Battleground.LastPosition.Row; rowIdx++)
            {
                for (int columnIdx = 1; columnIdx <= game.Battleground.LastPosition.Column; columnIdx++)
                {
                    game.MakeAttack(Factories.CreateCoordinate(rowIdx, columnIdx));
                }
            }
        }
    }
}
