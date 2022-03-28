using System;
using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CapstoneTest.BackendTests.Model.TestTransportationManager
{
    [TestClass]
    public class TestRemoveTransportation
    {
        [TestMethod]
        public void RemoveTransportation_TransportationDoesNotExist_ReturnsErrorMessage()
        {
            var mock = new Mock<TransportationDal>();
            mock.Setup(db => db.RemoveTransportation(2)).Returns(false);

            TransportationManager manager = new(mock.Object);
            var resultResponse = manager.RemoveTransportation(2);

            Assert.AreEqual((uint) Ui.StatusCode.BadRequest, resultResponse.StatusCode);
            Assert.AreEqual(Ui.ErrorMessages.TransportationNotFound, resultResponse.ErrorMessage);
        }

        [TestMethod]
        public void RemoveTransportation_ValidTransportationId_ReturnsTrue()
        {
            var mock = new Mock<TransportationDal>();
            mock.Setup(db => db.RemoveTransportation(1)).Returns(true);

            TransportationManager manager = new(mock.Object);
            var resultResponse = manager.RemoveTransportation(1);

            Assert.AreEqual((uint) Ui.StatusCode.Success, resultResponse.StatusCode);
            Assert.AreEqual(true, resultResponse.Data);
        }

        [TestMethod]
        public void RemoveTransportation_ServerMySqlException_ReturnsErrorMessage()
        {
            var mockDal = new Mock<TransportationDal>();
            var builder = new MySqlExceptionBuilder();
            mockDal.Setup(dal => dal.RemoveTransportation(1))
                .Throws(builder
                    .WithError((uint)Ui.StatusCode.InternalServerError, Ui.ErrorMessages.InternalServerError).Build());

            TransportationManager lodgingManager = new(mockDal.Object);

            var result = lodgingManager.RemoveTransportation(1);

            Assert.AreEqual((uint)Ui.StatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual(Ui.ErrorMessages.InternalServerError, result.ErrorMessage);
        }

        [TestMethod]
        public void RemoveTransportation_ServerException_ReturnsErrorMessage()
        {
            var mockDal = new Mock<TransportationDal>();
            mockDal.Setup(dal => dal.RemoveTransportation(1))
                .Throws(new Exception());

            TransportationManager transportationManager = new(mockDal.Object);

            var result = transportationManager.RemoveTransportation(1);

            Assert.AreEqual((uint)Ui.StatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual(Ui.ErrorMessages.InternalServerError, result.ErrorMessage);
        }

    }
}