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
    public class TestRemoveCommand
    {
        [TestMethod]
        public void RemoveCommand_Success()
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

            var eventRemoved = false;
            var viewModel = new WaypointViewModel(waypoint, mockScreen.Object)
            {
                WaypointManager = mockWaypointManager.Object
            };
            viewModel.RemoveEvent += (_, _) => eventRemoved = true;

            var testScheduler = new TestScheduler();

            viewModel.RemoveCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.IsTrue(eventRemoved);
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
            var eventRemoved = false;
            var viewModel = new WaypointViewModel(waypoint, mockScreen.Object)
            {
                WaypointManager = mockWaypointManager.Object
            };
            viewModel.RemoveEvent += (_, _) => eventRemoved = true;

            var testScheduler = new TestScheduler();

            viewModel.RemoveCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.IsFalse(eventRemoved);
        }
    }
}