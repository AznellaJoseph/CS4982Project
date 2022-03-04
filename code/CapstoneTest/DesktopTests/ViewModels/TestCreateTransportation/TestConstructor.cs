using CapstoneBackend.Model;
using CapstoneDesktop.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReactiveUI;

namespace CapstoneTest.DesktopTests.ViewModels.TestCreateTransportation
{
    [TestClass]
    public class TestConstructor
    {
        [TestMethod]
        public void Constructor_ThreeParameter_PropertyCreations()
        {
            var mockTrip = new Mock<Trip>();
            var mockTransportationManager = new Mock<TransportationManager>();
            var mockScreen = new Mock<IScreen>();
            CreateTransportationPageViewModel createTransportationViewModel =
                new(mockTrip.Object, mockTransportationManager.Object, mockScreen.Object);

            Assert.AreEqual(string.Empty, createTransportationViewModel.ErrorMessage);
            Assert.AreEqual(mockScreen.Object, createTransportationViewModel.HostScreen);
            Assert.IsNotNull(createTransportationViewModel.UrlPathSegment);
            Assert.IsNotNull(createTransportationViewModel.CreateTransportationCommand);
            Assert.IsNotNull(createTransportationViewModel.CancelCreateTransportationCommand);
            Assert.IsNull(createTransportationViewModel.StartTime);
            Assert.IsNull(createTransportationViewModel.EndTime);
            Assert.IsNull(createTransportationViewModel.StartDate);
            Assert.IsNull(createTransportationViewModel.EndDate);
            Assert.IsNull(createTransportationViewModel.Method);
        }

        [TestMethod]
        public void Constructor_TwoParameter_PropertyCreations()
        {
            var mockTrip = new Mock<Trip>();
            var mockScreen = new Mock<IScreen>();
            CreateTransportationPageViewModel createTransportationViewModel = new(mockTrip.Object, mockScreen.Object);

            Assert.AreEqual(string.Empty, createTransportationViewModel.ErrorMessage);
            Assert.AreEqual(mockScreen.Object, createTransportationViewModel.HostScreen);
            Assert.IsNotNull(createTransportationViewModel.UrlPathSegment);
            Assert.IsNotNull(createTransportationViewModel.CreateTransportationCommand);
            Assert.IsNotNull(createTransportationViewModel.CancelCreateTransportationCommand);
            Assert.IsNull(createTransportationViewModel.StartTime);
            Assert.IsNull(createTransportationViewModel.EndTime);
            Assert.IsNull(createTransportationViewModel.StartDate);
            Assert.IsNull(createTransportationViewModel.EndDate);
            Assert.IsNull(createTransportationViewModel.Method);
        }
    }
}