using System;
using CapstoneBackend.DAL;
using CapstoneBackend.Model;
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
                .Returns(400);

            WaypointManager waypointManager = new(mockWaypointDal.Object);

            var resultResponse =
                waypointManager.CreateWaypoint(1, "1601 Maple St", DateTime.Now.AddDays(4), DateTime.Now, null);

            Assert.AreEqual(400u, resultResponse.StatusCode);
        }

        [TestMethod]
        public void Create_ValidParameters_ReturnsWaypointNumber()
        {
            var mockWaypointDal = new Mock<WaypointDal>();
            mockWaypointDal.Setup(db =>
                    db.CreateWaypoint(1, "1601 Maple St", DateTime.Now, DateTime.Now.AddDays(2), null))
                .Returns(200);

            WaypointManager waypointManager = new(mockWaypointDal.Object);

            var resultResponse =
                waypointManager.CreateWaypoint(1, "1601 Maple St", DateTime.Now, DateTime.Now.AddDays(2), null);

            Assert.AreEqual(200u, resultResponse.StatusCode);
        }
    }
}