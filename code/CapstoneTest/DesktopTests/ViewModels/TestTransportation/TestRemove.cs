using System;
using CapstoneBackend.Model;
using CapstoneDesktop.ViewModels;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReactiveUI;

namespace CapstoneTest.DesktopTests.ViewModels.TestTransportation
{
    [TestClass]
    public class TestRemove
    {
        [TestMethod]
        public void Remove_Success()
        {
            var mockTransportationManager = new Mock<TransportationManager>();
            var transportation = new Transportation
            {
                TransportationId = 1
            };
            var mockScreen = new Mock<IScreen>();
            mockTransportationManager.Setup(wm => wm.RemoveTransportation(1))
                .Returns(new Response<bool>
                {
                    Data = true
                });
            var didRemovedEvent = false;
            var viewModel = new TransportationViewModel(transportation, mockScreen.Object)
            {
                TransportationManager = mockTransportationManager.Object
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
            var mockTransportationManager = new Mock<TransportationManager>();
            var transportation = new Transportation
            {
                TransportationId = 1
            };
            var mockScreen = new Mock<IScreen>();
            mockTransportationManager.Setup(wm => wm.RemoveTransportation(1))
                .Returns(new Response<bool>
                {
                    Data = false
                });
            var didRemovedEvent = false;
            var viewModel = new TransportationViewModel(transportation, mockScreen.Object)
            {
                TransportationManager = mockTransportationManager.Object
            };
            viewModel.RemoveEvent += (sender, e) => didRemovedEvent = true;
            var testScheduler = new TestScheduler();
            viewModel.RemoveCommand.Execute().Subscribe();
            testScheduler.Start();
            Assert.IsFalse(didRemovedEvent);
        }
    }
}