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
        public void TestConstructor_Success()
        {
            var mockWaypoint = new Mock<Waypoint>();
            var mockScreen = new Mock<IScreen>();
            var result = new WaypointViewModel(mockWaypoint.Object, mockScreen.Object);
            Assert.AreEqual(mockWaypoint.Object, result.Waypoint);
            Assert.AreEqual(mockWaypoint.Object, result.Event);
        }
    }
}