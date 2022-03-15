using System;
using CapstoneBackend.Model;
using CapstoneDesktop.ViewModels;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReactiveUI;

namespace CapstoneTest.DesktopTests.ViewModels.TestLodging
{
    [TestClass]
    public class TestRemoveCommand
    {
        [TestMethod]
        public void RemoveCommand_Success()
        {
            var mockLodgingManager = new Mock<LodgingManager>();
            var lodging = new Lodging
            {
                LodgingId = 1
            };
            var mockScreen = new Mock<IScreen>();
            mockLodgingManager.Setup(tm => tm.RemoveLodging(1))
                .Returns(new Response<bool>
                {
                    Data = true
                });

            var eventRemoved = false;

            var viewModel = new LodgingViewModel(lodging, mockScreen.Object)
            {
                LodgingManager = mockLodgingManager.Object
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
            var mockLodgingManager = new Mock<LodgingManager>();
            var lodging = new Lodging
            {
                LodgingId = 1
            };
            var mockScreen = new Mock<IScreen>();
            mockLodgingManager.Setup(wm => wm.RemoveLodging(1))
                .Returns(new Response<bool>
                {
                    Data = false
                });
            var eventRemoved = false;
            var viewModel = new LodgingViewModel(lodging, mockScreen.Object)
            {
                LodgingManager = mockLodgingManager.Object
            };
            viewModel.RemoveEvent += (_, _) => eventRemoved = true;

            var testScheduler = new TestScheduler();

            viewModel.RemoveCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.IsFalse(eventRemoved);
        }
    }
}