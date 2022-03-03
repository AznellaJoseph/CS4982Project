using System;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using CapstoneDesktop.ViewModels;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReactiveUI;

namespace CapstoneTest.DesktopTests.ViewModels.TestCreateAccount
{
    [TestClass]
    public class TestSubmitAccount
    {
        [TestMethod]
        public void CreateAccount_ValidCredentials_ReturnsNoError()
        {
            var mockUserManager = new Mock<UserManager>();
            mockUserManager.Setup(mu => mu.RegisterUser("admin", "admin", "admin", "admin")).Returns(new Response<int>
            {
                Data = 1
            });
            var mockScreen = new Mock<IScreen>();
            CreateAccountPageViewModel createAccountPageViewModel = new(mockUserManager.Object, mockScreen.Object);
            var testScheduler = new TestScheduler();

            createAccountPageViewModel.FirstName = "admin";
            createAccountPageViewModel.LastName = "admin";
            createAccountPageViewModel.Username = "admin";
            createAccountPageViewModel.Password = "admin";
            createAccountPageViewModel.ConfirmedPassword = "admin";

            createAccountPageViewModel.SubmitAccountCommand.ThrownExceptions.Subscribe();
            testScheduler.Start();
            Assert.AreEqual(string.Empty, createAccountPageViewModel.ErrorMessage);
        }

        [TestMethod]
        public void CreateAccount_DifferentPassword_ReturnsErrorMessage()
        {
            var mockUserManager = new Mock<UserManager>();
            var mockScreen = new Mock<IScreen>();
            CreateAccountPageViewModel createAccountPageViewModel = new(mockUserManager.Object, mockScreen.Object);
            var testScheduler = new TestScheduler();

            createAccountPageViewModel.FirstName = "admin";
            createAccountPageViewModel.LastName = "admin";
            createAccountPageViewModel.Username = "admin";
            createAccountPageViewModel.Password = "admin";
            createAccountPageViewModel.ConfirmedPassword = "notadmin";

            createAccountPageViewModel.SubmitAccountCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(Ui.ErrorMessages.PasswordsDoNotMatch, createAccountPageViewModel.ErrorMessage);
        }

        [TestMethod]
        public void CreateAccount_SomeServerError_ReturnsErrorMessage()
        {
            var mockUserManager = new Mock<UserManager>();
            var mockScreen = new Mock<IScreen>();
            mockUserManager.Setup(mu => mu.RegisterUser("admin", "admin", "admin", "admin")).Returns(new Response<int>
            {
                StatusCode = (uint) Ui.StatusCode.BadRequest,
                ErrorMessage = "test"
            });
            CreateAccountPageViewModel createAccountPageViewModel = new(mockUserManager.Object, mockScreen.Object);
            TestScheduler testScheduler = new();

            createAccountPageViewModel.FirstName = "admin";
            createAccountPageViewModel.LastName = "admin";
            createAccountPageViewModel.Username = "admin";
            createAccountPageViewModel.Password = "admin";
            createAccountPageViewModel.ConfirmedPassword = "admin";


            createAccountPageViewModel.SubmitAccountCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual("test", createAccountPageViewModel.ErrorMessage);
        }

        [TestMethod]
        public void CreateAccount_SomeServerError_ReturnsNoErrorMessage()
        {
            var mockUserManager = new Mock<UserManager>();
            var mockScreen = new Mock<IScreen>();
            mockUserManager.Setup(mu => mu.RegisterUser("admin", "admin", "admin", "admin")).Returns(new Response<int>
            {
                StatusCode = (uint) Ui.StatusCode.BadRequest
            });
            CreateAccountPageViewModel createAccountPageViewModel = new(mockUserManager.Object, mockScreen.Object);
            TestScheduler testScheduler = new();

            createAccountPageViewModel.FirstName = "admin";
            createAccountPageViewModel.LastName = "admin";
            createAccountPageViewModel.Username = "admin";
            createAccountPageViewModel.Password = "admin";
            createAccountPageViewModel.ConfirmedPassword = "admin";


            createAccountPageViewModel.SubmitAccountCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(Ui.ErrorMessages.UnknownError, createAccountPageViewModel.ErrorMessage);
        }

        [TestMethod]
        public void CreateAccount_EmptyFirstName_ReturnsErrorMessage()
        {
            var mockScreen = new Mock<IScreen>();

            CreateAccountPageViewModel createAccountPageViewModel = new(mockScreen.Object);
            TestScheduler testScheduler = new();

            createAccountPageViewModel.LastName = "admin";
            createAccountPageViewModel.Username = "admin";
            createAccountPageViewModel.Password = "admin";
            createAccountPageViewModel.ConfirmedPassword = "admin";
            createAccountPageViewModel.SubmitAccountCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(Ui.ErrorMessages.InvalidFirstName, createAccountPageViewModel.ErrorMessage);
        }

        [TestMethod]
        public void CreateAccount_EmptyLastName_ReturnsErrorMessage()
        {
            var mockScreen = new Mock<IScreen>();

            CreateAccountPageViewModel createAccountPageViewModel = new(mockScreen.Object);
            TestScheduler testScheduler = new();

            createAccountPageViewModel.FirstName = "admin";
            createAccountPageViewModel.Username = "admin";
            createAccountPageViewModel.Password = "admin";
            createAccountPageViewModel.ConfirmedPassword = "admin";
            createAccountPageViewModel.SubmitAccountCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(Ui.ErrorMessages.InvalidLastName, createAccountPageViewModel.ErrorMessage);
        }

        [TestMethod]
        public void CreateAccount_EmptyUsername_ReturnsErrorMessage()
        {
            var mockScreen = new Mock<IScreen>();

            CreateAccountPageViewModel createAccountPageViewModel = new(mockScreen.Object);
            TestScheduler testScheduler = new();

            createAccountPageViewModel.FirstName = "admin";
            createAccountPageViewModel.LastName = "admin";
            createAccountPageViewModel.Password = "admin";
            createAccountPageViewModel.ConfirmedPassword = "admin";
            createAccountPageViewModel.SubmitAccountCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(Ui.ErrorMessages.InvalidUsername, createAccountPageViewModel.ErrorMessage);
        }

        [TestMethod]
        public void CreateAccount_EmptyPassword_ReturnsErrorMessage()
        {
            var mockScreen = new Mock<IScreen>();

            CreateAccountPageViewModel createAccountPageViewModel = new(mockScreen.Object);
            TestScheduler testScheduler = new();

            createAccountPageViewModel.FirstName = "admin";
            createAccountPageViewModel.LastName = "admin";
            createAccountPageViewModel.Username = "admin";
            createAccountPageViewModel.ConfirmedPassword = "admin";
            createAccountPageViewModel.SubmitAccountCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(Ui.ErrorMessages.InvalidPassword, createAccountPageViewModel.ErrorMessage);
        }

        [TestMethod]
        public void CreateAccount_EmptyConfirmedPassword_ReturnsErrorMessage()
        {
            var mockScreen = new Mock<IScreen>();

            CreateAccountPageViewModel createAccountPageViewModel = new(mockScreen.Object);
            TestScheduler testScheduler = new();

            createAccountPageViewModel.FirstName = "admin";
            createAccountPageViewModel.LastName = "admin";
            createAccountPageViewModel.Username = "admin";
            createAccountPageViewModel.Password = "admin";
            createAccountPageViewModel.SubmitAccountCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(Ui.ErrorMessages.InvalidConfirmedPassword, createAccountPageViewModel.ErrorMessage);
        }
    }
}