using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapstoneBackend.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CapstoneTest.BackendTests.Model.TestTripManager
{
    [TestClass]
    public class TestFindClashingTrip
    {

        [TestMethod]
        public void FindClashingTrip_ReturnsNull()
        {
            var mockTripManager = new Mock<TripManager>();
            mockTripManager.Setup(em => em.FindClashingTrip(1, DateTime.Today, DateTime.Today))
                .Returns(new Response<Trip> { Data = null });


            Assert.IsNull(mockTripManager.Object.FindClashingTrip(1, DateTime.Today, DateTime.Today).Data);
        }

        [TestMethod]
        public void FindClashingTrip_ReturnsNotNull()
        {
            var mockTripManager = new Mock<TripManager>();
            mockTripManager.Setup(em => em.FindClashingTrip(1, DateTime.Today, DateTime.Today))
                .Returns(new Response<Trip> { Data = new Trip() });


            Assert.IsNotNull(mockTripManager.Object.FindClashingTrip(1, DateTime.Today, DateTime.Today).Data);
        }
    }
}
