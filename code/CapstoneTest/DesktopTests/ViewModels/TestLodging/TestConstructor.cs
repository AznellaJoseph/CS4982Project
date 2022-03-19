using CapstoneBackend.Model;
using CapstoneDesktop.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReactiveUI;

namespace CapstoneTest.DesktopTests.ViewModels.TestLodging
{
    [TestClass]
    public class TestConstructor
    {
        [TestMethod]
        public void Constructor_PropertyCreations()
        {
            var mockLodging = new Mock<Lodging>();
            var mockScreen = new Mock<IScreen>();

            var lodgingViewModel = new LodgingViewModel(mockLodging.Object, mockScreen.Object);

            Assert.AreEqual(mockLodging.Object, lodgingViewModel.Lodging);
            Assert.IsNotNull(lodgingViewModel.HostScreen);
            Assert.IsNotNull(lodgingViewModel.LodgingManager);
            Assert.IsNotNull(lodgingViewModel.RemoveCommand);
        }
    }
}