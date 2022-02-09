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
            mockWaypointDal.Setup(db => db.CreateWaypoint(1, "1601 Maple St", DateTime.Now.AddDays(4), DateTime.Now))
                .Returns(400);

            WaypointManager waypointManager = new(mockWaypointDal.Object);

            var resultResponse =
                waypointManager.CreateWaypoint(1, "1601 Maple St", DateTime.Now.AddDays(4), DateTime.Now);

            Assert.AreEqual(400, resultResponse.StatusCode);
        }

        [TestMethod]
        public void Create_ValidParameters_ReturnsWaypointNumber()
        {
            var mockWaypointDal = new Mock<WaypointDal>();
            mockWaypointDal.Setup(db => db.CreateWaypoint(1, "1601 Maple St", DateTime.Now, DateTime.Now.AddDays(2)))
                .Returns(200);

            WaypointManager waypointManager = new(mockWaypointDal.Object);

            var resultResponse =
                waypointManager.CreateWaypoint(1, "1601 Maple St", DateTime.Now, DateTime.Now.AddDays(2));

            Assert.AreEqual(200, resultResponse.StatusCode);

        }
    }
}