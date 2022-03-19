using System;
using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CapstoneTest.BackendTests.Model.TestLodgingManager
{
    [TestClass]
    public class TestCreateLodging
    {
        [TestMethod]
        public void CreateLodging_InvalidDates_ReturnsErrorMessage()
        {
            var mockLodgingDal = new Mock<LodgingDal>();
            mockLodgingDal.Setup(db =>
                    db.CreateLodging(1, "Some Hotel", DateTime.Now.AddDays(4), DateTime.Now, null))
                .Returns((int) Ui.StatusCode.BadRequest);

            LodgingManager lodgingManager = new(mockLodgingDal.Object);

            var resultResponse =
                lodgingManager.CreateLodging(1, "Some Hotel", DateTime.Now.AddDays(4), DateTime.Now, null);

            Assert.AreEqual((uint) Ui.StatusCode.BadRequest, resultResponse.StatusCode);
        }

        [TestMethod]
        public void CreateLodging_ServerMySqlException_ReturnsErrorMessage()
        {
            var mockDal = new Mock<LodgingDal>();
            var builder = new MySqlExceptionBuilder();
            var currentTime = DateTime.Now;
            mockDal.Setup(dal => dal.CreateLodging(1, "Some Hotel", currentTime, currentTime.AddDays(2), null))
                .Throws(builder
                    .WithError((uint) Ui.StatusCode.InternalServerError, Ui.ErrorMessages.InternalServerError).Build());

            LodgingManager lodgingManager = new(mockDal.Object);

            var result = lodgingManager.CreateLodging(1, "Some Hotel", currentTime, currentTime.AddDays(2), null);

            Assert.AreEqual((uint) Ui.StatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual(Ui.ErrorMessages.InternalServerError, result.ErrorMessage);
        }

        [TestMethod]
        public void CreateLodging_ServerException_ReturnsErrorMessage()
        {
            var mockDal = new Mock<LodgingDal>();
            var currentTime = DateTime.Now;
            mockDal.Setup(dal => dal.CreateLodging(1, "Some Hotel", currentTime, currentTime, null))
                .Throws(new Exception());

            LodgingManager lodgingManager = new(mockDal.Object);

            var result = lodgingManager.CreateLodging(1, "Some Hotel", currentTime, currentTime, null);
            
            Assert.AreEqual((uint) Ui.StatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual(Ui.ErrorMessages.InternalServerError, result.ErrorMessage);
        }


        [TestMethod]
        public void CreateLodging_Success()
        {
            var mockLodgingDal = new Mock<LodgingDal>();
            mockLodgingDal.Setup(db =>
                    db.CreateLodging(1, "Some Hotel", DateTime.Now, DateTime.Now.AddDays(2), null))
                .Returns((int) Ui.StatusCode.Success);

            LodgingManager lodgingManager = new(mockLodgingDal.Object);

            var resultResponse =
                lodgingManager.CreateLodging(1, "Some Hotel", DateTime.Now, DateTime.Now.AddDays(2), null);

            Assert.AreEqual((uint) Ui.StatusCode.Success, resultResponse.StatusCode);
        }
    }
}