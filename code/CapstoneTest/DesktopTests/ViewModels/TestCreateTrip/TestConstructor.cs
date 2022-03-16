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
        public void Constructor_PropertyCreations()
        {
            var mockUser = new Mock<User>();
            var mockScreen = new Mock<IScreen>();
            CreateTripPageViewModel createTripWindowViewModel =
                new(mockUser.Object, mockScreen.Object);

            Assert.AreEqual(mockScreen.Object, createTripWindowViewModel.HostScreen);
            Assert.AreEqual(string.Empty, createTripWindowViewModel.ErrorMessage);
            Assert.IsNotNull(createTripWindowViewModel.CancelCreateTripCommand);
            Assert.IsNotNull(createTripWindowViewModel.CreateTripCommand);
            Assert.IsNotNull(createTripWindowViewModel.UrlPathSegment);
            Assert.IsNotNull(createTripWindowViewModel.ValidationManager);
            Assert.IsNotNull(createTripWindowViewModel.TripManager);
            Assert.IsNull(createTripWindowViewModel.StartDate);
            Assert.IsNull(createTripWindowViewModel.EndDate);
            Assert.IsNull(createTripWindowViewModel.TripName);
            Assert.IsNull(createTripWindowViewModel.Notes);
        }
    }
}