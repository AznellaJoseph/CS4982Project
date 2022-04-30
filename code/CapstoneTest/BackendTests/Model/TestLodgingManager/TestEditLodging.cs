using System;
using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CapstoneTest.BackendTests.Model.TestLodgingManager
{
    [TestClass]
    public class TestEditLodging
    {
        [TestMethod]
        public void EditLodging_LodgingDoesNotExist_ReturnsErrorMessage()
        {
            Lodging lodging = new()
            {
                EndDate = DateTime.Today.AddDays(3),
                StartDate = DateTime.Today,
                Location = "Hilton",
                LodgingId = 1,
                TripId = 1,
                Notes = "notes"
            };
            var mockLodgingDal = new Mock<LodgingDal>();
            mockLodgingDal.Setup(db => db.EditLodging(lodging)).Returns(false);
            mockLodgingDal.Setup(db => db.RemoveLodging(1)).Returns(true);

            LodgingManager manager = new(mockLodgingDal.Object);

            lodging.Notes = "Bring blankets";

            manager.RemoveLodging(1);

            var resultResponse = manager.EditLodging(lodging);

            Assert.AreEqual(resultResponse.ErrorMessage, Ui.ErrorMessages.LodgingNotFound);
            Assert.AreEqual(resultResponse.StatusCode, (uint) Ui.StatusCode.DataNotFound);
        }

        [TestMethod]
        public void EditLodging_SuccessfulEdit_ReturnsTrue()
        {
            Lodging lodging = new()
            {
                EndDate = DateTime.Today.AddDays(3),
                StartDate = DateTime.Today,
                Location = "Hilton",
                LodgingId = 1,
                TripId = 1,
                Notes = "notes"
            };
            var mockLodgingDal = new Mock<LodgingDal>();
            mockLodgingDal.Setup(db => db.EditLodging(lodging)).Returns(true);

            LodgingManager manager = new(mockLodgingDal.Object);

            lodging.Notes = "Bring blankets";

            var resultResponse = manager.EditLodging(lodging);

            Assert.IsTrue(resultResponse.Data);
        }

        [TestMethod]
        public void EditLodging_ServerMySqlException_ReturnsErrorMessage()
        {
            Lodging lodging = new()
            {
                EndDate = DateTime.Today.AddDays(3),
                StartDate = DateTime.Today,
                Location = "Hilton",
                LodgingId = 1,
                TripId = 1,
                Notes = "notes"
            };
            var mockLodgingDal = new Mock<LodgingDal>();
            var builder = new MySqlExceptionBuilder();

            mockLodgingDal.Setup(db => db.EditLodging(lodging)).Throws(
                builder.WithError((uint) Ui.StatusCode.InternalServerError, Ui.ErrorMessages.InternalServerError)
                    .Build());


            LodgingManager manager = new(mockLodgingDal.Object);

            lodging.Notes = "Bring blankets";

            var resultResponse = manager.EditLodging(lodging);

            Assert.AreEqual(resultResponse.ErrorMessage, Ui.ErrorMessages.InternalServerError);
            Assert.AreEqual(resultResponse.StatusCode, (uint) Ui.StatusCode.InternalServerError);
        }

        [TestMethod]
        public void EditLodging_ServerException_ReturnsErrorMessage()
        {
            Lodging lodging = new()
            {
                EndDate = DateTime.Today.AddDays(3),
                StartDate = DateTime.Today,
                Location = "Hilton",
                LodgingId = 1,
                TripId = 1,
                Notes = "notes"
            };
            var mockLodgingDal = new Mock<LodgingDal>();

            mockLodgingDal.Setup(db => db.EditLodging(lodging)).Throws(
                new Exception());


            LodgingManager manager = new(mockLodgingDal.Object);

            lodging.Notes = "Bring blankets";

            var resultResponse = manager.EditLodging(lodging);

            Assert.AreEqual(resultResponse.ErrorMessage, Ui.ErrorMessages.InternalServerError);
            Assert.AreEqual(resultResponse.StatusCode, (uint) Ui.StatusCode.InternalServerError);
        }
    }
}