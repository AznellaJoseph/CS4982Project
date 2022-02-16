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
            var currentTime = DateTime.Now;

            IList<Waypoint> fakeWaypoints = new List<Waypoint>();

            var mockWaypointDal = new Mock<WaypointDal>();
            mockWaypointDal.Setup(db => db.GetWaypointsOnDate(1, currentTime)).Returns(fakeWaypoints);

            WaypointManager waypointManager = new(mockWaypointDal.Object);

            var resultResponse =
                waypointManager.GetWaypointsOnDate(1, currentTime);

            Assert.AreEqual(0, resultResponse.Data?.Count);
        }

        [TestMethod]
        public void Call_YieldsSetWithOneValue_ReturnsExpectedList()
        {
            var currentTime = DateTime.Now;
            IList<Waypoint> fakeWaypoints = new List<Waypoint>
            {
                new()
                {
                    TripId = 1,
                    WaypointId = 1,
                    Location = "1601 Maple St",
                    StartDate = currentTime,
                    EndDate = currentTime,
                    Notes = "notes"
                }
            };

            var mockWaypointDal = new Mock<WaypointDal>();

            mockWaypointDal.Setup(db => db.CreateWaypoint(1, "1601 Maple St", currentTime, currentTime, "notes"))
                .Returns(1);
            mockWaypointDal.Setup(db => db.GetWaypointsOnDate(1, currentTime)).Returns(fakeWaypoints);

            WaypointManager waypointManager = new(mockWaypointDal.Object);

            var resultResponse =
                waypointManager.GetWaypointsOnDate(1, currentTime);

            Assert.AreEqual(1, resultResponse.Data?.Count);
            Assert.AreEqual(1, resultResponse.Data?[0].TripId);
            Assert.AreEqual(1, resultResponse.Data?[0].WaypointId);
            Assert.AreEqual("1601 Maple St", resultResponse.Data?[0].Location);
            Assert.AreEqual(currentTime, resultResponse.Data?[0].StartDate);
            Assert.AreEqual(currentTime, resultResponse.Data?[0].EndDate);
            Assert.AreEqual("notes", resultResponse.Data?[0].Notes);
        }


        [TestMethod]
        public void Call_YieldsSetWithMultipleValues_ReturnsExpectedList()
        {
            var currentTime = DateTime.Now;
            IList<Waypoint> fakeWaypoints = new List<Waypoint>
            {
                new()
                {
                    TripId = 1,
                    WaypointId = 1,
                    Location = "1601 Maple St",
                    StartDate = currentTime,
                    EndDate = currentTime
                },
                new()
                {
                    TripId = 1,
                    WaypointId = 2,
                    Location = "1602 Maple St",
                    StartDate = currentTime,
                    EndDate = currentTime
                }
            };

            var mockWaypointDal = new Mock<WaypointDal>();

            mockWaypointDal.Setup(db => db.CreateWaypoint(1, "1601 Maple St", currentTime, currentTime, null))
                .Returns(1);
            mockWaypointDal.Setup(db => db.CreateWaypoint(1, "1602 Maple St", currentTime, currentTime, null))
                .Returns(1);
            mockWaypointDal.Setup(db => db.GetWaypointsOnDate(1, currentTime)).Returns(fakeWaypoints);

            WaypointManager waypointManager = new(mockWaypointDal.Object);

            var resultResponse =
                waypointManager.GetWaypointsOnDate(1, currentTime);

            Assert.AreEqual(2, resultResponse.Data?.Count);
        }
    }
}