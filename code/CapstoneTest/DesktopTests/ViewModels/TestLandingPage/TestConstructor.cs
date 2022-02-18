using System.Collections.Generic;
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
        public void Constructor_ThreeParameters_PropertyCreations()
        {
            var mockUser = new Mock<User>
            {
                Object =
                {
                    UserId = 0
                }
            };
            var mockTripManager = new Mock<TripManager>();
            var mockScreen = new Mock<IScreen>();

            mockTripManager.Setup(tm => tm.GetTripsByUser(0))
                .Returns(new Response<IList<Trip>> {Data = new List<Trip> {new()}});

            LandingPageViewModel landingPageViewModel = new(mockUser.Object, mockTripManager.Object, mockScreen.Object);


            Assert.AreEqual(mockScreen.Object, landingPageViewModel.HostScreen);
            Assert.IsNotNull(landingPageViewModel.CreateTripCommand);
            Assert.IsNotNull(landingPageViewModel.LogoutCommand);
            Assert.IsNotNull(landingPageViewModel.UrlPathSegment);
            Assert.IsNotNull(landingPageViewModel.TripViewModels);
            Assert.AreEqual(1, landingPageViewModel.TripViewModels.Count);
        }

        [TestMethod]
        public void Constructor_TwoParameters_PropertyCreations()
        {
            var mockUser = new Mock<User>();
            var mockScreen = new Mock<IScreen>();
            LandingPageViewModel landingPageViewModel = new(mockUser.Object, mockScreen.Object);

            Assert.AreEqual(mockScreen.Object, landingPageViewModel.HostScreen);
            Assert.IsNotNull(landingPageViewModel.CreateTripCommand);
            Assert.IsNotNull(landingPageViewModel.LogoutCommand);
            Assert.IsNotNull(landingPageViewModel.UrlPathSegment);
            Assert.IsNotNull(landingPageViewModel.TripViewModels);
        }
    }
}