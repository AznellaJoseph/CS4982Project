using CapstoneBackend.Model;
using CapstoneDesktop.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReactiveUI;

namespace CapstoneTest.DesktopTests.ViewModels.TestEventPage
{
    [TestClass]
    public class TestConstructor
    {
        [TestMethod]
        public void Constructor_PropertyCreations()
        {
            var mockEvent = new Mock<IEvent>();
            var mockScreen = new Mock<IScreen>();
            var viewModel = new EventPageViewModel(mockEvent.Object, mockScreen.Object);
            Assert.IsNotNull(viewModel.Event);
            Assert.IsNotNull(viewModel.BackCommand);
            Assert.IsNotNull(viewModel.LogoutCommand);

        }
    }
}