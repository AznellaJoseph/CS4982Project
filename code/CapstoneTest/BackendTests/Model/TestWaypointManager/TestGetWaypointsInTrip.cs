using System;
using System.Collections.Generic;
using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CapstoneTest.BackendTests.Model.TestWaypointManager
{
    [TestClass]
    public class TestGetWaypointsInTrip
    {
        [TestMethod]
        public void Call_EmptySet_ReturnsEmptyList()
        {
            IList<Waypoint> fakeWaypoints = new List<Waypoint>();

            var mockWaypointDal = new Mock<WaypointDal>();
            mockWaypointDal.Setup(db => db.GetWaypointsByTripId(1)).Returns(fakeWaypoints);

            WaypointManager waypointManager = new(mockWaypointDal.Object);

            var resultResponse =
                waypointManager.GetWaypointsByTripId(1);

            Assert.AreEqual(0, resultResponse.Data?.Count);
        }

        [TestMethod]
        public void Call_YieldsSetWithOneValue_ReturnsExpectedList()
        {
            IList<Waypoint> fakeWaypoints = new List<Waypoint>
            {
                new()
                {
                    TripId = 1,
                    WaypointId = 1,
                    Location = "1601 Maple St",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now
                }
            };

            var mockWaypointDal = new Mock<WaypointDal>();
            mockWaypointDal.Setup(db => db.CreateWaypoint(1, "1601 Maple St", DateTime.Now, DateTime.Now, null))
                .Returns(1);
            mockWaypointDal.Setup(db => db.GetWaypointsByTripId(1)).Returns(fakeWaypoints);

            WaypointManager waypointManager = new(mockWaypointDal.Object);

            var resultResponse =
                waypointManager.GetWaypointsByTripId(1);

            Assert.AreEqual(1, resultResponse.Data?.Count);
        }

        [TestMethod]
        public void Call_YieldsSetWithMultipleValues_ReturnsExpectedList()
        {
            IList<Waypoint> fakeWaypoints = new List<Waypoint>
            {
                new()
                {
                    TripId = 1,
                    WaypointId = 1,
                    Location = "1601 Maple St",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now
                },
                new()
                {
                    TripId = 1,
                    WaypointId = 2,
                    Location = "1602 Maple St",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now
                }
            };

            var mockWaypointDal = new Mock<WaypointDal>();
            mockWaypointDal.Setup(db => db.CreateWaypoint(1, "1601 Maple St", DateTime.Now, DateTime.Now, null))
                .Returns(1);
            mockWaypointDal.Setup(db => db.CreateWaypoint(1, "1602 Maple St", DateTime.Now, DateTime.Now, null))
                .Returns(1);
            mockWaypointDal.Setup(db => db.GetWaypointsByTripId(1)).Returns(fakeWaypoints);

            WaypointManager waypointManager = new(mockWaypointDal.Object);

            var resultResponse =
                waypointManager.GetWaypointsByTripId(1);

            Assert.AreEqual(2, resultResponse.Data?.Count);
        }


        [TestMethod]
        public void Call_ServerMySqlException_Failure()
        {
            var mockDal = new Mock<WaypointDal>();
            var builder = new MySqlExceptionBuilder();
            mockDal.Setup(dal => dal.GetWaypointsByTripId(1))
                .Throws(builder.WithError((uint)Ui.StatusCode.InternalServerError, "test").Build());
            var waypointManager = new WaypointManager(mockDal.Object);
            var result = waypointManager.GetWaypointsByTripId(1);
            Assert.AreEqual((uint)Ui.StatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual("test", result.ErrorMessage);
        }

        [TestMethod]
        public void Call_ServerException_Failure()
        {
            var mockDal = new Mock<WaypointDal>();
            var currentTime = DateTime.Now;
            mockDal.Setup(dal => dal.GetWaypointsByTripId(1))
                .Throws(new Exception());
            var waypointManager = new WaypointManager(mockDal.Object);
            var result = waypointManager.GetWaypointsByTripId(1);
            Assert.AreEqual((uint)Ui.StatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual(Ui.ErrorMessages.InternalServerError, result.ErrorMessage);
        }

    }
}