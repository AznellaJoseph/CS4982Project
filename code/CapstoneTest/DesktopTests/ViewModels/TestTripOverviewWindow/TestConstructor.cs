using CapstoneBackend.Model;
using CapstoneDesktop.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace CapstoneTest.DesktopTests.ViewModels.TestTripOverviewWindow
{
    [TestClass]
    public class TestConstructor
    {

        [TestMethod]
        public void Constructor_NoParameters_PropertyCreations()
        {
            TripOverviewViewModel testViewModel = new();

            Assert.IsNotNull(testViewModel.LogoutCommand);
            Assert.IsNotNull(testViewModel.ProfileCommand);
            Assert.IsNotNull(testViewModel.BackCommand);
            Assert.IsNotNull(testViewModel.TripName);
            Assert.IsNotNull(testViewModel.TripNotes);
            Assert.IsNotNull(testViewModel.TripStartDate);
            Assert.IsNotNull(testViewModel.TripEndDate);

            Assert.AreEqual(String.Empty, testViewModel.TripName);
            Assert.AreEqual(String.Empty, testViewModel.TripNotes);
            Assert.AreEqual(DateTime.MinValue, testViewModel.TripStartDate);
            Assert.AreEqual(DateTime.MaxValue, testViewModel.TripEndDate);
        }

        [TestMethod]
        public void Constructor_OneParameter_PropertyCreations()
        {
            Trip testTrip = new();
            testTrip.Name = "Test Trip";
            testTrip.Notes = "Some Notes";
            testTrip.StartDate = new DateTime(2022, 2, 2);
            testTrip.EndDate = new DateTime(3033, 3, 3);

            TripOverviewViewModel testViewModel = new(testTrip);

            Assert.IsNotNull(testViewModel.LogoutCommand);
            Assert.IsNotNull(testViewModel.ProfileCommand);
            Assert.IsNotNull(testViewModel.BackCommand);
            Assert.IsNotNull(testViewModel.TripName);
            Assert.IsNotNull(testViewModel.TripNotes);
            Assert.IsNotNull(testViewModel.TripStartDate);
            Assert.IsNotNull(testViewModel.TripEndDate);

            Assert.AreEqual("TEST TRIP", testViewModel.TripName);
            Assert.AreEqual("Some Notes", testViewModel.TripNotes);
            Assert.AreEqual(new DateTime(2022, 2, 2), testViewModel.TripStartDate);
            Assert.AreEqual(new DateTime(3033, 3, 3), testViewModel.TripEndDate);
        }
    }
}
