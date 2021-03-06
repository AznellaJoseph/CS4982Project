using System.Collections.Generic;
using CapstoneBackend.Model;
using CapstoneDesktop.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReactiveUI;

namespace CapstoneTest.DesktopTests.ViewModels.TestLandingPage
{
    [TestClass]
    public class TestLoadTrips
    {
        [TestMethod]
        public void LoadTrips_UserHasOneTrip_ReturnsCorrectCount()
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

            LandingPageViewModel landingPageViewModel = new(mockUser.Object, mockScreen.Object, mockTripManager.Object);

            Assert.AreEqual(1, landingPageViewModel.TripViewModels.Count);
        }

        [TestMethod]
        public void LoadTrips_UserHasNoTrips_ReturnsCorrectCount()
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

            mockTripManager.Setup(tm => tm.GetTripsByUser(0)).Returns(new Response<IList<Trip>> {Data = null});

            LandingPageViewModel landingPageViewModel = new(mockUser.Object, mockScreen.Object, mockTripManager.Object);

            Assert.AreEqual(0, landingPageViewModel.TripViewModels.Count);
        }
    }
}