using System;
using CapstoneBackend.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CapstoneTest.BackendTests.Model.TestTransportation
{
    [TestClass]
    public class TestGetHashCode
    {
        [TestMethod]
        public void GetHashCode_ReturnsCombinedHashCode()
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

            Assert.AreEqual(HashCode.Combine(transportation.Id, transportation.DisplayName, transportation.EventType),
                transportation.GetHashCode());
        }
    }
}