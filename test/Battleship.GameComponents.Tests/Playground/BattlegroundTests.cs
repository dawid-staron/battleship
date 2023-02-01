using System;
using FluentAssertions;
using GameComponents.Playground.Implementations;
using TestingUtilities;
using Xunit;

namespace GameComponents.Tests.Playground
{
    public class BattlegroundTests
    {
        private readonly Battleground _sut = new(
            Factories.CreatePositions(
                Factories.CreatePosition(1, 1),
                Factories.CreatePosition(1, 2),
                Factories.CreatePosition(2, 1),
                Factories.CreatePosition(2, 2)));

        [Fact]
        public void GetFirstPosition_FromBattleground()
        {
            var expectedFirstPosition = Factories.CreatePosition(1, 1);

            _sut.FirstPosition.Should().BeEquivalentTo(expectedFirstPosition.Coordinate);
        }

        [Fact]
        public void GetLastPosition_FromBattleground()
        {
            var expectedFirstPosition = Factories.CreatePosition(2, 2);

            _sut.LastPosition.Should().BeEquivalentTo(expectedFirstPosition.Coordinate);
        }

        [Fact]
        public void GetPositionByCoordinate_WhenCoordinatesAreOutOfBattlegroundRange_ThenThrow()
        {
            var coordinateOutOfRange = Factories.CreateCoordinate(4, 2);

            var assertion = () => _sut.GetPositionByCoordinate(coordinateOutOfRange);

            assertion.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void GetRandom_FromBattleground()
        {
            var randomCoordinate = _sut.GetRandom;

            randomCoordinate.Row.Should().BeGreaterOrEqualTo(_sut.FirstPosition.Row);
            randomCoordinate.Row.Should().BeLessThanOrEqualTo(_sut.LastPosition.Row);
            randomCoordinate.Column.Should().BeGreaterOrEqualTo(_sut.FirstPosition.Column);
            randomCoordinate.Column.Should().BeLessThanOrEqualTo(_sut.LastPosition.Column);
        }

        [Fact]
        public void GetAllExcept_ReturnAllRestBattlegroundPositions()
        {
            var expectedPositions = Factories.CreatePositions(
                Factories.CreatePosition(2, 1),
                Factories.CreatePosition(2, 2));

            var actualPositions = _sut.GetAllExcept(
                Factories.CreatePositions(
                    Factories.CreatePosition(1, 1),
                    Factories.CreatePosition(1, 2)));

            actualPositions.Should().BeEquivalentTo(expectedPositions);
        }

        [Fact]
        public void GetRandomSiblingsPositions_WhenOnlyEastDirectionAvailable_ThenReturnSiblingsPositionsOnDistanceWithEastDirection()
        {
            var startingPositionOnLeftTopCorner = Factories.CreatePosition(1, 1);
            var sut = new Battleground(Factories.FormBattlegroundPositions(2, 3));
            var expectedSiblingsPositionsOnEastDirection = Factories.CreatePositions(
                startingPositionOnLeftTopCorner,
                Factories.CreatePosition(1, 2),
                Factories.CreatePosition(1, 3));
            var onDistanceWhichIsLongerThenRowsAmountOnBattleground = sut.LastPosition.Row + 1;

            var actualResult = sut.GetRandomSiblingsPositions(
                onDistanceWhichIsLongerThenRowsAmountOnBattleground, startingPositionOnLeftTopCorner.Coordinate);

            actualResult.Should().BeEquivalentTo(expectedSiblingsPositionsOnEastDirection);
        }

        [Fact]
        public void GetRandomSiblingsPositions_WhenOnlyWestDirectionAvailable_ThenReturnSiblingsPositionsOnDistanceWithWestDirection()
        {
            var startingPositionOnRightBottomCorner = Factories.CreatePosition(2, 3);
            var sut = new Battleground(Factories.FormBattlegroundPositions(2, 3));
            var expectedSiblingsPositionsOnWestDirection = Factories.CreatePositions(
                startingPositionOnRightBottomCorner,
                Factories.CreatePosition(2, 2),
                Factories.CreatePosition(2, 1));
            var onDistanceWhichIsLongerThenRowsAmountOnBattleground = sut.LastPosition.Row + 1;

            var actualResult = sut.GetRandomSiblingsPositions(
                onDistanceWhichIsLongerThenRowsAmountOnBattleground, startingPositionOnRightBottomCorner.Coordinate);

            actualResult.Should().BeEquivalentTo(expectedSiblingsPositionsOnWestDirection);
        }

        [Fact]
        public void GetRandomSiblingsPositions_WhenOnlySouthDirectionAvailable_ThenReturnSiblingsPositionsOnDistanceWithSouthDirection()
        {
            var startingPositionOnLeftTopCorner = Factories.CreatePosition(1, 1);
            var sut = new Battleground(Factories.FormBattlegroundPositions(3, 2));
            var expectedSiblingsPositionsOnSouthDirection = Factories.CreatePositions(
                startingPositionOnLeftTopCorner,
                Factories.CreatePosition(2, 1),
                Factories.CreatePosition(3, 1));
            var onDistanceWhichIsLongerThenColumnsAmountOnBattleground = sut.LastPosition.Column + 1;

            var actualResult = sut.GetRandomSiblingsPositions(
                onDistanceWhichIsLongerThenColumnsAmountOnBattleground, startingPositionOnLeftTopCorner.Coordinate);

            actualResult.Should().BeEquivalentTo(expectedSiblingsPositionsOnSouthDirection);
        }

        [Fact]
        public void GetRandomSiblingsPositions_WhenOnlyNorthDirectionAvailable_ThenReturnSiblingsPositionsOnDistanceWithNorthDirection()
        {
            var startingPositionOnRightBottomCorner = Factories.CreatePosition(3, 2);
            var sut = new Battleground(Factories.FormBattlegroundPositions(3, 2));
            var expectedSiblingsPositionsOnNorthDirection = Factories.CreatePositions(
                startingPositionOnRightBottomCorner,
                Factories.CreatePosition(2, 2),
                Factories.CreatePosition(1, 2));
            var onDistanceWhichIsLongerThenColumnsAmountOnBattleground = sut.LastPosition.Column + 1;

            var actualResult = sut.GetRandomSiblingsPositions(
                onDistanceWhichIsLongerThenColumnsAmountOnBattleground, startingPositionOnRightBottomCorner.Coordinate);

            actualResult.Should().BeEquivalentTo(expectedSiblingsPositionsOnNorthDirection);
        }
    }
}
