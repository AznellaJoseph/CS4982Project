using CapstoneBackend.Model;
using CapstoneDesktop.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using Microsoft.Reactive.Testing;

namespace CapstoneTest.DesktopTests.ViewModels.TestMainWindow
{

    [TestClass]
    public class TestSubmitAccountCommand
    {
        [TestMethod]
        public void SubmitAccount_RegisterValidUser_ReturnsNoError()
        {
            var mockUserManager = new Mock<UserManager>();
            mockUserManager.Setup(um => um.RegisterUser("admin", "admin", "admin", "admin")).Returns(new Response<int> { Data = 0, StatusCode = 200 });
            MainWindowViewModel mainWindowViewModel = new(mockUserManager.Object);
            var testScheduler = new TestScheduler();

            mainWindowViewModel.Username = "admin";
            mainWindowViewModel.Password = "admin";
            mainWindowViewModel.ConfirmedPassword = "admin";
            mainWindowViewModel.FirstName = "admin";
            mainWindowViewModel.LastName = "admin";

            mainWindowViewModel.SubmitAccountCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(string.Empty, mainWindowViewModel.ErrorMessage);

        }

        [TestMethod]
        public void SubmitAccount_PasswordMisMatch_ReturnsError()
        {
            MainWindowViewModel mainWindowViewModel = new();
            var testScheduler = new TestScheduler();

            mainWindowViewModel.Username = "admin";
            mainWindowViewModel.Password = "admin";
            mainWindowViewModel.ConfirmedPassword = "Passwords Dont Match";
            mainWindowViewModel.FirstName = "admin";
            mainWindowViewModel.LastName = "admin";

            mainWindowViewModel.SubmitAccountCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual("The given passwords must match.", mainWindowViewModel.ErrorMessage);

        }

        [TestMethod]
        public void SubmitAccount_RegisterAlreadyExistingUser_ReturnsError()
        {
            var mockUserManager = new Mock<UserManager>();
            mockUserManager.Setup(um => um.RegisterUser("admin", "admin", "admin", "admin")).Returns(new Response<int> { StatusCode = 404, ErrorMessage = "Username is taken." });
            MainWindowViewModel mainWindowViewModel = new(mockUserManager.Object);
            var testScheduler = new TestScheduler();

            mainWindowViewModel.Username = "admin";
            mainWindowViewModel.Password = "admin";
            mainWindowViewModel.ConfirmedPassword = "admin";
            mainWindowViewModel.FirstName = "admin";
            mainWindowViewModel.LastName = "admin";

            mainWindowViewModel.SubmitAccountCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual("Username is taken.", mainWindowViewModel.ErrorMessage);
        }

        [TestMethod]
        public void SubmitAccount_UnsuccessfulRegister_ReturnsUnknownError()
        {
            var mockUserManager = new Mock<UserManager>();
            mockUserManager.Setup(um => um.RegisterUser("admin", "admin", "admin", "admin")).Returns(new Response<int> { StatusCode = 404 });
            MainWindowViewModel mainWindowViewModel = new(mockUserManager.Object);
            var testScheduler = new TestScheduler();

            mainWindowViewModel.Username = "admin";
            mainWindowViewModel.Password = "admin";
            mainWindowViewModel.ConfirmedPassword = "admin";
            mainWindowViewModel.FirstName = "admin";
            mainWindowViewModel.LastName = "admin";

            mainWindowViewModel.SubmitAccountCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual("Unknown Error.", mainWindowViewModel.ErrorMessage);
        }

        [TestMethod]
        public void SubmitAccount_RegisterWithNullCredentials_ReturnsInternalServerError()
        {
            var mockUserManager = new Mock<UserManager>();
            mockUserManager.Setup(um => um.RegisterUser("", "", "", "")).Returns(new Response<int> { StatusCode = 404, ErrorMessage = "Internal Server Error." });
            MainWindowViewModel mainWindowViewModel = new(mockUserManager.Object);
            var testScheduler = new TestScheduler();

            mainWindowViewModel.Username = null;
            mainWindowViewModel.Password = null;
            mainWindowViewModel.ConfirmedPassword = null;
            mainWindowViewModel.FirstName = null;
            mainWindowViewModel.LastName = null;

            mainWindowViewModel.SubmitAccountCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual("Internal Server Error.", mainWindowViewModel.ErrorMessage);
        }
    }
}