using System;
using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CapstoneTest.BackendTests.Model.TestTransportationManager
{
    [TestClass]
    public class TestEditTransportation
    {
        [TestMethod]
        public void EditTransportation_TransportationDoesNotExist_ReturnsErrorMessage()
        {
            Transportation transportation = new()
            {
                EndDate = DateTime.Today.AddDays(3),
                StartDate = DateTime.Today,
                Method = "Hilton",
                TransportationId = 1,
                TripId = 1,
                Notes = "notes"
            };
            var mockTransportationDal = new Mock<TransportationDal>();
            mockTransportationDal.Setup(db => db.EditTransportation(transportation)).Returns(false);
            mockTransportationDal.Setup(db => db.RemoveTransportation(1)).Returns(true);

            TransportationManager manager = new(mockTransportationDal.Object);

            transportation.Notes = "Bring blankets";

            manager.RemoveTransportation(1);

            var resultResponse = manager.EditTransportation(transportation);

            Assert.AreEqual(resultResponse.ErrorMessage, Ui.ErrorMessages.TransportationNotFound);
            Assert.AreEqual(resultResponse.StatusCode, (uint) Ui.StatusCode.DataNotFound);
        }

        [TestMethod]
        public void EditTransportation_SuccessfulEdit_ReturnsTrue()
        {
            Transportation transportation = new()
            {
                EndDate = DateTime.Today.AddDays(3),
                StartDate = DateTime.Today,
                Method = "Hilton",
                TransportationId = 1,
                TripId = 1,
                Notes = "notes"
            };
            var mockTransportationDal = new Mock<TransportationDal>();
            mockTransportationDal.Setup(db => db.EditTransportation(transportation)).Returns(true);

            TransportationManager manager = new(mockTransportationDal.Object);

            transportation.Notes = "Bring blankets";

            var resultResponse = manager.EditTransportation(transportation);

            Assert.IsTrue(resultResponse.Data);
        }

        [TestMethod]
        public void EditTransportation_ServerMySqlException_ReturnsErrorMessage()
        {
            Transportation transportation = new()
            {
                EndDate = DateTime.Today.AddDays(3),
                StartDate = DateTime.Today,
                Method = "Hilton",
                TransportationId = 1,
                TripId = 1,
                Notes = "notes"
            };
            var mockTransportationDal = new Mock<TransportationDal>();
            var builder = new MySqlExceptionBuilder();

            mockTransportationDal.Setup(db => db.EditTransportation(transportation)).Throws(
                builder.WithError((uint) Ui.StatusCode.InternalServerError, Ui.ErrorMessages.InternalServerError)
                    .Build());


            TransportationManager manager = new(mockTransportationDal.Object);

            transportation.Notes = "Bring blankets";

            var resultResponse = manager.EditTransportation(transportation);

            Assert.AreEqual(resultResponse.ErrorMessage, Ui.ErrorMessages.InternalServerError);
            Assert.AreEqual(resultResponse.StatusCode, (uint) Ui.StatusCode.InternalServerError);
        }

        [TestMethod]
        public void EditTransportation_ServerException_ReturnsErrorMessage()
        {
            Transportation transportation = new()
            {
                EndDate = DateTime.Today.AddDays(3),
                StartDate = DateTime.Today,
                Method = "Hilton",
                TransportationId = 1,
                TripId = 1,
                Notes = "notes"
            };
            var mockTransportationDal = new Mock<TransportationDal>();

            mockTransportationDal.Setup(db => db.EditTransportation(transportation)).Throws(
                new Exception());


            TransportationManager manager = new(mockTransportationDal.Object);

            transportation.Notes = "Bring blankets";

            var resultResponse = manager.EditTransportation(transportation);

            Assert.AreEqual(resultResponse.ErrorMessage, Ui.ErrorMessages.InternalServerError);
            Assert.AreEqual(resultResponse.StatusCode, (uint) Ui.StatusCode.InternalServerError);
        }
    }
}