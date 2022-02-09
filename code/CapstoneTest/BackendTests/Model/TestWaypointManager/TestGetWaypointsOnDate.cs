using System;
using System.Collections.Generic;
using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CapstoneTest.BackendTests.Model.TestWaypointManager
{
    [TestClass]
    public class TestGetWaypointsOnDate
    {
        [TestMethod]
        public void Call_EmptySet_ReturnsEmptyList()
        {
            IList<Waypoint> fakeWaypoints = new List<Waypoint>();

            var mockWaypointDal = new Mock<WaypointDal>();
            mockWaypointDal.Setup(db => db.GetWaypyointsOnDate(1, DateTime.Now)).Returns(fakeWaypoints);

            WaypointManager waypointManager = new(mockWaypointDal.Object);

            var resultResponse =
                waypointManager.GetWaypointsOnDate(1, DateTime.Now);

            Assert.AreEqual(0, resultResponse.Data?.Count);
        }

        [TestMethod]
        public void Call_YieldsSetWithOneValue_ReturnsExpectedList()
        {
            IList<Waypoint> fakeWaypoints = new List<Waypoint>
            {
                new Waypoint
                {
                    TripId = 1,
                    WaypointNum = 1,
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now
                }
            };

            var mockWaypointDal = new Mock<WaypointDal>();
            mockWaypointDal.Setup(db => db.CreateWaypoint(1, "1601 Maple St", DateTime.Now, DateTime.Now))
                .Returns(1);
            mockWaypointDal.Setup(db => db.GetWaypyointsOnDate(1, DateTime.Now)).Returns(fakeWaypoints);

            WaypointManager waypointManager = new(mockWaypointDal.Object);

            var resultResponse =
                waypointManager.GetWaypointsOnDate(1, DateTime.Now);

            Assert.AreEqual(1, resultResponse.Data?.Count);
        }
    }
}