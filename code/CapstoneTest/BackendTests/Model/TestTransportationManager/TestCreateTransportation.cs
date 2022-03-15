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
        public void CreateTransportation_InvalidTimes_ReturnsErrorMessage()
        {
            var mockTransportationDal = new Mock<TransportationDal>();
            mockTransportationDal.Setup(db =>
                    db.CreateTransportation(1, "Car", DateTime.Now.AddDays(4), DateTime.Now, null))
                .Returns((int) Ui.StatusCode.BadRequest);

            TransportationManager transportationManager = new(mockTransportationDal.Object);

            var resultResponse =
                transportationManager.CreateTransportation(1, "Car", DateTime.Now.AddDays(4), DateTime.Now, null);

            Assert.AreEqual((uint) Ui.StatusCode.BadRequest, resultResponse.StatusCode);
        }

        [TestMethod]
        public void CreateTransportation_MySqlException_ReturnsErrorMessage()
        {
            var mockTransportationDal = new Mock<TransportationDal>();
            var builder = new MySqlExceptionBuilder();
            var currentTime = DateTime.Now;
            mockTransportationDal.Setup(db =>
                    db.CreateTransportation(1, "Car", currentTime, currentTime.AddDays(2), "notes"))
                .Throws(builder
                    .WithError((uint) Ui.StatusCode.InternalServerError, Ui.ErrorMessages.InternalServerError).Build());

            TransportationManager transportationManager = new(mockTransportationDal.Object);

            var resultResponse =
                transportationManager.CreateTransportation(1, "Car", currentTime, currentTime.AddDays(2), "notes");

            Assert.AreEqual((uint) Ui.StatusCode.InternalServerError, resultResponse.StatusCode);
            Assert.AreEqual(Ui.ErrorMessages.InternalServerError, resultResponse.ErrorMessage);
        }

        [TestMethod]
        public void CreateTransportation_InternalServerError_ReturnsErrorMessage()
        {
            var mockTransportationDal = new Mock<TransportationDal>();
            var currentTime = DateTime.Now;
            mockTransportationDal.Setup(db =>
                    db.CreateTransportation(1, "Car", currentTime, currentTime.AddDays(2), "notes"))
                .Throws(new Exception());

            TransportationManager transportationManager = new(mockTransportationDal.Object);

            var resultResponse =
                transportationManager.CreateTransportation(1, "Car", currentTime, currentTime.AddDays(2), "notes");

            Assert.AreEqual((uint) Ui.StatusCode.InternalServerError, resultResponse.StatusCode);
            Assert.AreEqual(Ui.ErrorMessages.InternalServerError, resultResponse.ErrorMessage);
        }

        [TestMethod]
        public void CreateTransportation_ValidParameters_ReturnsTransportationNumber()
        {
            var mockTransportationDal = new Mock<TransportationDal>();
            mockTransportationDal.Setup(db =>
                    db.CreateTransportation(1, "Car", DateTime.Now, DateTime.Now.AddDays(2), "notes"))
                .Returns((int) Ui.StatusCode.Success);

            TransportationManager transportationManager = new(mockTransportationDal.Object);

            var resultResponse =
                transportationManager.CreateTransportation(1, "Car", DateTime.Now, DateTime.Now.AddDays(2), "notes");

            Assert.AreEqual((uint) Ui.StatusCode.Success, resultResponse.StatusCode);
        }
    }
}