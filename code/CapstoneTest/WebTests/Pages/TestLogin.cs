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
    public class TestLogin
    {
        [TestMethod]
        public void Post_Success_RedirectsToIndex()
        {
            var session = new Mock<ISession>();
            var fakeUserManager = new Mock<UserManager>();
            fakeUserManager.Setup(um => um.GetUserByCredentials("admin", "admin"))
                .Returns(new Response<User> {Data = new User {UserId = 0}});
            var page = TestPageBuilder.BuildPage<LoginModel>(session.Object);
            page.FakeUserManager = fakeUserManager.Object;
            page.Username = "admin";
            page.Password = "admin";
            var result = page.OnPostLogin();
            var outBytes = Encoding.UTF8.GetBytes("0");
            session.Verify(s => s.Set("userId", outBytes));
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirect = (RedirectToPageResult) result;
            Assert.AreEqual("Index", redirect.PageName);
        }

        [TestMethod]
        public void Post_InternalServerError_ReturnsErrorMessage()
        {
            var session = new Mock<ISession>();
            var fakeUserManager = new Mock<UserManager>();
            fakeUserManager.Setup(um => um.GetUserByCredentials("admin", "admin"))
                .Returns(new Response<User> {ErrorMessage = Ui.ErrorMessages.InternalServerError});
            var page = TestPageBuilder.BuildPage<LoginModel>(session.Object);
            page.FakeUserManager = fakeUserManager.Object;
            page.Username = "admin";
            page.Password = "admin";
            var result = page.OnPostLogin();
            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.AreEqual(Ui.ErrorMessages.InternalServerError, page.ErrorMessage);
        }

        [TestMethod]
        public void PostCreateAccount_Success_RedirectsToCreateAccount()
        {
            var session = new Mock<ISession>();

            var page = TestPageBuilder.BuildPage<LoginModel>(session.Object);

            var result = page.OnPostCreateAccount();

            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirect = (RedirectToPageResult) result;
            Assert.AreEqual("CreateAccount", redirect.PageName);
        }
    }
}