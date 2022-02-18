﻿using CapstoneBackend.Model;
using CapstoneDesktop.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
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
            var mockWaypointManager = new Mock<WaypointManager>();
            var mockScreen = new Mock<IScreen>();
            TripOverviewPageViewModel testViewModel = new(mockTrip.Object, mockWaypointManager.Object, mockScreen.Object);

            Assert.IsNotNull(testViewModel.LogoutCommand);
            Assert.IsNotNull(testViewModel.BackCommand);
        }

        [TestMethod]
        public void Constructor_OneParameter_PropertyCreations()
        {
            Trip testTrip = new();
            testTrip.Name = "Test Trip";
            testTrip.Notes = "Some Notes";
            testTrip.StartDate = new DateTime(2022, 2, 2);
            testTrip.EndDate = new DateTime(3033, 3, 3);
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
