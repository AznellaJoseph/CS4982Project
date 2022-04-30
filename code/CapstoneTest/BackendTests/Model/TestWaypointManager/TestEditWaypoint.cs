using System;
using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CapstoneTest.BackendTests.Model.TestWaypointManager
{
    [TestClass]
    public class TestEditWaypoint
    {
        [TestMethod]
        public void EditWaypoint_WaypointDoesNotExist_ReturnsErrorMessage()
        {
            Waypoint waypoint = new()
            {
                EndDate = DateTime.Today.AddDays(3),
                StartDate = DateTime.Today,
                Location = "Hilton",
                WaypointId = 1,
                TripId = 1,
                Notes = "notes"
            };
            var mockWaypointDal = new Mock<WaypointDal>();
            mockWaypointDal.Setup(db => db.EditWaypoint(waypoint)).Returns(false);
            mockWaypointDal.Setup(db => db.RemoveWaypoint(1)).Returns(true);

            WaypointManager manager = new(mockWaypointDal.Object);

            waypoint.Notes = "Bring blankets";

            manager.RemoveWaypoint(1);

            var resultResponse = manager.EditWaypoint(waypoint);

            Assert.AreEqual(resultResponse.ErrorMessage, Ui.ErrorMessages.WaypointNotFound);
            Assert.AreEqual(resultResponse.StatusCode, (uint) Ui.StatusCode.DataNotFound);
        }

        [TestMethod]
        public void EditWaypoint_SuccessfulEdit_ReturnsTrue()
        {
            Waypoint waypoint = new()
            {
                EndDate = DateTime.Today.AddDays(3),
                StartDate = DateTime.Today,
                Location = "Hilton",
                WaypointId = 1,
                TripId = 1,
                Notes = "notes"
            };
            var mockWaypointDal = new Mock<WaypointDal>();
            mockWaypointDal.Setup(db => db.EditWaypoint(waypoint)).Returns(true);

            WaypointManager manager = new(mockWaypointDal.Object);

            waypoint.Notes = "Bring blankets";

            var resultResponse = manager.EditWaypoint(waypoint);

            Assert.IsTrue(resultResponse.Data);
        }

        [TestMethod]
        public void EditWaypoint_ServerMySqlException_ReturnsErrorMessage()
        {
            Waypoint waypoint = new()
            {
                EndDate = DateTime.Today.AddDays(3),
                StartDate = DateTime.Today,
                Location = "Hilton",
                WaypointId = 1,
                TripId = 1,
                Notes = "notes"
            };
            var mockWaypointDal = new Mock<WaypointDal>();
            var builder = new MySqlExceptionBuilder();

            mockWaypointDal.Setup(db => db.EditWaypoint(waypoint)).Throws(
                builder.WithError((uint) Ui.StatusCode.InternalServerError, Ui.ErrorMessages.InternalServerError)
                    .Build());


            WaypointManager manager = new(mockWaypointDal.Object);

            waypoint.Notes = "Bring blankets";

            var resultResponse = manager.EditWaypoint(waypoint);

            Assert.AreEqual(resultResponse.ErrorMessage, Ui.ErrorMessages.InternalServerError);
            Assert.AreEqual(resultResponse.StatusCode, (uint) Ui.StatusCode.InternalServerError);
        }

        [TestMethod]
        public void EditWaypoint_ServerException_ReturnsErrorMessage()
        {
            Waypoint waypoint = new()
            {
                EndDate = DateTime.Today.AddDays(3),
                StartDate = DateTime.Today,
                Location = "Hilton",
                WaypointId = 1,
                TripId = 1,
                Notes = "notes"
            };
            var mockWaypointDal = new Mock<WaypointDal>();

            mockWaypointDal.Setup(db => db.EditWaypoint(waypoint)).Throws(
                new Exception());


            WaypointManager manager = new(mockWaypointDal.Object);

            waypoint.Notes = "Bring blankets";

            var resultResponse = manager.EditWaypoint(waypoint);

            Assert.AreEqual(resultResponse.ErrorMessage, Ui.ErrorMessages.InternalServerError);
            Assert.AreEqual(resultResponse.StatusCode, (uint) Ui.StatusCode.InternalServerError);
        }
    }
}