using System;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using CapstoneDesktop.ViewModels;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReactiveUI;

namespace CapstoneTest.DesktopTests.ViewModels.TestLoginPage
{
    [TestClass]
    public class TestLoginCommand
    {
        [TestMethod]
        public void Login_ValidCredentials_ReturnsNoError()
        {
            var user = new User
            {
                FirstName = "admin",
                LastName = "admin",
                Username = "admin",
                Password = "admin",
                UserId = 0
            };
            var mockUserManager = new Mock<UserManager>();
            var mockScreen = new Mock<IScreen>();
            mockUserManager.Setup(um => um.GetUserByCredentials("admin", "admin"))
                .Returns(new Response<User> { Data = user });
            LoginPageViewModel loginPageViewModel = new(mockUserManager.Object, mockScreen.Object);
            var testScheduler = new TestScheduler();

            loginPageViewModel.Username = "admin";
            loginPageViewModel.Password = "admin";

            loginPageViewModel.LoginCommand.ThrownExceptions.Subscribe();
            testScheduler.Start();
            Assert.AreEqual(string.Empty, loginPageViewModel.ErrorMessage);
        }

        [TestMethod]
        public void Login_IncorrectUsername_ReturnsErrorMessage()
        {
            var mockUserManager = new Mock<UserManager>();
            var mockScreen = new Mock<IScreen>();
            mockUserManager.Setup(um => um.GetUserByCredentials("admin", "admin"))
                .Returns(new Response<User> { ErrorMessage = Ui.ErrorMessages.IncorrectUsername });
            LoginPageViewModel loginPageViewModel = new(mockUserManager.Object, mockScreen.Object);
            var testScheduler = new TestScheduler();

            loginPageViewModel.Username = "admin";
            loginPageViewModel.Password = "admin";

            loginPageViewModel.LoginCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(Ui.ErrorMessages.IncorrectUsername, loginPageViewModel.ErrorMessage);
        }

        [TestMethod]
        public void Login_NullErrorMessage_ReturnsEmptyErrorMessage()
        {
            var mockUserManager = new Mock<UserManager>();
            var mockScreen = new Mock<IScreen>();
            mockUserManager.Setup(um => um.GetUserByCredentials("admin", "admin"))
                .Returns(new Response<User> { ErrorMessage = null });
            LoginPageViewModel loginPageViewModel = new(mockUserManager.Object, mockScreen.Object);
            var testScheduler = new TestScheduler();

            loginPageViewModel.Username = "admin";
            loginPageViewModel.Password = "admin";

            loginPageViewModel.LoginCommand.ThrownExceptions.Subscribe();

            testScheduler.Start();

            Assert.AreEqual(string.Empty, loginPageViewModel.ErrorMessage);
        }

        [TestMethod]
        public void Login_NullCredentials_ReturnsErrorMessage()
        {
            var mockScreen = new Mock<IScreen>();

            LoginPageViewModel loginPageViewModel = new(mockScreen.Object);
            var testScheduler = new TestScheduler();

            loginPageViewModel.LoginCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(Ui.ErrorMessages.InvalidFields, loginPageViewModel.ErrorMessage);
        }
    }
}