using System;
using CapstoneBackend.Model;
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

            createAccountPageViewModel.Username = "admin";
            createAccountPageViewModel.Password = "admin";
            createAccountPageViewModel.ConfirmedPassword = "notadmin";

            createAccountPageViewModel.SubmitAccountCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual("The given passwords must match.", createAccountPageViewModel.ErrorMessage);
        }

        [TestMethod]
        public void Login_SomeServerError_ReturnsErrorMessage()
        {
            var mockUserManager = new Mock<UserManager>();
            var mockScreen = new Mock<IScreen>();
            mockUserManager.Setup(mu => mu.RegisterUser("admin", "admin", "admin", "admin")).Returns(new Response<int>
            {
                StatusCode = 400U,
                ErrorMessage = "test"
            });
            CreateAccountPageViewModel createAccountPageViewModel = new(mockUserManager.Object, mockScreen.Object);
            TestScheduler testScheduler = new TestScheduler();
            
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
        public void Login_SomeServerError_ReturnsNoErrorMessage()
        {
            var mockUserManager = new Mock<UserManager>();
            var mockScreen = new Mock<IScreen>();
            mockUserManager.Setup(mu => mu.RegisterUser("admin", "admin", "admin", "admin")).Returns(new Response<int>
            {
                StatusCode = 400U
            });
            CreateAccountPageViewModel createAccountPageViewModel = new(mockUserManager.Object, mockScreen.Object);
            TestScheduler testScheduler = new TestScheduler();
            
            createAccountPageViewModel.FirstName = "admin";
            createAccountPageViewModel.LastName = "admin";
            createAccountPageViewModel.Username = "admin";
            createAccountPageViewModel.Password = "admin";
            createAccountPageViewModel.ConfirmedPassword = "admin";
            
            
            createAccountPageViewModel.SubmitAccountCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual("Unknown Error.", createAccountPageViewModel.ErrorMessage);
        }
    }
}