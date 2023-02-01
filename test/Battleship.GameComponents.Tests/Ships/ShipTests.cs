using System.Collections.Generic;
using FluentAssertions;
using GameComponents.Playground.BuildingBlocks;
using GameComponents.Ships;
using GameComponents.Ships.Implementations;
using TestingUtilities;
using Xunit;

namespace GameComponents.Tests.Ships
{
    public class ShipTests
    {
        private IShip _sut;

        [Fact]
        public void GhostShip_ByDefaultIsDestroyed()
        {
            _sut = new GhostShip(Factories.CreatePositions(Factories.CreatePosition()));

            _sut.Destroyed.Should().BeTrue();
        }

        [Fact]
        public void GhostShip_CannotBeHit_AllAttacksAlwaysMisses()
        {
            var shipLocation = Factories.CreatePosition();
            _sut = new GhostShip(Factories.CreatePositions(shipLocation));

            _sut.AttackIncoming(shipLocation.Coordinate);

            shipLocation.Status.Should().Be(PositionStatus.Empty);
        }

        [Fact]
        public void GhostShip_ProtectionZoneCannotBeNeverViolated_BecauseItNotExist()
        {
            var shipLocation = Factories.CreatePosition();
            _sut = new GhostShip(Factories.CreatePositions(shipLocation));

            _sut.ProtectionZoneViolate(Factories.CreateCoordinates(shipLocation.Coordinate)).Should().BeFalse();
        }

        [Theory]
        [MemberData(nameof(StandardShipProtectionZoneViolated))]
        public void StandardShip_WhenRequestedCoordinateIsWithinOneSpaceMarginAroundShip_ThenProtectionZoneViolated(
            Position shipLocation, Coordinate overlapPositionWithProtectionZone)
        {
            _sut = new Ship(Factories.CreatePositions(shipLocation));

            _sut.ProtectionZoneViolate(Factories.CreateCoordinates(overlapPositionWithProtectionZone)).Should().BeTrue();
        }

        [Fact]
        public void StandardShip_WhenEntireShipLocationIsOnFire_ThenShipSunk()
        {
            var shipLocation = Factories.CreatePosition();
            _sut = new Ship(Factories.CreatePositions(shipLocation));

            _sut.AttackIncoming(shipLocation.Coordinate);

            shipLocation.Status.Should().Be(PositionStatus.Sank);
        }

        [Fact]
        public void StandardShip_WhenShipSunk_ThenDestroyed()
        {
            var shipLocation = Factories.CreatePosition();
            _sut = new Ship(Factories.CreatePositions(shipLocation));

            _sut.AttackIncoming(shipLocation.Coordinate);

            _sut.Destroyed.Should().BeTrue();
        }

        public static readonly IEnumerable<object[]> StandardShipProtectionZoneViolated = new List<object[]>
        {
            new object[]
            {
                Factories.CreatePosition(5, 5),
                Factories.CreateCoordinate(4, 4),
            },
            new object[]
            {
                Factories.CreatePosition(5, 5),
                Factories.CreateCoordinate(4, 5),
            },
            new object[]
            {
                Factories.CreatePosition(5, 5),
                Factories.CreateCoordinate(4, 6),
            },
            new object[]
            {
                Factories.CreatePosition(5, 5),
                Factories.CreateCoordinate(5, 4),
            },
            new object[]
            {
                Factories.CreatePosition(5, 5),
                Factories.CreateCoordinate(5, 5),
            },
            new object[]
            {
                Factories.CreatePosition(5, 5),
                Factories.CreateCoordinate(5, 6),
            },
            new object[]
            {
                Factories.CreatePosition(5, 5),
                Factories.CreateCoordinate(6, 4),
            },
            new object[]
            {
                Factories.CreatePosition(5, 5),
                Factories.CreateCoordinate(6, 5),
            },
            new object[]
            {
                Factories.CreatePosition(5, 5),
                Factories.CreateCoordinate(6, 6),
            },
        };
    }
}