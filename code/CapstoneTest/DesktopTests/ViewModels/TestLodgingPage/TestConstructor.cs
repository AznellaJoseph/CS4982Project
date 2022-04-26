using CapstoneBackend.Model;
using CapstoneDesktop.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReactiveUI;

namespace CapstoneTest.DesktopTests.ViewModels.TestLodgingPage
{
    [TestClass]
    public class TestConstructor
    {
        [TestMethod]
        public void Constructor_PropertyCreations()
        {
            var mockLodging = new Mock<Lodging>();
            var mockScreen = new Mock<IScreen>();
            var lodgingPageViewModel = new LodgingPageViewModel(mockLodging.Object, mockScreen.Object);

            Assert.IsNotNull(lodgingPageViewModel.BackCommand);
            Assert.IsNotNull(lodgingPageViewModel.Lodging);
            Assert.IsNotNull(lodgingPageViewModel.LogoutCommand);
        }
    }
}