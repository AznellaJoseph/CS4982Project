using CapstoneBackend.Model;
using CapstoneDesktop.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReactiveUI;

namespace CapstoneTest.DesktopTests.ViewModels.TestCreateTrip
{
    [TestClass]
    public class TestConstructor
    {
        [TestMethod]
        public void Constructor_OneParameter_PropertyCreations()
        {
            var mockUser = new Mock<User>();
            var mockTripManager = new Mock<TripManager>();
            var mockScreen = new Mock<IScreen>();
            CreateTripPageViewModel createTripWindowViewModel =
                new(mockUser.Object, mockTripManager.Object, mockScreen.Object);

            Assert.IsNotNull(createTripWindowViewModel.CancelCreateTripCommand);
            Assert.IsNotNull(createTripWindowViewModel.CreateTripCommand);
            Assert.AreEqual(string.Empty, createTripWindowViewModel.ErrorMessage);
            Assert.IsNull(createTripWindowViewModel.StartDate);
            Assert.IsNull(createTripWindowViewModel.EndDate);
            Assert.IsNull(createTripWindowViewModel.TripName);
            Assert.IsNull(createTripWindowViewModel.Notes);
        }

        [TestMethod]
        public void Constructor_NoParameters_PropertyCreations()
        {
            var mockUser = new Mock<User>();
            var mockTripManager = new Mock<TripManager>();
            var mockScreen = new Mock<IScreen>();
            CreateTripPageViewModel createTripWindowViewModel =
                new(mockUser.Object, mockTripManager.Object, mockScreen.Object);

            Assert.IsNotNull(createTripWindowViewModel.CancelCreateTripCommand);
            Assert.IsNotNull(createTripWindowViewModel.CreateTripCommand);
            Assert.AreEqual(string.Empty, createTripWindowViewModel.ErrorMessage);
            Assert.IsNull(createTripWindowViewModel.StartDate);
            Assert.IsNull(createTripWindowViewModel.EndDate);
            Assert.IsNull(createTripWindowViewModel.TripName);
            Assert.IsNull(createTripWindowViewModel.Notes);
        }
    }
}