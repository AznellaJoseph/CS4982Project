using System;
using CapstoneBackend.Model;
using CapstoneDesktop.ViewModels;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CapstoneTest.DesktopTests.ViewModels.TestMainWindow
{
    [TestClass]
    public class TestOpenCreateAccountCommand
    {
        [TestMethod]
        public void OpenCreateAccount_CorrectOutput()
        {
            var mockUserManager = new Mock<UserManager>();
            MainWindowViewModel mainWindowViewModel = new(mockUserManager.Object);
            var testScheduler = new TestScheduler();

            Assert.AreEqual(true, mainWindowViewModel.LoginControlsVisible);

            mainWindowViewModel.OpenCreateAccountCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(false, mainWindowViewModel.LoginControlsVisible);
        }
    }
}