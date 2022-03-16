using CapstoneDesktop.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReactiveUI;

namespace CapstoneTest.DesktopTests.ViewModels.TestLoginPage
{
    [TestClass]
    public class TestConstructor
    {
        [TestMethod]
        public void Constructor_PropertyCreations()
        {
            var mockScreen = new Mock<IScreen>();
            var mainWindowViewModel = new LoginPageViewModel(mockScreen.Object);

            Assert.AreEqual(string.Empty, mainWindowViewModel.ErrorMessage);
            Assert.IsNotNull(mainWindowViewModel.LoginCommand);
            Assert.IsNotNull(mainWindowViewModel.OpenCreateAccountCommand);
            Assert.IsNotNull(mainWindowViewModel.HostScreen);
            Assert.IsNotNull(mainWindowViewModel.UrlPathSegment);
            Assert.IsNotNull(mainWindowViewModel.UserManager);
            Assert.IsNull(mainWindowViewModel.Username);
            Assert.IsNull(mainWindowViewModel.Password);
        }
    }
}