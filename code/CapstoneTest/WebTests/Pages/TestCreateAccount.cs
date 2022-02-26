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
        public void PostSuccess()
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
        public void PostFailure_ServerError()
        {
            var session = new Mock<ISession>();
            var fakeUserManager = new Mock<UserManager>();
            fakeUserManager.Setup(um => um.RegisterUser("admin", "admin", "admin", "admin"))
                .Returns(new Response<int> { StatusCode = 500, ErrorMessage = Ui.ErrorMessages.InternalServerError });
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
        public void PostFailure_PasswordMismatch()
        {
            var session = new Mock<ISession>();
            var fakeUserManager = new Mock<UserManager>();
            var page = TestPageBuilder.BuildPage<CreateAccountModel>(session.Object);
            page.FakeUserManager = fakeUserManager.Object;
            page.Username = "admin";
            page.Password = "admin";
            page.ConfirmedPassword = "WrongPassword";
            page.FirstName = "admin";
            page.LastName = "admin";

            var result = page.OnPost();

            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.AreEqual(Ui.ErrorMessages.PasswordsDoNotMatch, page.ErrorMessage);
        }

        [TestMethod]
        public void PostCancelSuccess()
        {
            var session = new Mock<ISession>();
            var page = TestPageBuilder.BuildPage<CreateAccountModel>(session.Object);

            var result = page.OnPostCancel();

            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirect = (RedirectToPageResult)result;
            Assert.AreEqual("login", redirect.PageName);
        }
    }
}