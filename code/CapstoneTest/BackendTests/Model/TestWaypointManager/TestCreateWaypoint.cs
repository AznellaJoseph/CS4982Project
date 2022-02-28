using System;
using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CapstoneTest.BackendTests.Model.TestWaypointManager
{
    [TestClass]
    public class TestCreateWaypoint
    {
        [TestMethod]
        public void Create_InvalidTimes_ReturnsErrorMessage()
        {
            var mockWaypointDal = new Mock<WaypointDal>();
            mockWaypointDal.Setup(db =>
                    db.CreateWaypoint(1, "1601 Maple St", DateTime.Now.AddDays(4), DateTime.Now, null))
                .Returns((int)Ui.StatusCode.BadRequest);

            WaypointManager waypointManager = new(mockWaypointDal.Object);

            var resultResponse =
                waypointManager.CreateWaypoint(1, "1601 Maple St", DateTime.Now.AddDays(4), DateTime.Now, null);

            Assert.AreEqual((uint)Ui.StatusCode.BadRequest, resultResponse.StatusCode);
        }

        [TestMethod]
        public void CreateWaypoint_ServerMySqlException_Failure()
        {
            var mockDal = new Mock<WaypointDal>();
            var builder = new MySqlExceptionBuilder();
            var currentTime = DateTime.Now;
            mockDal.Setup(dal => dal.CreateWaypoint(1, "1601 Maple St", currentTime, currentTime.AddDays(2), null))
                .Throws(builder.WithError((uint)Ui.StatusCode.InternalServerError, Ui.ErrorMessages.InternalServerError).Build());
            var waypointManager = new WaypointManager(mockDal.Object);
            var result = waypointManager.CreateWaypoint(1, "1601 Maple St", currentTime, currentTime.AddDays(2), null);
            Assert.AreEqual((uint)Ui.StatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual(Ui.ErrorMessages.InternalServerError, result.ErrorMessage);
        }

        [TestMethod]
        public void CreateWaypoint_ServerException_Failure()
        {
            var mockDal = new Mock<WaypointDal>();
            var currentTime = DateTime.Now;
            mockDal.Setup(dal => dal.CreateWaypoint(1, "1601 Maple St", currentTime, currentTime, null))
                .Throws(new Exception());
            var waypointManager = new WaypointManager(mockDal.Object);
            var result = waypointManager.CreateWaypoint(1, "1601 Maple St", currentTime, currentTime, null);
            Assert.AreEqual((uint)Ui.StatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual(Ui.ErrorMessages.InternalServerError, result.ErrorMessage);
        }


        [TestMethod]
        public void Create_ValidParameters_ReturnsWaypointNumber()
        {
            var mockWaypointDal = new Mock<WaypointDal>();
            mockWaypointDal.Setup(db =>
                    db.CreateWaypoint(1, "1601 Maple St", DateTime.Now, DateTime.Now.AddDays(2), null))
                .Returns((int)Ui.StatusCode.Success);

            WaypointManager waypointManager = new(mockWaypointDal.Object);

            var resultResponse =
                waypointManager.CreateWaypoint(1, "1601 Maple St", DateTime.Now, DateTime.Now.AddDays(2), null);

            Assert.AreEqual(Ui.StatusCode.Success, resultResponse.StatusCode);
        }
    }
}