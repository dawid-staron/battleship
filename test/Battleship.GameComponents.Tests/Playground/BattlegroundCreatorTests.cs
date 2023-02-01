using FluentAssertions;
using GameComponents.Playground;
using GameComponents.Playground.BuildingBlocks;
using TestingUtilities;
using Xunit;

namespace GameComponents.Tests.Playground
{
    public class BattlegroundCreatorTests
    {
        [Fact]
        public void Create_BattlegroundWithExpectedSizesAndAllPositionsHaveInitialState()
        {
            var rows = 5;
            var columns = 5;

            var sut = BattlegroundCreator.Create(rows, columns);

            for (int rowIdx = 1; rowIdx <= 5; rowIdx++)
            {
                for (int columnIdx = 1; columnIdx <= 5; columnIdx++)
                {
                    var actualPosition = sut.GetPositionByCoordinate(Factories.CreateCoordinate(rowIdx, columnIdx));
                    actualPosition.Status.Should().Be(PositionStatus.Untouched);
                }
            }
        }
    }
}