using System;
using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CapstoneTest.BackendTests.Model.TestTransportationManager
{
    [TestClass]
    public class TestCreateTransportation
    {
        [TestMethod]
        public void Create_InvalidTimes_ReturnsErrorMessage()
        {
            var mockTransportationDal = new Mock<TransportationDal>();
            mockTransportationDal.Setup(db =>
                    db.CreateTransportation(1, "Car", DateTime.Now.AddDays(4), DateTime.Now))
                .Returns(400);

            TransportationManager transportationManager = new(mockTransportationDal.Object);

            var resultResponse =
                transportationManager.CreateWaypoint(1, "Car", DateTime.Now.AddDays(4), DateTime.Now);

            Assert.AreEqual(400u, resultResponse.StatusCode);
        }

        [TestMethod]
        public void Create_ValidParameters_ReturnsWaypointNumber()
        {
            var mockTransportationDal = new Mock<TransportationDal>();
            mockTransportationDal.Setup(db =>
                    db.CreateTransportation(1, "Car", DateTime.Now, DateTime.Now.AddDays(2)))
                .Returns(200);
            
            TransportationManager transportationManager = new(mockTransportationDal.Object);

            var resultResponse =
                transportationManager.CreateWaypoint(1, "Car", DateTime.Now, DateTime.Now.AddDays(2));

            Assert.AreEqual(200u, resultResponse.StatusCode);
        }
    }
}