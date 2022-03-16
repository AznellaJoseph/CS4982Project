using CapstoneBackend.Model;
using CapstoneDesktop.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReactiveUI;

namespace CapstoneTest.DesktopTests.ViewModels.TestTrip
{
    [TestClass]
    public class TestConstructor
    {
        [TestMethod]
        public void TestConstructor_Success()
        {
            var mockTrip = new Mock<Trip>();
            var mockScreen = new Mock<IScreen>();

            var tripViewModel = new TripViewModel(mockTrip.Object, mockScreen.Object);

            Assert.AreEqual(mockTrip.Object, tripViewModel.Trip);
            Assert.IsNotNull(tripViewModel.TripClickCommand);
        }
    }
}