using System;
using CapstoneBackend.Model;
using CapstoneDesktop.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReactiveUI;

namespace CapstoneTest.DesktopTests.ViewModels.TestTripOverviewWindow
{
    [TestClass]
    public class TestConstructor
    {
        [TestMethod]
        public void Constructor_NoParameters_PropertyCreations()
        {
            var mockTrip = new Mock<Trip>();
            var mockEventManager = new Mock<EventManager>();
            var mockScreen = new Mock<IScreen>();
            TripOverviewPageViewModel testViewModel =
                new(mockTrip.Object, mockEventManager.Object, mockScreen.Object);

            Assert.IsNotNull(testViewModel.LogoutCommand);
            Assert.IsNotNull(testViewModel.BackCommand);
        }

        [TestMethod]
        public void Constructor_OneParameter_PropertyCreations()
        {
            Trip testTrip = new()
            {
                Name = "Test Trip",
                Notes = "Some Notes",
                StartDate = new DateTime(2022, 2, 2),
                EndDate = new DateTime(3033, 3, 3)
            };
            var mockScreen = new Mock<IScreen>();
            TripOverviewPageViewModel testViewModel = new(testTrip, mockScreen.Object);

            Assert.IsNotNull(testViewModel.LogoutCommand);
            Assert.IsNotNull(testViewModel.CreateWaypointCommand);
            Assert.IsNotNull(testViewModel.BackCommand);
            Assert.AreEqual(mockScreen.Object, testViewModel.HostScreen);
            Assert.AreEqual(testTrip, testViewModel.Trip);
            Assert.IsNotNull(testViewModel.UrlPathSegment);
            Assert.IsNull(testViewModel.SelectedDate);
        }
    }
}