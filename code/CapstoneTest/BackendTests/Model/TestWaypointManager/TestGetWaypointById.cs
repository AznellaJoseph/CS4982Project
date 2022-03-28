using System;
using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CapstoneTest.BackendTests.Model.TestWaypointManager
{
    [TestClass]
    public class TestGetWaypointById
    {
        [TestMethod]
        public void GetWaypointById_NoWaypointFound_ReturnsErrorMessage()
        {
            Waypoint? waypoint = null;
            var mockDal = new Mock<WaypointDal>();
            mockDal.Setup(dal => dal.GetWaypointById(1)).Returns(waypoint);

            WaypointManager waypointManager = new(mockDal.Object);

            var resultResponse = waypointManager.GetWaypointById(1);

            Assert.AreEqual((uint) Ui.StatusCode.DataNotFound, resultResponse.StatusCode);
            Assert.AreEqual(Ui.ErrorMessages.WaypointNotFound, resultResponse.ErrorMessage);
        }

        [TestMethod]
        public void GetWaypointById_ServerMySqlException_ReturnsErrorMessage()
        {
            var builder = new MySqlExceptionBuilder();
            var mockDal = new Mock<WaypointDal>();
            mockDal.Setup(dal => dal.GetWaypointById(1)).Throws(builder
                .WithError((uint) Ui.StatusCode.InternalServerError, Ui.ErrorMessages.InternalServerError).Build());

            WaypointManager waypointManager = new(mockDal.Object);

            var resultResponse = waypointManager.GetWaypointById(1);

            Assert.AreEqual((uint) Ui.StatusCode.InternalServerError, resultResponse.StatusCode);
            Assert.AreEqual(Ui.ErrorMessages.InternalServerError, resultResponse.ErrorMessage);
        }

        [TestMethod]
        public void GetWaypointById_ServerException_ReturnsErrorMessage()
        {
            var mockDal = new Mock<WaypointDal>();
            mockDal.Setup(dal => dal.GetWaypointById(1)).Throws(new Exception());

            WaypointManager waypointManager = new(mockDal.Object);

            var resultResponse = waypointManager.GetWaypointById(1);

            Assert.AreEqual((uint) Ui.StatusCode.InternalServerError, resultResponse.StatusCode);
            Assert.AreEqual(Ui.ErrorMessages.InternalServerError, resultResponse.ErrorMessage);
        }

        [TestMethod]
        public void GetWaypointById_Success()
        {
            Waypoint waypoint = new();
            var mockDal = new Mock<WaypointDal>();
            mockDal.Setup(dal => dal.GetWaypointById(1)).Returns(waypoint);

            WaypointManager waypointManager = new(mockDal.Object);

            var resultResponse = waypointManager.GetWaypointById(1);

            Assert.IsNotNull(resultResponse.Data);
            Assert.AreEqual((uint) Ui.StatusCode.Success, resultResponse.StatusCode);
        }
    }
}