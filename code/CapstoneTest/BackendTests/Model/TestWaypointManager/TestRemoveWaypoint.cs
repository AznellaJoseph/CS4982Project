using System;
using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CapstoneTest.BackendTests.Model.TestWaypointManager
{
    [TestClass]
    public class TestRemoveWaypoint
    {
        [TestMethod]
        public void RemoveWaypoint_WaypointNotExists_ReturnsErrorMessage()
        {
            var mockWaypointDal = new Mock<WaypointDal>();
            mockWaypointDal.Setup(db => db.CreateWaypoint(1, "vacation", DateTime.Today, DateTime.Today, string.Empty))
                .Returns(1);
            mockWaypointDal.Setup(db => db.RemoveWaypoint(2)).Returns(false);

            WaypointManager manager = new(mockWaypointDal.Object);

            var resultResponse = manager.RemoveWaypoint(2);

            Assert.AreEqual((uint) Ui.StatusCode.BadRequest, resultResponse.StatusCode);
            Assert.AreEqual(Ui.ErrorMessages.WaypointNotFound, resultResponse.ErrorMessage);
        }

        [TestMethod]
        public void RemoveWaypoint_ValidWaypointId_ReturnsTrue()
        {
            var mockWaypointDal = new Mock<WaypointDal>();
            mockWaypointDal.Setup(db => db.CreateWaypoint(1, "vacation", DateTime.Today, DateTime.Today, string.Empty))
                .Returns(1);
            mockWaypointDal.Setup(db => db.RemoveWaypoint(1)).Returns(true);

            WaypointManager manager = new(mockWaypointDal.Object);

            var resultResponse = manager.RemoveWaypoint(1);

            Assert.AreEqual((uint) Ui.StatusCode.Success, resultResponse.StatusCode);
            Assert.AreEqual(true, resultResponse.Data);
        }

        [TestMethod]
        public void RemoveWaypoint_ServerMySqlException_Failure()
        {
            var mockDal = new Mock<WaypointDal>();
            var builder = new MySqlExceptionBuilder();
            mockDal.Setup(dal => dal.RemoveWaypoint(1))
                .Throws(builder
                    .WithError((uint) Ui.StatusCode.InternalServerError, Ui.ErrorMessages.InternalServerError).Build());
            var waypointManager = new WaypointManager(mockDal.Object);
            var result = waypointManager.RemoveWaypoint(1);
            Assert.AreEqual((uint) Ui.StatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual(Ui.ErrorMessages.InternalServerError, result.ErrorMessage);
        }

        [TestMethod]
        public void RemoveWaypoint_ServerException_Failure()
        {
            var mockDal = new Mock<WaypointDal>();
            mockDal.Setup(dal => dal.RemoveWaypoint(1))
                .Throws(new Exception());
            var waypointManager = new WaypointManager(mockDal.Object);
            var result = waypointManager.RemoveWaypoint(1);
            Assert.AreEqual((uint) Ui.StatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual(Ui.ErrorMessages.InternalServerError, result.ErrorMessage);
        }
    }
}