using System;
using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
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
                .Returns((int) Ui.StatusCode.BadRequest);

            TransportationManager transportationManager = new(mockTransportationDal.Object);

            var resultResponse =
                transportationManager.CreateTransportation(1, "Car", DateTime.Now.AddDays(4), DateTime.Now);

            Assert.AreEqual((uint) Ui.StatusCode.BadRequest, resultResponse.StatusCode);
        }

        [TestMethod]
        public void Create_ValidParameters_ReturnsWaypointNumber()
        {
            var mockTransportationDal = new Mock<TransportationDal>();
            mockTransportationDal.Setup(db =>
                    db.CreateTransportation(1, "Car", DateTime.Now, DateTime.Now.AddDays(2)))
                .Returns((int) Ui.StatusCode.Success);

            TransportationManager transportationManager = new(mockTransportationDal.Object);

            var resultResponse =
                transportationManager.CreateTransportation(1, "Car", DateTime.Now, DateTime.Now.AddDays(2));

            Assert.AreEqual((uint) Ui.StatusCode.Success, resultResponse.StatusCode);
        }
    }
}