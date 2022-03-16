using CapstoneBackend.Model;
using CapstoneDesktop.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReactiveUI;

namespace CapstoneTest.DesktopTests.ViewModels.TestWaypoint
{
    [TestClass]
    public class TestConstructor
    {
        [TestMethod]
        public void TestConstructor_PropertyCreations()
        {
            var mockWaypoint = new Mock<Waypoint>();
            var mockScreen = new Mock<IScreen>();

            var waypointViewModel = new WaypointViewModel(mockWaypoint.Object, mockScreen.Object);
            Assert.AreEqual(mockWaypoint.Object, waypointViewModel.Waypoint);
            Assert.AreEqual(mockWaypoint.Object, waypointViewModel.Event);
            Assert.IsNotNull(waypointViewModel.WaypointManager);
            Assert.IsNotNull(waypointViewModel.RemoveCommand);
            Assert.IsNotNull(waypointViewModel.HostScreen);
        }
    }
}