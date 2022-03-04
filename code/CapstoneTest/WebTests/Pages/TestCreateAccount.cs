using System.Text;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using CapstoneWeb.Pages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CapstoneTest.WebTests.Pages
{
    [TestClass]
    public class TestCreateAccount
    {
        [TestMethod]
        public void Post_Success_RedirectsToIndex()
        {
            var session = new Mock<ISession>();
            var fakeUserManager = new Mock<UserManager>();
            fakeUserManager.Setup(um => um.RegisterUser("admin", "admin", "admin", "admin"))
                .Returns(new Response<int> { Data = 0 });
            var page = TestPageBuilder.BuildPage<CreateAccountModel>(session.Object);
            page.FakeUserManager = fakeUserManager.Object;
            page.Username = "admin";
            page.Password = "admin";
            page.ConfirmedPassword = "admin";
            page.FirstName = "admin";
            page.LastName = "admin";
            var result = page.OnPost();
            var outBytes = Encoding.UTF8.GetBytes("0");
            session.Verify(s => s.Set("userId", outBytes));
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirect = (RedirectToPageResult)result;
            Assert.AreEqual("Index", redirect.PageName);
        }

        [TestMethod]
        public void Post_InternalServerError_ReturnsErrorMessage()
        {
            var session = new Mock<ISession>();
            var fakeUserManager = new Mock<UserManager>();
            fakeUserManager.Setup(um => um.RegisterUser("admin", "admin", "admin", "admin"))
                .Returns(new Response<int> { StatusCode = (uint)Ui.StatusCode.DataNotFound, ErrorMessage = Ui.ErrorMessages.InternalServerError });
            var page = TestPageBuilder.BuildPage<CreateAccountModel>(session.Object);
            page.FakeUserManager = fakeUserManager.Object;
            page.Username = "admin";
            page.Password = "admin";
            page.ConfirmedPassword = "admin";
            page.FirstName = "admin";
            page.LastName = "admin";
            var result = page.OnPost();
            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.AreEqual(Ui.ErrorMessages.InternalServerError, page.ErrorMessage);
        }

        [TestMethod]
        public void Post_MismatchPasswords_ReturnsErrorMessage()
        {
            var session = new Mock<ISession>();
            var fakeUserManager = new Mock<UserManager>();
            fakeUserManager.Setup(um => um.RegisterUser("admin", "admin", "admin", "admin"))
                .Returns(new Response<int> { StatusCode = (uint)Ui.StatusCode.DataNotFound, ErrorMessage = Ui.ErrorMessages.InternalServerError });
            var page = TestPageBuilder.BuildPage<CreateAccountModel>(session.Object);
            page.FakeUserManager = fakeUserManager.Object;
            page.Username = "admin";
            page.Password = "admin";
            page.ConfirmedPassword = "test";
            page.FirstName = "admin";
            page.LastName = "admin";
            var result = page.OnPost();
            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.AreEqual(Ui.ErrorMessages.PasswordsDoNotMatch, page.ErrorMessage);
        }

        [TestMethod]
        public void Post_NullUsername_ReturnsErrorMessage()
        {
            var session = new Mock<ISession>();
            var page = TestPageBuilder.BuildPage<CreateAccountModel>(session.Object);
            page.Password = "admin";
            page.ConfirmedPassword = "test";
            page.FirstName = "admin";
            page.LastName = "admin";
            var result = page.OnPost();
            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.AreEqual(Ui.ErrorMessages.InvalidUsername, page.ErrorMessage);
        }

        [TestMethod]
        public void Post_NullPassword_ReturnsErrorMessage()
        {
            var session = new Mock<ISession>();
            var page = TestPageBuilder.BuildPage<CreateAccountModel>(session.Object);
            page.Username = "admin";
            page.ConfirmedPassword = "test";
            page.FirstName = "admin";
            page.LastName = "admin";
            var result = page.OnPost();
            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.AreEqual(Ui.ErrorMessages.InvalidPassword, page.ErrorMessage);
        }

        [TestMethod]
        public void Post_NullConfirmedPassword_ReturnsErrorMessage()
        {
            var session = new Mock<ISession>();
            var page = TestPageBuilder.BuildPage<CreateAccountModel>(session.Object);
            page.Username = "admin";
            page.Password = "test";
            page.FirstName = "admin";
            page.LastName = "admin";
            var result = page.OnPost();
            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.AreEqual(Ui.ErrorMessages.InvalidConfirmedPassword, page.ErrorMessage);
        }

        [TestMethod]
        public void Post_NullFirstName_ReturnsErrorMessage()
        {
            var session = new Mock<ISession>();
            var page = TestPageBuilder.BuildPage<CreateAccountModel>(session.Object);
            page.Password = "admin";
            page.ConfirmedPassword = "test";
            page.Username = "admin";
            page.LastName = "admin";
            var result = page.OnPost();
            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.AreEqual(Ui.ErrorMessages.InvalidFirstName, page.ErrorMessage);
        }

        [TestMethod]
        public void Post_NullLastName_ReturnsErrorMessage()
        {
            var session = new Mock<ISession>();
            var page = TestPageBuilder.BuildPage<CreateAccountModel>(session.Object);
            page.Password = "admin";
            page.ConfirmedPassword = "test";
            page.FirstName = "admin";
            page.Username = "admin";
            var result = page.OnPost();
            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.AreEqual(Ui.ErrorMessages.InvalidLastName, page.ErrorMessage);
        }

        [TestMethod]
        public void PostCancel_Redirects()
        {
            var session = new Mock<ISession>();
            var fakeUserManager = new Mock<UserManager>();
            fakeUserManager.Setup(um => um.RegisterUser("admin", "admin", "admin", "admin"))
                .Returns(new Response<int> { Data = 0 });
            var page = TestPageBuilder.BuildPage<CreateAccountModel>(session.Object);
            page.FakeUserManager = fakeUserManager.Object;
            page.Username = "admin";
            page.Password = "admin";
            page.ConfirmedPassword = "admin";
            page.FirstName = "admin";
            page.LastName = "admin";
            var result = page.OnPostCancel();
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirect = (RedirectToPageResult)result;
            Assert.AreEqual("login", redirect.PageName);
        }
    }
}