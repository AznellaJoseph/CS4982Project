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
    public class TestRemoveCommand
    {
        [TestMethod]
        public void RemoveCommand_Success()
        {
            var mockTransportationManager = new Mock<TransportationManager>();
            var transportation = new Transportation
            {
                TransportationId = 1
            };
            var mockScreen = new Mock<IScreen>();
            mockTransportationManager.Setup(tm => tm.RemoveTransportation(1))
                .Returns(new Response<bool>
                {
                    Data = true
                });

            var eventRemoved = false;

            var viewModel = new TransportationViewModel(transportation, mockScreen.Object)
            {
                TransportationManager = mockTransportationManager.Object
            };
            viewModel.RemoveEvent += (_, _) => eventRemoved = true;

            var testScheduler = new TestScheduler();

            viewModel.RemoveCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.IsTrue(eventRemoved);
        }

        [TestMethod]
        public void RemoveCommand_Failure()
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
            var eventRemoved = false;
            var viewModel = new TransportationViewModel(transportation, mockScreen.Object)
            {
                TransportationManager = mockTransportationManager.Object
            };
            viewModel.RemoveEvent += (_, _) => eventRemoved = true;

            var testScheduler = new TestScheduler();

            viewModel.RemoveCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.IsFalse(eventRemoved);
        }
    }
}