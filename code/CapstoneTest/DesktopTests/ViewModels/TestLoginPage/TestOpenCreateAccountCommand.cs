using System;
using CapstoneBackend.Model;
using CapstoneDesktop.ViewModels;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReactiveUI;

namespace CapstoneTest.DesktopTests.ViewModels.TestLoginPage
{
    [TestClass]
    public class TestOpenCreateAccountCommand
    {
        [TestMethod]
        public void OpenCreateAccount_CorrectOutput()
        {
            var mockUserManager = new Mock<UserManager>();
            var mockScreen = new Mock<IScreen>();
            LoginPageViewModel mainWindowViewModel = new(mockUserManager.Object, mockScreen.Object);
            var testScheduler = new TestScheduler();

            mainWindowViewModel.OpenCreateAccountCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(new CreateAccountPageViewModel(mockScreen.Object), mainWindowViewModel.HostScreen.Router.CurrentViewModel);
        }
    }
}