using System;
using System.Collections.Generic;
using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using Castle.Core.Internal;
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
            var result = tripManager.CreateTrip(1, string.Empty, DateTime.Now.AddDays(1), DateTime.Now);
            Assert.AreEqual(400U, result.StatusCode);
            Assert.AreEqual("Start date of a trip cannot be after the end date.", result.ErrorMessage);
        }
        
        [TestMethod]
        public void CreateTrip_TripIdIsNull_Failure()
        {
            var mockDal = new Mock<TripDal>();
            var startDate = DateTime.Now;
            var endDate = DateTime.Now;
            mockDal.Setup(dal => dal.CreateTrip(1, string.Empty, startDate, endDate)).Returns((int?) null);
            var tripManager = new TripManager(mockDal.Object);
            var result = tripManager.CreateTrip(1, string.Empty, startDate, endDate);
            Assert.AreEqual(500U, result.StatusCode);
            Assert.AreEqual("Server Error", result.ErrorMessage);
        }
        
        [TestMethod]
        public void CreateTrip_Success()
        {
            var mockDal = new Mock<TripDal>();
            var startDate = DateTime.Now;
            var endDate = DateTime.Now;
            mockDal.Setup(dal => dal.CreateTrip(1, string.Empty, startDate, endDate)).Returns(1);
            var tripManager = new TripManager(mockDal.Object);
            var result = tripManager.CreateTrip(1, string.Empty, startDate, endDate);
            Assert.AreEqual(200U, result.StatusCode);
            Assert.AreEqual(1, result.Data);
        }
        
        [TestMethod]
        public void CreateTrip_ServerError_Failure()
        {
            var mockDal = new Mock<TripDal>();
            var builder = new MySqlExceptionBuilder();
            var startDate = DateTime.Now;
            var endDate = DateTime.Now;
            mockDal.Setup(dal => dal.CreateTrip(1, string.Empty, startDate, endDate)).Throws(builder.WithError(500, "test").Build());
            var tripManager = new TripManager(mockDal.Object);
            var result = tripManager.CreateTrip(1, string.Empty, startDate, endDate);
            Assert.AreEqual((uint) 500, result.StatusCode);
            Assert.AreEqual("test", result.ErrorMessage);
        }
    }
}