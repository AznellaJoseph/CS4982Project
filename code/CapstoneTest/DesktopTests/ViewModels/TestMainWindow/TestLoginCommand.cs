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
        public void TestLoginSuccessful()
        {
            var mockUserManager = new Mock<UserManager>();
            MainWindowViewModel mainWindowViewModel = new(mockUserManager.Object);
            var testScheduler = new TestScheduler();

            mainWindowViewModel.Username = "admin";
            mainWindowViewModel.Password = "admin";

            mainWindowViewModel.LoginCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(string.Empty, mainWindowViewModel.ErrorMessage);
        }

        [TestMethod]
        public void TestLoginUnsuccessful()
        {
            var mockUserManager = new Mock<UserManager>();
            MainWindowViewModel mainWindowViewModel = new(mockUserManager.Object);
            var testScheduler = new TestScheduler();

            mainWindowViewModel.Username = "admin test";
            mainWindowViewModel.Password = "admin test";

            mainWindowViewModel.LoginCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual("Username is incorrect.", mainWindowViewModel.ErrorMessage);
        }
    }
}