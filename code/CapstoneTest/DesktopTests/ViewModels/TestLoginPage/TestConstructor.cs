using CapstoneBackend.Model;
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
        public void Constructor_OneParameter_PropertyCreations()
        {
            var mockUserManager = new Mock<UserManager>();
            var mockScreen = new Mock<IScreen>();
            LoginPageViewModel mainWindowViewModel = new(mockUserManager.Object, mockScreen.Object);

            Assert.IsNotNull(mainWindowViewModel.LoginCommand);
            Assert.IsNotNull(mainWindowViewModel.OpenCreateAccountCommand);
            Assert.IsNotNull(mainWindowViewModel.HostScreen);
            Assert.IsNotNull(mainWindowViewModel.UrlPathSegment);
            Assert.IsNull(mainWindowViewModel.Username);
            Assert.IsNull(mainWindowViewModel.Password);
            Assert.AreEqual(string.Empty, mainWindowViewModel.ErrorMessage);
        }

        [TestMethod]
        public void Constructor_NoParameters_PropertyCreations()
        {
            var mockScreen = new Mock<IScreen>();
            var mainWindowViewModel = new LoginPageViewModel(mockScreen.Object);

            Assert.IsNotNull(mainWindowViewModel.LoginCommand);
            Assert.IsNotNull(mainWindowViewModel.OpenCreateAccountCommand);
            Assert.IsNotNull(mainWindowViewModel.HostScreen);
            Assert.IsNotNull(mainWindowViewModel.UrlPathSegment);
            Assert.IsNull(mainWindowViewModel.Username);
            Assert.IsNull(mainWindowViewModel.Password);
            Assert.AreEqual(string.Empty, mainWindowViewModel.ErrorMessage);
        }
    }
}