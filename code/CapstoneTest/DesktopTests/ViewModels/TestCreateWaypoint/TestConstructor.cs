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
        public void Constructor_PropertyCreations()
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
            Assert.IsNotNull(createWaypointWindowViewModel.ValidationManager);
            Assert.IsNotNull(createWaypointWindowViewModel.WaypointManager);
            Assert.IsNull(createWaypointWindowViewModel.StartDate);
            Assert.IsNull(createWaypointWindowViewModel.EndDate);
            Assert.IsNull(createWaypointWindowViewModel.StartTime);
            Assert.IsNull(createWaypointWindowViewModel.EndTime);
            Assert.IsNull(createWaypointWindowViewModel.Location);
            Assert.IsNull(createWaypointWindowViewModel.Notes);
        }
    }
}