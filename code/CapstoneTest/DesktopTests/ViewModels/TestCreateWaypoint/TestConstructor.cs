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
            Assert.IsNotNull(createWaypointWindowViewModel.CreateWaypointCommand);
            Assert.IsNotNull(createWaypointWindowViewModel.CancelCreateWaypointCommand);
            Assert.IsNull(createWaypointWindowViewModel.StartTime);
            Assert.IsNull(createWaypointWindowViewModel.EndTime);
            Assert.IsNull(createWaypointWindowViewModel.StartTime);
            Assert.IsNull(createWaypointWindowViewModel.EndTime);
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
            Assert.IsNotNull(createWaypointWindowViewModel.CreateWaypointCommand);
            Assert.IsNotNull(createWaypointWindowViewModel.CancelCreateWaypointCommand);
            Assert.IsNull(createWaypointWindowViewModel.StartDate);
            Assert.IsNull(createWaypointWindowViewModel.EndDate);
            Assert.IsNull(createWaypointWindowViewModel.StartTime);
            Assert.IsNull(createWaypointWindowViewModel.EndTime);
            Assert.IsNull(createWaypointWindowViewModel.Location);
            Assert.IsNull(createWaypointWindowViewModel.Notes);
        }
    }
}