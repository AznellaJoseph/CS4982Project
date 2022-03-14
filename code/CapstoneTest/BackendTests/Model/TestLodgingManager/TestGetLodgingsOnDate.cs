using System;
using System.Collections.Generic;
using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CapstoneTest.BackendTests.Model.TestLodgingManager
{
    [TestClass]
    public class TestGetLodgingsOnDate
    {
        [TestMethod]
        public void Call_EmptySet_ReturnsEmptyList()
        {
            var currentTime = DateTime.Now;

            IList<Lodging> fakeLodgings = new List<Lodging>();

            var mockLodgingDal = new Mock<LodgingDal>();
            mockLodgingDal.Setup(db => db.GetLodgingsOnDate(1, currentTime)).Returns(fakeLodgings);

            LodgingManager lodgingManager = new(mockLodgingDal.Object);

            var resultResponse =
                lodgingManager.GetLodgingsOnDate(1, currentTime);

            Assert.AreEqual(0, resultResponse.Data?.Count);
        }

        [TestMethod]
        public void Call_YieldsSetWithOneValue_ReturnsExpectedList()
        {
            var currentTime = DateTime.Now;
            IList<Lodging> fakeLodgings = new List<Lodging>
            {
                new()
                {
                    TripId = 1,
                    LodgingId = 1,
                    Location = "Some Hotel",
                    StartDate = currentTime,
                    EndDate = currentTime,
                    Notes = "notes"
                }
            };

            var mockLodgingDal = new Mock<LodgingDal>();

            mockLodgingDal.Setup(db => db.CreateLodging(1, "Some Hotel", currentTime, currentTime, "notes"))
                .Returns(1);
            mockLodgingDal.Setup(db => db.GetLodgingsOnDate(1, currentTime)).Returns(fakeLodgings);

            LodgingManager lodgingManager = new(mockLodgingDal.Object);

            var resultResponse =
                lodgingManager.GetLodgingsOnDate(1, currentTime);

            Assert.AreEqual(1, resultResponse.Data?.Count);
            Assert.AreEqual(1, resultResponse.Data?[0].TripId);
            Assert.AreEqual(1, resultResponse.Data?[0].LodgingId);
            Assert.AreEqual("Some Hotel", resultResponse.Data?[0].Location);
            Assert.AreEqual(currentTime, resultResponse.Data?[0].StartDate);
            Assert.AreEqual(currentTime, resultResponse.Data?[0].EndDate);
            Assert.AreEqual("notes", resultResponse.Data?[0].Notes);
        }


        [TestMethod]
        public void Call_YieldsSetWithMultipleValues_ReturnsExpectedList()
        {
            var currentTime = DateTime.Now;
            IList<Lodging> fakeLodgings = new List<Lodging>
            {
                new()
                {
                    TripId = 1,
                    LodgingId = 1,
                    Location = "Some Hotel",
                    StartDate = currentTime,
                    EndDate = currentTime
                },
                new()
                {
                    TripId = 1,
                    LodgingId = 2,
                    Location = "1602 Maple St",
                    StartDate = currentTime,
                    EndDate = currentTime
                }
            };

            var mockLodgingDal = new Mock<LodgingDal>();

            mockLodgingDal.Setup(db => db.CreateLodging(1, "Some Hotel", currentTime, currentTime, null))
                .Returns(1);
            mockLodgingDal.Setup(db => db.CreateLodging(1, "1602 Maple St", currentTime, currentTime, null))
                .Returns(1);
            mockLodgingDal.Setup(db => db.GetLodgingsOnDate(1, currentTime)).Returns(fakeLodgings);

            LodgingManager lodgingManager = new(mockLodgingDal.Object);

            var resultResponse =
                lodgingManager.GetLodgingsOnDate(1, currentTime);

            Assert.AreEqual(2, resultResponse.Data?.Count);
        }

        [TestMethod]
        public void Call_ServerMySqlException_Failure()
        {
            var mockDal = new Mock<LodgingDal>();
            var builder = new MySqlExceptionBuilder();
            var currentTime = DateTime.Now;
            mockDal.Setup(dal => dal.GetLodgingsOnDate(1, currentTime))
                .Throws(builder
                    .WithError((uint) Ui.StatusCode.InternalServerError, Ui.ErrorMessages.InternalServerError).Build());
            var lodgingManager = new LodgingManager(mockDal.Object);
            var result = lodgingManager.GetLodgingsOnDate(1, currentTime);
            Assert.AreEqual((uint) Ui.StatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual(Ui.ErrorMessages.InternalServerError, result.ErrorMessage);
        }

        [TestMethod]
        public void Call_ServerException_Failure()
        {
            var mockDal = new Mock<LodgingDal>();
            var currentTime = DateTime.Now;
            mockDal.Setup(dal => dal.GetLodgingsOnDate(1, currentTime))
                .Throws(new Exception());
            var lodgingManager = new LodgingManager(mockDal.Object);
            var result = lodgingManager.GetLodgingsOnDate(1, currentTime);
            Assert.AreEqual((uint) Ui.StatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual(Ui.ErrorMessages.InternalServerError, result.ErrorMessage);
        }
    }
}