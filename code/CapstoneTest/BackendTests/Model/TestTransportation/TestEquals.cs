using System;
using CapstoneBackend.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CapstoneTest.BackendTests.Model.TestTransportation
{
    [TestClass]
    public class TestEquals
    {
        [TestMethod]
        public void Equals_EventIsNotEqual_ReturnsFalse()
        {
            Transportation transportation = new()
            {
                TripId = 1,
                TransportationId = 1,
                EndDate = DateTime.Today.AddDays(4),
                StartDate = DateTime.Today,
                Method = "Car",
                Notes = string.Empty
            };

            Assert.IsFalse(transportation.Equals(null));
        }

        [TestMethod]
        public void Equals_EventIsEqual_ReturnsTrue()
        {
            Transportation transportation = new()
            {
                TripId = 1,
                TransportationId = 1,
                EndDate = DateTime.Today.AddDays(4),
                StartDate = DateTime.Today,
                Method = "Car",
                Notes = string.Empty
            };

            Assert.IsTrue(transportation.Equals(new Transportation
            {
                TripId = 1, TransportationId = 1, EndDate = DateTime.Today.AddDays(4), StartDate = DateTime.Today,
                Method = "Car"
            }));
        }
    }
}