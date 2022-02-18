using System;
using CapstoneBackend.Model;
using CapstoneDesktop.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReactiveUI;

namespace CapstoneTest.DesktopTests.ViewModels.TestCreateWaypoint
{
    [TestClass]
    public class TestConstructor
    {
        [TestMethod]
        public void Constructor_OneParameter_PropertyCreations()
        {
            var mockTrip = new Mock<Trip>();
            var mockWaypointManager = new Mock<WaypointManager>();
            var mockScreen = new Mock<IScreen>();
            CreateWaypointPageViewModel createWaypointWindowViewModel = new(mockTrip.Object, mockWaypointManager.Object, mockScreen.Object);

            Assert.AreEqual(string.Empty, createWaypointWindowViewModel.ErrorMessage);
            Assert.AreEqual(DateTime.MinValue, createWaypointWindowViewModel.StartDate);
            Assert.AreEqual(DateTime.MinValue, createWaypointWindowViewModel.EndDate);
            Assert.IsNotNull(createWaypointWindowViewModel.CreateWaypointCommand);
            Assert.IsNotNull(createWaypointWindowViewModel.CancelCreateWaypointCommand);
            Assert.IsNotNull(createWaypointWindowViewModel.StartTime);
            Assert.IsNotNull(createWaypointWindowViewModel.EndTime);
            Assert.IsNull(createWaypointWindowViewModel.Location);
            Assert.IsNull(createWaypointWindowViewModel.Notes);
        }

        [TestMethod]
        public void Constructor_NoParameters_PropertyCreations()
        {
            var mockTrip = new Mock<Trip>();
            var mockWaypointManager = new Mock<WaypointManager>();
            var mockScreen = new Mock<IScreen>();
            CreateWaypointPageViewModel createWaypointWindowViewModel = new(mockTrip.Object, mockWaypointManager.Object, mockScreen.Object);

            Assert.AreEqual(string.Empty, createWaypointWindowViewModel.ErrorMessage);
            Assert.AreEqual(DateTime.MinValue, createWaypointWindowViewModel.StartDate);
            Assert.AreEqual(DateTime.MinValue, createWaypointWindowViewModel.EndDate);
            Assert.IsNotNull(createWaypointWindowViewModel.CreateWaypointCommand);
            Assert.IsNotNull(createWaypointWindowViewModel.CancelCreateWaypointCommand);
            Assert.IsNotNull(createWaypointWindowViewModel.StartTime);
            Assert.IsNotNull(createWaypointWindowViewModel.EndTime);
            Assert.IsNull(createWaypointWindowViewModel.Location);
            Assert.IsNull(createWaypointWindowViewModel.Notes);
        }
    }
}