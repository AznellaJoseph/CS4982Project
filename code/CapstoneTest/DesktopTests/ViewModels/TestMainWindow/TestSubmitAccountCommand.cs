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
        public void TestCommandSuccessfulSubmit()
        {
            var mockUserManager = new Mock<UserManager>();
            MainWindowViewModel mainWindowViewModel = new(mockUserManager.Object);
            var testScheduler = new TestScheduler();

            mainWindowViewModel.Username = "TestUsername";
            mainWindowViewModel.Password = "TestPassword";
            mainWindowViewModel.FirstName = "TestFirstName";
            mainWindowViewModel.LastName = "TestLastName";
            
            mainWindowViewModel.SubmitAccountCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(string.Empty, mainWindowViewModel.ErrorMessage);

        }

        [TestMethod]
        public void TestCommandUnsuccessfulSubmit()
        {
            var mockUserManager = new Mock<UserManager>();
            MainWindowViewModel mainWindowViewModel = new(mockUserManager.Object);
            var testScheduler = new TestScheduler();

            mainWindowViewModel.SubmitAccountCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual("Username is taken.", mainWindowViewModel.ErrorMessage);
        }
    }
}