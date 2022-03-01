using System;
using System.Collections.Generic;
using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CapstoneTest.BackendTests.Model.TestTripManager
{
    [TestClass]
    public class TestGetTripsByUserId
    {
        [TestMethod]
        public void GetTrips_Success()
        {
            var mockDal = new Mock<TripDal>();
            var startDate = DateTime.Now;
            var endDate = DateTime.Now;
            mockDal.Setup(dal => dal.GetTripsByUserId(1)).Returns(new List<Trip>
            {
                new()
                {
                    TripId = 1,
                    UserId = 1,
                    Name = "test",
                    StartDate = startDate,
                    EndDate = endDate,
                    Notes = "notes"
                }
            });
            var tripManager = new TripManager(mockDal.Object);
            var result = tripManager.GetTripsByUser(1);
            Assert.AreEqual((uint)Ui.StatusCode.Success, result.StatusCode);
            Assert.IsFalse(result.Data?.Count == 0);
            var trip = result.Data?[0];
            Assert.AreEqual(1, trip?.TripId);
            Assert.AreEqual(1, trip?.UserId);
            Assert.AreEqual("test", trip?.Name);
            Assert.AreEqual("notes", trip?.Notes);
            Assert.AreEqual(startDate, trip?.StartDate);
            Assert.AreEqual(endDate, trip?.EndDate);
        }

        [TestMethod]
        public void GetTrips_MySqlException_Failure()
        {
            var mockDal = new Mock<TripDal>();
            var builder = new MySqlExceptionBuilder();
            mockDal.Setup(dal => dal.GetTripsByUserId(1)).Throws(builder.WithError((uint)Ui.StatusCode.InternalServerError, "test").Build());
            var tripManager = new TripManager(mockDal.Object);
            var result = tripManager.GetTripsByUser(1);
            Assert.AreEqual((uint)Ui.StatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual("test", result.ErrorMessage);
        }

        [TestMethod]
        public void GetTrips_Exception_Failure()
        {
            var mockDal = new Mock<TripDal>();
            mockDal.Setup(dal => dal.GetTripsByUserId(1)).Throws(new Exception("test"));
            var tripManager = new TripManager(mockDal.Object);
            var result = tripManager.GetTripsByUser(1);
            Assert.AreEqual((uint)Ui.StatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual("test", result.ErrorMessage);
        }
    }
}