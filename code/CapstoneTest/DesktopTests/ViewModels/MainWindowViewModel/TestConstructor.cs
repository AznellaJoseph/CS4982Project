using CapstoneBackend.Model;
using CapstoneDesktop.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CapstoneTest.DesktopTests.ViewModels.MainWindowViewModel
{

    [TestClass]
    public class TestConstructor
    {
        [TestMethod]
        public void TestPropertyCreations()
        {
            var mockUserManager = new Mock<UserManager>();
            CapstoneDesktop.ViewModels.MainWindowViewModel mainWindowViewModel = new(mockUserManager.Object);

            Assert.IsNotNull(mainWindowViewModel.CancelCreateAccountCommand);
            Assert.IsNotNull(mainWindowViewModel.LoginCommand);
            Assert.IsNotNull(mainWindowViewModel.OpenCreateAccountCommand);
            Assert.IsNotNull(mainWindowViewModel.SubmitAccountCommand);
            Assert.IsNull(mainWindowViewModel.FirstName);
            Assert.IsNull(mainWindowViewModel.LastName);
            Assert.IsNull(mainWindowViewModel.Username);
            Assert.IsNull(mainWindowViewModel.Password);
            Assert.AreEqual(string.Empty, mainWindowViewModel.ErrorMessage);
            Assert.AreEqual(true, mainWindowViewModel.LoginControlsVisible);
        }
    }
}