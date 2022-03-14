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
    public class TestGetLodgingsInTrip
    {
        [TestMethod]
        public void Call_EmptySet_ReturnsEmptyList()
        {
            IList<Lodging> fakeLodgings = new List<Lodging>();

            var mockLodgingDal = new Mock<LodgingDal>();
            mockLodgingDal.Setup(db => db.GetLodgingsByTripId(1)).Returns(fakeLodgings);

            LodgingManager lodgingManager = new(mockLodgingDal.Object);

            var resultResponse =
                lodgingManager.GetLodgingsByTripId(1);

            Assert.AreEqual(0, resultResponse.Data?.Count);
        }

        [TestMethod]
        public void Call_YieldsSetWithOneValue_ReturnsExpectedList()
        {
            IList<Lodging> fakeLodgings = new List<Lodging>
            {
                new()
                {
                    TripId = 1,
                    LodgingId = 1,
                    Location = "Some Hotel",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now
                }
            };

            var mockLodgingDal = new Mock<LodgingDal>();
            mockLodgingDal.Setup(db => db.CreateLodging(1, "Some Hotel", DateTime.Now, DateTime.Now, null))
                .Returns(1);
            mockLodgingDal.Setup(db => db.GetLodgingsByTripId(1)).Returns(fakeLodgings);

            LodgingManager lodgingManager = new(mockLodgingDal.Object);

            var resultResponse =
                lodgingManager.GetLodgingsByTripId(1);

            Assert.AreEqual(1, resultResponse.Data?.Count);
        }

        [TestMethod]
        public void Call_YieldsSetWithMultipleValues_ReturnsExpectedList()
        {
            IList<Lodging> fakeLodgings = new List<Lodging>
            {
                new()
                {
                    TripId = 1,
                    LodgingId = 1,
                    Location = "Some Hotel",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now
                },
                new()
                {
                    TripId = 1,
                    LodgingId = 2,
                    Location = "1602 Maple St",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now
                }
            };

            var mockLodgingDal = new Mock<LodgingDal>();
            mockLodgingDal.Setup(db => db.CreateLodging(1, "Some Hotel", DateTime.Now, DateTime.Now, null))
                .Returns(1);
            mockLodgingDal.Setup(db => db.CreateLodging(1, "1602 Maple St", DateTime.Now, DateTime.Now, null))
                .Returns(1);
            mockLodgingDal.Setup(db => db.GetLodgingsByTripId(1)).Returns(fakeLodgings);

            LodgingManager lodgingManager = new(mockLodgingDal.Object);

            var resultResponse =
                lodgingManager.GetLodgingsByTripId(1);

            Assert.AreEqual(2, resultResponse.Data?.Count);
        }


        [TestMethod]
        public void Call_ServerMySqlException_Failure()
        {
            var mockDal = new Mock<LodgingDal>();
            var builder = new MySqlExceptionBuilder();
            mockDal.Setup(dal => dal.GetLodgingsByTripId(1))
                .Throws(builder.WithError((uint) Ui.StatusCode.InternalServerError, "test").Build());
            var lodgingManager = new LodgingManager(mockDal.Object);
            var result = lodgingManager.GetLodgingsByTripId(1);
            Assert.AreEqual((uint) Ui.StatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual("test", result.ErrorMessage);
        }

        [TestMethod]
        public void Call_ServerException_Failure()
        {
            var mockDal = new Mock<LodgingDal>();
            mockDal.Setup(dal => dal.GetLodgingsByTripId(1))
                .Throws(new Exception());
            var lodgingManager = new LodgingManager(mockDal.Object);
            var result = lodgingManager.GetLodgingsByTripId(1);
            Assert.AreEqual((uint) Ui.StatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual(Ui.ErrorMessages.InternalServerError, result.ErrorMessage);
        }
    }
}