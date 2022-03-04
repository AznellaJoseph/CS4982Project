using System;
using CapstoneBackend.Model;
using CapstoneDesktop.ViewModels;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReactiveUI;

namespace CapstoneTest.DesktopTests.ViewModels.TestWaypoint
{
    [TestClass]
    public class TestRemove
    {
        [TestMethod]
        public void Remove_Success()
        {
            var mockWaypointManager = new Mock<WaypointManager>();
            var waypoint = new Waypoint
            {
                WaypointId = 1
            };
            var mockScreen = new Mock<IScreen>();
            mockWaypointManager.Setup(wm => wm.RemoveWaypoint(1))
                .Returns(new Response<bool>
                {
                    Data = true
                });
            var didRemovedEvent = false;
            var viewModel = new WaypointViewModel(waypoint, mockScreen.Object)
            {
                FakeWaypointManager = mockWaypointManager.Object
            };
            viewModel.RemoveEvent += (sender, e) => didRemovedEvent = true;
            var testScheduler = new TestScheduler();
            viewModel.RemoveCommand.Execute().Subscribe();
            testScheduler.Start();
            Assert.IsTrue(didRemovedEvent);
        }

        [TestMethod]
        public void Remove_Failure()
        {
            var mockWaypointManager = new Mock<WaypointManager>();
            var waypoint = new Waypoint
            {
                WaypointId = 1
            };
            var mockScreen = new Mock<IScreen>();
            mockWaypointManager.Setup(wm => wm.RemoveWaypoint(1))
                .Returns(new Response<bool>
                {
                    Data = false
                });
            var didRemovedEvent = false;
            var viewModel = new WaypointViewModel(waypoint, mockScreen.Object)
            {
                FakeWaypointManager = mockWaypointManager.Object
            };
            viewModel.RemoveEvent += (sender, e) => didRemovedEvent = true;
            var testScheduler = new TestScheduler();
            viewModel.RemoveCommand.Execute().Subscribe();
            testScheduler.Start();
            Assert.IsFalse(didRemovedEvent);
        }
    }
}