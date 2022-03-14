using System;
using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CapstoneTest.BackendTests.Model.TestLodgingManager
{
    [TestClass]
    public class TestRemoveLodging
    {
        [TestMethod]
        public void RemoveLodging_LodgingNotExists_ReturnsErrorMessage()
        {
            var mockLodgingDal = new Mock<LodgingDal>();
            mockLodgingDal.Setup(db => db.CreateLodging(1, "vacation", DateTime.Today, DateTime.Today, string.Empty))
                .Returns(1);
            mockLodgingDal.Setup(db => db.RemoveLodging(2)).Returns(false);

            LodgingManager manager = new(mockLodgingDal.Object);

            var resultResponse = manager.RemoveLodging(2);

            Assert.AreEqual((uint) Ui.StatusCode.BadRequest, resultResponse.StatusCode);
            Assert.AreEqual(Ui.ErrorMessages.LodgingNotFound, resultResponse.ErrorMessage);
        }

        [TestMethod]
        public void RemoveLodging_ValidLodgingId_ReturnsTrue()
        {
            var mockLodgingDal = new Mock<LodgingDal>();
            mockLodgingDal.Setup(db => db.CreateLodging(1, "vacation", DateTime.Today, DateTime.Today, string.Empty))
                .Returns(1);
            mockLodgingDal.Setup(db => db.RemoveLodging(1)).Returns(true);

            LodgingManager manager = new(mockLodgingDal.Object);

            var resultResponse = manager.RemoveLodging(1);

            Assert.AreEqual((uint) Ui.StatusCode.Success, resultResponse.StatusCode);
            Assert.AreEqual(true, resultResponse.Data);
        }

        [TestMethod]
        public void RemoveLodging_ServerMySqlException_Failure()
        {
            var mockDal = new Mock<LodgingDal>();
            var builder = new MySqlExceptionBuilder();
            mockDal.Setup(dal => dal.RemoveLodging(1))
                .Throws(builder
                    .WithError((uint) Ui.StatusCode.InternalServerError, Ui.ErrorMessages.InternalServerError).Build());
            var lodgingManager = new LodgingManager(mockDal.Object);
            var result = lodgingManager.RemoveLodging(1);
            Assert.AreEqual((uint) Ui.StatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual(Ui.ErrorMessages.InternalServerError, result.ErrorMessage);
        }

        [TestMethod]
        public void RemoveLodging_ServerException_Failure()
        {
            var mockDal = new Mock<LodgingDal>();
            mockDal.Setup(dal => dal.RemoveLodging(1))
                .Throws(new Exception());
            var lodgingManager = new LodgingManager(mockDal.Object);
            var result = lodgingManager.RemoveLodging(1);
            Assert.AreEqual((uint) Ui.StatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual(Ui.ErrorMessages.InternalServerError, result.ErrorMessage);
        }
    }
}