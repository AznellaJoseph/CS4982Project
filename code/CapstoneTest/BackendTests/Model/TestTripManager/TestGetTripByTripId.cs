using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;


namespace CapstoneTest.BackendTests.Model.TestTripManager
{
    [TestClass]
    public class TestGetTripByTripId
    {
        [TestMethod]
        public void GetTrip_Success()
        {
            var mockDal = new Mock<TripDal>();

            var startDate = DateTime.Now;
            var endDate = DateTime.Now;

            mockDal.Setup(dal => dal.GetTripByTripId(1)).Returns(new Trip
            {
                TripId = 1,
                UserId = 1,
                Name = "test",
                StartDate = startDate,
                EndDate = endDate,
                Notes = "notes"
            });

            var tripManager = new TripManager(mockDal.Object);
            var result = tripManager.GetTripByTripId(1);
            var trip = result.Data;

            Assert.AreEqual(200U, result.StatusCode);
            Assert.AreEqual(1, trip?.TripId);
            Assert.AreEqual(1, trip?.UserId);
            Assert.AreEqual("test", trip?.Name);
            Assert.AreEqual(startDate, trip?.StartDate);
            Assert.AreEqual(endDate, trip?.EndDate);
            Assert.AreEqual("notes", trip?.Notes);
        }

        [TestMethod]
        public void GetTrip_NoTripFound_Failure()
        {
            var mockDal = new Mock<TripDal>();
            Trip? missingTrip = null;
            mockDal.Setup(dal => dal.GetTripByTripId(1)).Returns(missingTrip);

            var tripManager = new TripManager(mockDal.Object);
            var result = tripManager.GetTripByTripId(1);

            Assert.AreEqual(404U, result.StatusCode);
            Assert.IsNotNull(result.ErrorMessage);
            Assert.AreNotEqual(String.Empty, result.ErrorMessage);
        }

        [TestMethod]
        public void GetTrip_MySqlException_Failure()
        {
            var mockDal = new Mock<TripDal>();
            var builder = new MySqlExceptionBuilder();
            mockDal.Setup(dal => dal.GetTripByTripId(1)).Throws(builder.WithError(500, "test").Build());

            var tripManager = new TripManager(mockDal.Object);
            var result = tripManager.GetTripByTripId(1);

            Assert.AreEqual(500U, result.StatusCode);
            Assert.AreEqual("test", result.ErrorMessage);
        }

        [TestMethod]
        public void GetTrip_Exception_Failure()
        {
            var mockDal = new Mock<TripDal>();
            mockDal.Setup(dal => dal.GetTripByTripId(1)).Throws(new Exception("test"));

            var tripManager = new TripManager(mockDal.Object);
            var result = tripManager.GetTripByTripId(1);

            Assert.AreEqual(500U, result.StatusCode);
        }
    }
}
