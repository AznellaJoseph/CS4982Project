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
            var result = new TripViewModel(mockTrip.Object, mockScreen.Object);
            Assert.AreEqual(mockTrip.Object, result.Trip);
            Assert.IsNotNull(result.TripClickCommand);
        }
    }
}