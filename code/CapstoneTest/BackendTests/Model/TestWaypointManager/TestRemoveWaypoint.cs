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

            Assert.AreEqual((uint)Ui.StatusCode.BadRequest, resultResponse.StatusCode);
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

            Assert.AreEqual((uint)Ui.StatusCode.Success, resultResponse.StatusCode);
            Assert.AreEqual(true, resultResponse.Data);
        }
    }
}