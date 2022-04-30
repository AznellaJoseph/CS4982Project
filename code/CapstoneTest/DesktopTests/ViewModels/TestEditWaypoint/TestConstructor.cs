using System;
using CapstoneBackend.Model;
using CapstoneDesktop.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReactiveUI;

namespace CapstoneTest.DesktopTests.ViewModels.TestEditWaypoint
{
    [TestClass]
    public class TestConstructor
    {
        [TestMethod]
        public void Constructor_PropertyCreations()
        {
            Waypoint waypoint = new()
            {
                EndDate = DateTime.Today.AddDays(3),
                StartDate = DateTime.Today,
                Location = "Hilton",
                WaypointId = 1,
                TripId = 1,
                Notes = "notes"
            };
            var mockScreen = new Mock<IScreen>();
            EditWaypointPageViewModel editWaypointPageViewModel = new(waypoint, mockScreen.Object);

            Assert.AreEqual(editWaypointPageViewModel.HostScreen, mockScreen.Object);
            Assert.AreEqual(editWaypointPageViewModel.StartDate, waypoint.StartDate.Date);
            Assert.AreEqual(editWaypointPageViewModel.StartTime, waypoint.StartDate.TimeOfDay);
            Assert.AreEqual(editWaypointPageViewModel.EndDate, waypoint.EndDate.Date);
            Assert.AreEqual(editWaypointPageViewModel.EndTime, waypoint.EndDate.TimeOfDay);
            Assert.AreEqual(editWaypointPageViewModel.Location, waypoint.Location);
            Assert.AreEqual(editWaypointPageViewModel.Notes, waypoint.Notes);
            Assert.IsNotNull(editWaypointPageViewModel.EditWaypointCommand);
            Assert.IsNotNull(editWaypointPageViewModel.CancelEditWaypointCommand);
            Assert.IsNotNull(editWaypointPageViewModel.WaypointManager);
            Assert.IsNotNull(editWaypointPageViewModel.ValidationManager);
            Assert.IsNotNull(editWaypointPageViewModel.UrlPathSegment);
        }
    }
}