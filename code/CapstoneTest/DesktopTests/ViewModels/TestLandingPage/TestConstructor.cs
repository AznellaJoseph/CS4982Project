using CapstoneBackend.Model;
using CapstoneDesktop.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReactiveUI;

namespace CapstoneTest.DesktopTests.ViewModels.TestLandingPage
{
    [TestClass]
    public class TestConstructor
    {
        [TestMethod]
        public void Constructor_PropertyCreations()
        {
            var mockUser = new Mock<User>();
            var mockScreen = new Mock<IScreen>();
            LandingPageViewModel landingPageViewModel = new(mockUser.Object, mockScreen.Object);

            Assert.AreEqual(mockScreen.Object, landingPageViewModel.HostScreen);
            Assert.IsNotNull(landingPageViewModel.CreateTripCommand);
            Assert.IsNotNull(landingPageViewModel.LogoutCommand);
            Assert.IsNotNull(landingPageViewModel.UrlPathSegment);
            Assert.IsNotNull(landingPageViewModel.TripViewModels);
            Assert.IsNotNull(landingPageViewModel.TripManager);
        }
    }
}