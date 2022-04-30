using CapstoneBackend.Model;
using CapstoneDesktop.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReactiveUI;

namespace CapstoneTest.DesktopTests.ViewModels.TestTransportation
{
    [TestClass]
    public class TestConstructor
    {
        [TestMethod]
        public void Constructor_PropertyCreations()
        {
            var mockTransportation = new Mock<Transportation>();
            var mockScreen = new Mock<IScreen>();

            var transportationViewModel = new TransportationViewModel(mockTransportation.Object, mockScreen.Object);

            Assert.AreEqual(mockTransportation.Object, transportationViewModel.Transportation);
            Assert.AreEqual(mockTransportation.Object, transportationViewModel.Event);
            Assert.IsNotNull(transportationViewModel.HostScreen);
            Assert.IsNotNull(transportationViewModel.TransportationManager);
            Assert.IsNotNull(transportationViewModel.RemoveCommand);
            Assert.IsNotNull(transportationViewModel.ViewCommand);
            Assert.IsNotNull(transportationViewModel.EditCommand);
            Assert.IsNotNull(transportationViewModel.ImagePath);
        }
    }
}