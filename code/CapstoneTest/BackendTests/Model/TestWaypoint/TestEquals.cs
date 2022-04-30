using System;
using CapstoneBackend.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CapstoneTest.BackendTests.Model.TestWaypoint
{
    [TestClass]
    public class TestEquals
    {
        [TestMethod]
        public void Equals_EventIsNotEqual_ReturnsFalse()
        {
            Waypoint waypoint = new()
            {
                TripId = 1,
                WaypointId = 1,
                EndDate = DateTime.Today.AddDays(4),
                StartDate = DateTime.Today,
                Location = "UWG Library, Carrollton GA",
                Notes = string.Empty
            };

            Assert.IsFalse(waypoint.Equals(null));
        }

        [TestMethod]
        public void Equals_EventIsEqual_ReturnsTrue()
        {
            Waypoint waypoint = new()
            {
                TripId = 1,
                WaypointId = 1,
                EndDate = DateTime.Today.AddDays(4),
                StartDate = DateTime.Today,
                Location = "UWG Library, Carrollton GA",
                Notes = string.Empty
            };

            Assert.IsTrue(waypoint.Equals(new Waypoint
            {
                TripId = 1, WaypointId = 1, EndDate = DateTime.Today.AddDays(4), StartDate = DateTime.Today,
                Location = "UWG Library, Carrollton GA"
            }));
        }
    }
}