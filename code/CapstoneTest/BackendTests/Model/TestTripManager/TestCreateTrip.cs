using System;
using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CapstoneTest.BackendTests.Model.TestTripManager
{
    [TestClass]
    public class TestCreateTrip
    {
        [TestMethod]
        public void CreateTrip_StartDateAfterEndDate_Failure()
        {
            var mockDal = new Mock<TripDal>();
            var tripManager = new TripManager(mockDal.Object);
            var result = tripManager.CreateTrip(1, string.Empty, null, DateTime.Now.AddDays(1), DateTime.Now);
            Assert.AreEqual(400U, result.StatusCode);
            Assert.AreEqual(Ui.ErrorMessages.InvalidStartDate, result.ErrorMessage);
        }

        [TestMethod]
        public void CreateTrip_Success()
        {
            var mockDal = new Mock<TripDal>();
            var startDate = DateTime.Now;
            var endDate = DateTime.Now;
            mockDal.Setup(dal => dal.CreateTrip(1, string.Empty, null, startDate, endDate)).Returns(1);
            var tripManager = new TripManager(mockDal.Object);
            var result = tripManager.CreateTrip(1, string.Empty, null, startDate, endDate);
            Assert.AreEqual(200U, result.StatusCode);
            Assert.AreEqual(1, result.Data);
        }

        [TestMethod]
        public void CreateTrip_ServerMySqlException_Failure()
        {
            var mockDal = new Mock<TripDal>();
            var builder = new MySqlExceptionBuilder();
            var startDate = DateTime.Now;
            var endDate = DateTime.Now;
            mockDal.Setup(dal => dal.CreateTrip(1, string.Empty, null, startDate, endDate))
                .Throws(builder.WithError(500, "test").Build());
            var tripManager = new TripManager(mockDal.Object);
            var result = tripManager.CreateTrip(1, string.Empty, null, startDate, endDate);
            Assert.AreEqual(500U, result.StatusCode);
            Assert.AreEqual("test", result.ErrorMessage);
        }

        [TestMethod]
        public void CreateTrip_ServerException_Failure()
        {
            var mockDal = new Mock<TripDal>();
            var startDate = DateTime.Now;
            var endDate = DateTime.Now;
            mockDal.Setup(dal => dal.CreateTrip(1, string.Empty, null, startDate, endDate))
                .Throws(new Exception("test"));
            var tripManager = new TripManager(mockDal.Object);
            var result = tripManager.CreateTrip(1, string.Empty, null, startDate, endDate);
            Assert.AreEqual(500U, result.StatusCode);
            Assert.AreEqual("test", result.ErrorMessage);
        }
    }
}