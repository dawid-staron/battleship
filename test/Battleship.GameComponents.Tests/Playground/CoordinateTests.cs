using System;
using FluentAssertions;
using GameComponents.Playground.BuildingBlocks;
using Xunit;

namespace GameComponents.Tests.Playground
{
    public class CoordinateTests
    {
        [Theory]
        [InlineData(1, 0)]
        [InlineData(1, -1)]
        [InlineData(0, 1)]
        [InlineData(-1, 1)]
        public void Construct_WhenZeroedOrNegativeValues_ThenThrow(int row, int column)
        {
            var assertion = () => new Coordinate(row, column);

            assertion.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(10, 1)]
        [InlineData(1, 10)]
        [InlineData(6, 5)]
        public void Identifier_IsComposedOnRowAndColumn(
            int row, int column)
        {
            var expectedIdentifier = $"{row}:{column}";

            var actualIdentifier = new Coordinate(row, column).Identifier;

            actualIdentifier.Should().Be(expectedIdentifier);
        }
    }
}
