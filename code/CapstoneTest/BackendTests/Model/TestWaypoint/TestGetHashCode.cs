using System;
using CapstoneBackend.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CapstoneTest.BackendTests.Model.TestWaypoint
{
    [TestClass]
    public class TestGetHashCode
    {
        [TestMethod]
        public void GetHashCode_ReturnsCombinedHashCode()
        {
            Waypoint waypoint = new()
            {
                TripId = 1,
                WaypointId = 1,
                EndDate = DateTime.Today.AddDays(4),
                StartDate = DateTime.Today,
                Location = "UWG Library, Carrollton",
                Notes = string.Empty
            };

            Assert.AreEqual(HashCode.Combine(waypoint.Id, waypoint.DisplayName, waypoint.EventType),
                waypoint.GetHashCode());
        }
    }
}