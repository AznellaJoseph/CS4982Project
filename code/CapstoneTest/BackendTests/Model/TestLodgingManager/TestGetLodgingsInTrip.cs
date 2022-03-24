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
        public void GetLodgingsInTrip_EmptySet_ReturnsEmptyList()
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
        public void GetLodgingsInTrip_YieldsSetWithOneValue_ReturnsExpectedList()
        {
            IList<Lodging> fakeLodgings = new List<Lodging>
            {
                new()
                {
                    TripId = 1,
                    LodgingId = 1,
                    Location = "Some Hotel",
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddDays(1),
                    Notes = "notes"
                }
            };

            var mockLodgingDal = new Mock<LodgingDal>();
            mockLodgingDal.Setup(db => db.CreateLodging(1, "Some Hotel", DateTime.Today, DateTime.Today.AddDays(1), "notes"))
                .Returns(1);
            mockLodgingDal.Setup(db => db.GetLodgingsByTripId(1)).Returns(fakeLodgings);

            LodgingManager lodgingManager = new(mockLodgingDal.Object);

            var resultResponse =
                lodgingManager.GetLodgingsByTripId(1);

            Assert.AreEqual(1, resultResponse.Data?.Count);
            Assert.AreEqual(1, resultResponse.Data?[0].TripId);
            Assert.AreEqual(1, resultResponse.Data?[0].LodgingId);
            Assert.AreEqual("Some Hotel", resultResponse.Data?[0].Location);
            Assert.AreEqual(DateTime.Today, resultResponse.Data?[0].StartDate);
            Assert.AreEqual(DateTime.Today.AddDays(1), resultResponse.Data?[0].EndDate);
            Assert.AreEqual("notes", resultResponse.Data?[0].Notes);
        }

        [TestMethod]
        public void GetLodgingsInTrip_YieldsSetWithMultipleValues_ReturnsExpectedList()
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
        public void GetLodgingsInTrip_ServerMySqlException_ReturnsErrorMessage()
        {
            var mockDal = new Mock<LodgingDal>();
            var builder = new MySqlExceptionBuilder();
            mockDal.Setup(dal => dal.GetLodgingsByTripId(1))
                .Throws(builder.WithError((uint)Ui.StatusCode.InternalServerError, "test").Build());

            LodgingManager lodgingManager = new(mockDal.Object);

            var result = lodgingManager.GetLodgingsByTripId(1);

            Assert.AreEqual((uint)Ui.StatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual("test", result.ErrorMessage);
        }

        [TestMethod]
        public void GetLodgingsInTrip_ServerException_ReturnsErrorMessage()
        {
            var mockDal = new Mock<LodgingDal>();
            mockDal.Setup(dal => dal.GetLodgingsByTripId(1))
                .Throws(new Exception());

            LodgingManager lodgingManager = new(mockDal.Object);

            var result = lodgingManager.GetLodgingsByTripId(1);

            Assert.AreEqual((uint)Ui.StatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual(Ui.ErrorMessages.InternalServerError, result.ErrorMessage);
        }
    }
}