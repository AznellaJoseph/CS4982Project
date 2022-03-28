using System;
using CapstoneBackend.Model;
using CapstoneDesktop.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReactiveUI;

namespace CapstoneTest.DesktopTests.ViewModels.TestTripOverviewPage
{
    [TestClass]
    public class TestConstructor
    {
        [TestMethod]
        public void Constructor_PropertyCreations()
        {
            Trip testTrip = new()
            {
                Name = "Test Trip",
                Notes = "Some Notes",
                StartDate = new DateTime(2022, 2, 2),
                EndDate = new DateTime(3033, 3, 3)
            };
            var mockScreen = new Mock<IScreen>();
            TripOverviewPageViewModel tripOverviewPageViewModel =
                new(testTrip, mockScreen.Object, new LodgingManager());
            Assert.AreEqual(mockScreen.Object, tripOverviewPageViewModel.HostScreen);
            Assert.AreEqual(testTrip, tripOverviewPageViewModel.Trip);
            Assert.IsNotNull(tripOverviewPageViewModel.LogoutCommand);
            Assert.IsNotNull(tripOverviewPageViewModel.CreateWaypointCommand);
            Assert.IsNotNull(tripOverviewPageViewModel.CreateTransportationCommand);
            Assert.IsNotNull(tripOverviewPageViewModel.CreateLodgingCommand);
            Assert.IsNotNull(tripOverviewPageViewModel.BackCommand);
            Assert.IsNotNull(tripOverviewPageViewModel.EventManager);
            Assert.IsNotNull(tripOverviewPageViewModel.LodgingManager);
            Assert.IsNotNull(tripOverviewPageViewModel.UrlPathSegment);
            Assert.IsNull(tripOverviewPageViewModel.SelectedDate);
        }
    }
}