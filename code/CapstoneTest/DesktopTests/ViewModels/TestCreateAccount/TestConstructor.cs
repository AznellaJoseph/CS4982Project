using CapstoneDesktop.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReactiveUI;

namespace CapstoneTest.DesktopTests.ViewModels.TestCreateAccount
{
    [TestClass]
    public class TestConstructor
    {
        [TestMethod]
        public void Constructor_PropertyCreations()
        {
            var mockScreen = new Mock<IScreen>();
            CreateAccountPageViewModel createAccountPageViewModel = new(mockScreen.Object);

            Assert.AreEqual(mockScreen.Object, createAccountPageViewModel.HostScreen);
            Assert.AreEqual(string.Empty, createAccountPageViewModel.ErrorMessage);
            Assert.IsNotNull(createAccountPageViewModel.UrlPathSegment);
            Assert.IsNotNull(createAccountPageViewModel.CancelCreateAccountCommand);
            Assert.IsNotNull(createAccountPageViewModel.SubmitAccountCommand);
            Assert.IsNotNull(createAccountPageViewModel.UserManager);
            Assert.IsNull(createAccountPageViewModel.Username);
            Assert.IsNull(createAccountPageViewModel.Password);
            Assert.IsNull(createAccountPageViewModel.ConfirmedPassword);
            Assert.IsNull(createAccountPageViewModel.FirstName);
            Assert.IsNull(createAccountPageViewModel.LastName);
        }
    }
}