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
        public void Constructor_ThreeParameter_PropertyCreations()
        {
            var mockTrip = new Mock<Trip>();
            var mockWaypointManager = new Mock<WaypointManager>();
            var mockScreen = new Mock<IScreen>();
            CreateWaypointPageViewModel createWaypointWindowViewModel =
                new(mockTrip.Object, mockWaypointManager.Object, mockScreen.Object);

            Assert.AreEqual(string.Empty, createWaypointWindowViewModel.ErrorMessage);
            Assert.AreEqual(mockScreen.Object, createWaypointWindowViewModel.HostScreen);
            Assert.IsNotNull(createWaypointWindowViewModel.UrlPathSegment);
            Assert.IsNotNull(createWaypointWindowViewModel.CreateWaypointCommand);
            Assert.IsNotNull(createWaypointWindowViewModel.CancelCreateWaypointCommand);
            Assert.IsNull(createWaypointWindowViewModel.StartTime);
            Assert.IsNull(createWaypointWindowViewModel.EndTime);
            Assert.IsNull(createWaypointWindowViewModel.StartDate);
            Assert.IsNull(createWaypointWindowViewModel.EndDate);
            Assert.IsNull(createWaypointWindowViewModel.Location);
            Assert.IsNull(createWaypointWindowViewModel.Notes);
        }

        [TestMethod]
        public void Constructor_TwoParameters_PropertyCreations()
        {
            var mockTrip = new Mock<Trip>();
            var mockScreen = new Mock<IScreen>();
            CreateWaypointPageViewModel createWaypointWindowViewModel =
                new(mockTrip.Object, mockScreen.Object);

            Assert.AreEqual(string.Empty, createWaypointWindowViewModel.ErrorMessage);
            Assert.AreEqual(mockScreen.Object, createWaypointWindowViewModel.HostScreen);
            Assert.IsNotNull(createWaypointWindowViewModel.UrlPathSegment);
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