using System;
using System.Collections.Generic;
using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CapstoneTest.BackendTests.Model.TestTransportationManager
{
    [TestClass]
    public class TestGetTransportationOnDate
    {
        [TestMethod]
        public void GetTransportationOnDate_EmptySet_ReturnsEmptyList()
        {
            var currentTime = DateTime.Now;
            IList<Transportation> transportation = new List<Transportation>();

            var mockTransportationDal = new Mock<TransportationDal>();
            mockTransportationDal.Setup(db => db.GetTransportationOnDate(1, currentTime)).Returns(transportation);

            TransportationManager transportationManager = new(mockTransportationDal.Object);

            var resultResponse =
                transportationManager.GetTransportationOnDate(1, currentTime);

            Assert.AreEqual(0, resultResponse.Data?.Count);
        }

        [TestMethod]
        public void GetTransportationOnDate_YieldsSetWithOneValue_ReturnsExpectedList()
        {
            var currentTime = DateTime.Now;
            IList<Transportation> transportation = new List<Transportation>
            {
                new()
                {
                    TripId = 1,
                    TransportationId = 1,
                    Method = "Car",
                    StartDate = currentTime,
                    EndDate = currentTime,
                    Notes = "notes"
                }
            };

            var mockTransportationDal = new Mock<TransportationDal>();
            mockTransportationDal.Setup(db => db.GetTransportationOnDate(1, currentTime)).Returns(transportation);

            TransportationManager transportationManager = new(mockTransportationDal.Object);

            var resultResponse =
                transportationManager.GetTransportationOnDate(1, currentTime);

            Assert.AreEqual(1, resultResponse.Data?.Count);
            Assert.AreEqual(1, resultResponse.Data?[0].TripId);
            Assert.AreEqual(1, resultResponse.Data?[0].TransportationId);
            Assert.AreEqual("Car", resultResponse.Data?[0].Method);
            Assert.AreEqual("Car", resultResponse.Data?[0].DisplayName);
            Assert.AreEqual(currentTime, resultResponse.Data?[0].StartDate);
            Assert.AreEqual(currentTime, resultResponse.Data?[0].EndDate);
            Assert.AreEqual("notes", resultResponse.Data?[0].Notes);
            Assert.AreEqual(1, resultResponse.Data?[0].Id);
            Assert.AreEqual(nameof(Transportation), resultResponse.Data?[0].EventType);
        }

        [TestMethod]
        public void GetTransportationOnDate_YieldsSetWithMultipleValues_ReturnsExpectedList()
        {
            var currentTime = DateTime.Now;
            IList<Transportation> transportation = new List<Transportation>
            {
                new()
                {
                    TripId = 1,
                    TransportationId = 1,
                    Method = "Car",
                    StartDate = currentTime,
                    EndDate = currentTime
                },
                new()
                {
                    TripId = 1,
                    TransportationId = 2,
                    Method = "Bus",
                    StartDate = currentTime,
                    EndDate = currentTime
                }
            };

            var mockTransportationDal = new Mock<TransportationDal>();

            mockTransportationDal.Setup(db => db.CreateTransportation(1, "Car", currentTime, currentTime, null))
                .Returns(1);
            mockTransportationDal.Setup(db => db.CreateTransportation(1, "Bus", currentTime, currentTime, null))
                .Returns(1);
            mockTransportationDal.Setup(db => db.GetTransportationOnDate(1, currentTime)).Returns(transportation);

            TransportationManager transportationManager = new(mockTransportationDal.Object);

            var resultResponse =
                transportationManager.GetTransportationOnDate(1, currentTime);

            Assert.AreEqual(2, resultResponse.Data?.Count);
        }

        [TestMethod]
        public void GetTransportationOnDate_ServerMySqlException_ReturnsErrorMessage()
        {
            var mockDal = new Mock<TransportationDal>();
            var builder = new MySqlExceptionBuilder();
            var currentTime = DateTime.Now;
            mockDal.Setup(dal => dal.GetTransportationOnDate(1, currentTime))
                .Throws(builder
                    .WithError((uint) Ui.StatusCode.InternalServerError, Ui.ErrorMessages.InternalServerError).Build());

            TransportationManager transportationManager = new(mockDal.Object);

            var result = transportationManager.GetTransportationOnDate(1, currentTime);

            Assert.AreEqual((uint) Ui.StatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual(Ui.ErrorMessages.InternalServerError, result.ErrorMessage);
        }

        [TestMethod]
        public void GetTransportationOnDate_ServerException_ReturnsErrorMessage()
        {
            var mockDal = new Mock<TransportationDal>();
            var currentTime = DateTime.Now;
            mockDal.Setup(dal => dal.GetTransportationOnDate(1, currentTime))
                .Throws(new Exception());

            TransportationManager transportationManager = new(mockDal.Object);

            var result = transportationManager.GetTransportationOnDate(1, currentTime);

            Assert.AreEqual((uint) Ui.StatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual(Ui.ErrorMessages.InternalServerError, result.ErrorMessage);
        }
    }
}