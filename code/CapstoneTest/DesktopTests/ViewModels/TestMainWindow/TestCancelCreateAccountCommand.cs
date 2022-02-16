using System;
using CapstoneBackend.Model;
using CapstoneDesktop.ViewModels;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CapstoneTest.DesktopTests.ViewModels.TestMainWindow
{
    [TestClass]
    public class TestCancelCreateAccountCommand
    {
        [TestMethod]
        public void CancelCreateAccount_CorrectOutput()
        {
            var mockUserManager = new Mock<UserManager>();
            MainWindowViewModel mainWindowViewModel = new(mockUserManager.Object);
            var testScheduler = new TestScheduler();

            Assert.AreEqual(true, mainWindowViewModel.LoginControlsVisible);

            mainWindowViewModel.CancelCreateAccountCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(true, mainWindowViewModel.LoginControlsVisible);
        }
    }
}