using CapstoneBackend.Model;
using CapstoneDesktop.ViewModels;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace CapstoneTest.DesktopTests.ViewModels.TestMainWindow
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
                Id = 0
            };
            var mockUserManager = new Mock<UserManager>();
            mockUserManager.Setup(um => um.GetUserByCredentials("admin", "admin")).Returns(new Response<User> { Data = user });
            MainWindowViewModel mainWindowViewModel = new(mockUserManager.Object);
            var testScheduler = new TestScheduler();

            mainWindowViewModel.Username = "admin";
            mainWindowViewModel.Password = "admin";

            mainWindowViewModel.LoginCommand.Execute().Subscribe();
            testScheduler.Start();
            Assert.AreEqual(string.Empty, mainWindowViewModel.ErrorMessage);
        }

        [TestMethod]
        public void Login_IncorrectUsername_ReturnsErrorMessage()
        {
            var mockUserManager = new Mock<UserManager>();
            mockUserManager.Setup(um => um.GetUserByCredentials("admin", "admin")).Returns(new Response<User> { ErrorMessage = "Username is incorrect." });
            MainWindowViewModel mainWindowViewModel = new(mockUserManager.Object);
            var testScheduler = new TestScheduler();

            mainWindowViewModel.Username = "admin";
            mainWindowViewModel.Password = "admin";

            mainWindowViewModel.LoginCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual("Username is incorrect.", mainWindowViewModel.ErrorMessage);
        }

        [TestMethod]
        public void Login_NullErrorMessage_ReturnsEmptyErrorMessage()
        {
            var mockUserManager = new Mock<UserManager>();
            mockUserManager.Setup(um => um.GetUserByCredentials("admin", "admin")).Returns(new Response<User> { ErrorMessage = null });
            MainWindowViewModel mainWindowViewModel = new(mockUserManager.Object);
            var testScheduler = new TestScheduler();

            mainWindowViewModel.Username = "admin";
            mainWindowViewModel.Password = "admin";

            mainWindowViewModel.LoginCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(string.Empty, mainWindowViewModel.ErrorMessage);
        }

        [TestMethod]
        public void Login_NullCredentials_ReturnsErrorMessage()
        {
            var mockUserManager = new Mock<UserManager>();
            mockUserManager.Setup(um => um.GetUserByCredentials("", "")).Returns(new Response<User> { ErrorMessage = "Username is incorrect." });
            MainWindowViewModel mainWindowViewModel = new(mockUserManager.Object);
            var testScheduler = new TestScheduler();

            mainWindowViewModel.Username = null;
            mainWindowViewModel.Password = null;

            mainWindowViewModel.LoginCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual("Username is incorrect.", mainWindowViewModel.ErrorMessage);
        }

    }
}