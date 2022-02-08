using System.Collections.Generic;
using System.Data.Common;
using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using Castle.Core.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MySql.Data.MySqlClient;

namespace CapstoneTest.BackendTests.Model.TestTripManager
{
    
    [TestClass]
    public class TestGetTripsByUserId
    {

        public void GetTrips_Success()
        {
            var mockDal = new Mock<TripDal>();
            mockDal.Setup(dal => dal.GetTripsByUserId(1)).Returns(new List<Trip>());
            var tripManager = new TripManager(mockDal.Object);
            var result = tripManager.GetTripsByUser(1);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsTrue(result.Data?.IsNullOrEmpty());
        }
        
        [TestMethod]
        public void GetTrips_Failure()
        {
            var mockDal = new Mock<TripDal>();
            var builder = new MySqlExceptionBuilder();
            mockDal.Setup(dal => dal.GetTripsByUserId(1)).Throws(builder.WithError(500, "test").Build());
            var tripManager = new TripManager(mockDal.Object);
            var result = tripManager.GetTripsByUser(1);
            Assert.AreEqual((uint) 500, result.StatusCode);
            Assert.AreEqual("test", result.ErrorMessage);
        }
        
    }
}