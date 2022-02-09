using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CapstoneBackend.Model;
using Castle.Core.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CapstoneTest.BackendTests.Model.TestUser
{
    [TestClass]
    public class TestTripsProperty
    {
        
        [TestMethod]
        public void GetTrips_Success()
        {
            var mockTripManager = new Mock<TripManager>();
            var startDate = DateTime.Now;
            var endDate = DateTime.Now;
            mockTripManager.Setup(tm => tm.GetTripsByUser(1)).Returns(new Response<IList<Trip>>
            {
                Data = new List<Trip>
                {
                    new()
                    {
                        Id = 1,
                        UserId = 1,
                        Name = string.Empty,
                        StartDate = startDate,
                        EndDate = endDate,
                    }
                }
            });
            var user = new User
            {
                Id = 1,
                TripManager = mockTripManager.Object
            };
            var result = user.Trips;
            var item = result.ElementAt(0);
            Assert.AreEqual(1,item.Id);
            Assert.AreEqual(1, item.UserId);
            Assert.AreEqual(string.Empty, item.Name);
            Assert.AreEqual(startDate, item.StartDate);
            Assert.AreEqual(endDate, item.EndDate);
        }
        
        [TestMethod]
        public void GetTrips_Failure()
        {
            var mockTripManager = new Mock<TripManager>();
            mockTripManager.Setup(tm => tm.GetTripsByUser(1)).Returns(new Response<IList<Trip>>());
            var user = new User
            {
                Id = 1,
                TripManager = mockTripManager.Object
            };
            var result = user.Trips;
            Assert.IsTrue(result.IsNullOrEmpty());
        }
    }
}