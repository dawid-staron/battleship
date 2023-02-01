using System.Collections.Generic;
using FluentAssertions;
using GameComponents.Playground.BuildingBlocks;
using Xunit;

namespace GameComponents.Tests.Playground
{
    public class PositionTests
    {
        [Fact]
        public void Construct_InitialStatus_IsUntouched()
        {
            var expectedInitialStatus = PositionStatus.Untouched;

            var initialStatus = new Position(new Coordinate(1, 1)).Status;

            initialStatus.Should().Be(expectedInitialStatus);
        }

        [Fact]
        public void ChangeStatus_CurrentStatusIsChangeIntoNewOne()
        {
            var expectedStatus = PositionStatus.OnFire;
            var sut = new Position(new Coordinate(1, 1));

            sut.ChangeStatus(PositionStatus.OnFire);

            sut.Status.Should().Be(expectedStatus);
        }

        [Theory]
        [MemberData(nameof(PositionStatusesComparing))]
        public void WhenTwoStatusesAreTheSame_ThenAreEqualToEachOther(
            PositionStatus leftStatus, PositionStatus rightStatus, bool areEqual)
        {
            (leftStatus == rightStatus).Should().Be(areEqual);
        }

        public static readonly IEnumerable<object[]> PositionStatusesComparing = new List<object[]>
        {
            new object[]
            {
                PositionStatus.Empty,
                PositionStatus.Sank,
                false
            },
            new object[]
            {
                null,
                PositionStatus.Sank,
                false
            },
            new object[]
            {
                PositionStatus.Empty,
                null,
                false
            },
            new object[]
            {
                null,
                null,
                true
            },
            new object[]
            {
                PositionStatus.OnFire,
                PositionStatus.OnFire,
                true
            },
            new object[]
            {
                PositionStatus.Empty,
                PositionStatus.Empty,
                true
            },
            new object[]
            {
                PositionStatus.Sank,
                PositionStatus.Sank,
                true
            },
            new object[]
            {
                PositionStatus.Untouched,
                PositionStatus.Untouched,
                true
            },
        };
    }
}