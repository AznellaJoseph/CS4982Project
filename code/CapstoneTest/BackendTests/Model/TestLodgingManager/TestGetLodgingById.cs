using System;
using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CapstoneTest.BackendTests.Model.TestLodgingManager
{
    [TestClass]
    public class TestGetTransportationById
    {
        [TestMethod]
        public void GetLodgingById_NoLodgingFound_ReturnsErrorMessage()
        {
            Lodging? lodging = null;
            var mockDal = new Mock<LodgingDal>();
            mockDal.Setup(dal => dal.GetLodgingById(1)).Returns(lodging);

            LodgingManager lodgingManager = new(mockDal.Object);

            var resultResponse = lodgingManager.GetLodgingById(1);

            Assert.AreEqual((uint) Ui.StatusCode.DataNotFound, resultResponse.StatusCode);
            Assert.AreEqual(Ui.ErrorMessages.LodgingNotFound, resultResponse.ErrorMessage);
        }

        [TestMethod]
        public void GetLodgingById_ServerMySqlException_ReturnsErrorMessage()
        {
            var builder = new MySqlExceptionBuilder();
            var mockDal = new Mock<LodgingDal>();
            mockDal.Setup(dal => dal.GetLodgingById(1)).Throws(builder
                .WithError((uint) Ui.StatusCode.InternalServerError, Ui.ErrorMessages.InternalServerError).Build());

            LodgingManager lodgingManager = new(mockDal.Object);

            var resultResponse = lodgingManager.GetLodgingById(1);

            Assert.AreEqual((uint) Ui.StatusCode.InternalServerError, resultResponse.StatusCode);
            Assert.AreEqual(Ui.ErrorMessages.InternalServerError, resultResponse.ErrorMessage);
        }

        [TestMethod]
        public void GetLodgingById_ServerException_ReturnsErrorMessage()
        {
            var mockDal = new Mock<LodgingDal>();
            mockDal.Setup(dal => dal.GetLodgingById(1)).Throws(new Exception());

            LodgingManager lodgingManager = new(mockDal.Object);

            var resultResponse = lodgingManager.GetLodgingById(1);

            Assert.AreEqual((uint) Ui.StatusCode.InternalServerError, resultResponse.StatusCode);
            Assert.AreEqual(Ui.ErrorMessages.InternalServerError, resultResponse.ErrorMessage);
        }

        [TestMethod]
        public void GetLodgingById_Success()
        {
            Lodging lodging = new();
            var mockDal = new Mock<LodgingDal>();
            mockDal.Setup(dal => dal.GetLodgingById(1)).Returns(lodging);

            LodgingManager lodgingManager = new(mockDal.Object);

            var resultResponse = lodgingManager.GetLodgingById(1);

            Assert.IsNotNull(resultResponse.Data);
            Assert.AreEqual((uint) Ui.StatusCode.Success, resultResponse.StatusCode);
        }
    }
}