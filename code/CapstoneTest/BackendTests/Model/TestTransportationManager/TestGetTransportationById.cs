using System;
using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CapstoneTest.BackendTests.Model.TestTransportationManager
{
    [TestClass]
    public class TestGetTransportationById
    {
        [TestMethod]
        public void GetTransportationById_NoTransportationFound_ReturnsErrorMessage()
        {
            Transportation? transportation = null;
            var mockDal = new Mock<TransportationDal>();
            mockDal.Setup(dal => dal.GetTransportationById(1)).Returns(transportation);

            TransportationManager transportationManager = new(mockDal.Object);

            var resultResponse = transportationManager.GetTransportationById(1);

            Assert.AreEqual((uint) Ui.StatusCode.DataNotFound, resultResponse.StatusCode);
            Assert.AreEqual(Ui.ErrorMessages.TransportationNotFound, resultResponse.ErrorMessage);
        }

        [TestMethod]
        public void GetTransportationById_ServerMySqlException_ReturnsErrorMessage()
        {
            var builder = new MySqlExceptionBuilder();
            var mockDal = new Mock<TransportationDal>();
            mockDal.Setup(dal => dal.GetTransportationById(1)).Throws(builder
                .WithError((uint) Ui.StatusCode.InternalServerError, Ui.ErrorMessages.InternalServerError).Build());

            TransportationManager transportationManager = new(mockDal.Object);

            var resultResponse = transportationManager.GetTransportationById(1);

            Assert.AreEqual((uint) Ui.StatusCode.InternalServerError, resultResponse.StatusCode);
            Assert.AreEqual(Ui.ErrorMessages.InternalServerError, resultResponse.ErrorMessage);
        }

        [TestMethod]
        public void GetTransportationById_ServerException_ReturnsErrorMessage()
        {
            var mockDal = new Mock<TransportationDal>();
            mockDal.Setup(dal => dal.GetTransportationById(1)).Throws(new Exception());

            TransportationManager transportationManager = new(mockDal.Object);

            var resultResponse = transportationManager.GetTransportationById(1);

            Assert.AreEqual((uint) Ui.StatusCode.InternalServerError, resultResponse.StatusCode);
            Assert.AreEqual(Ui.ErrorMessages.InternalServerError, resultResponse.ErrorMessage);
        }

        [TestMethod]
        public void GetTransportationById_Success()
        {
            Transportation transportation = new();
            var mockDal = new Mock<TransportationDal>();
            mockDal.Setup(dal => dal.GetTransportationById(1)).Returns(transportation);

            TransportationManager transportationManager = new(mockDal.Object);

            var resultResponse = transportationManager.GetTransportationById(1);

            Assert.IsNotNull(resultResponse.Data);
            Assert.AreEqual((uint) Ui.StatusCode.Success, resultResponse.StatusCode);
        }
    }
}