using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapstoneBackend.Model;
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
        public void Constructor_TwoParameter_PropertyCreations()
        {
            var mockUserManager = new Mock<UserManager>();
            var mockScreen = new Mock<IScreen>();
            CreateAccountPageViewModel createAccountPageViewModel = new(mockUserManager.Object, mockScreen.Object);

            Assert.AreEqual(mockScreen.Object, createAccountPageViewModel.HostScreen);
            Assert.AreEqual(string.Empty, createAccountPageViewModel.ErrorMessage);
            Assert.IsNotNull(createAccountPageViewModel.UrlPathSegment);
            Assert.IsNotNull(createAccountPageViewModel.CancelCreateAccountCommand);
            Assert.IsNotNull(createAccountPageViewModel.SubmitAccountCommand);
            Assert.IsNull(createAccountPageViewModel.Username);
            Assert.IsNull(createAccountPageViewModel.Password);
            Assert.IsNull(createAccountPageViewModel.ConfirmedPassword);
            Assert.IsNull(createAccountPageViewModel.FirstName);
            Assert.IsNull(createAccountPageViewModel.LastName);
        }

        [TestMethod]
        public void Constructor_OneParameter_PropertyCreations()
        {
            var mockScreen = new Mock<IScreen>();
            var createAccountPageViewModel = new CreateAccountPageViewModel(mockScreen.Object);

            Assert.AreEqual(mockScreen.Object, createAccountPageViewModel.HostScreen);
            Assert.AreEqual(string.Empty, createAccountPageViewModel.ErrorMessage);
            Assert.IsNotNull(createAccountPageViewModel.UrlPathSegment);
            Assert.IsNotNull(createAccountPageViewModel.CancelCreateAccountCommand);
            Assert.IsNotNull(createAccountPageViewModel.SubmitAccountCommand);
            Assert.IsNull(createAccountPageViewModel.Username);
            Assert.IsNull(createAccountPageViewModel.Password);
            Assert.IsNull(createAccountPageViewModel.ConfirmedPassword);
            Assert.IsNull(createAccountPageViewModel.FirstName);
            Assert.IsNull(createAccountPageViewModel.LastName);
        }
    }
}
