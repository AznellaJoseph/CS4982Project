using System.Text;
using CapstoneBackend.Model;
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
        public void PostSuccess()
        {
            var session = new Mock<ISession>();
            var fakeUserManager = new Mock<UserManager>();
            fakeUserManager.Setup(um => um.GetUserByCredentials("admin", "admin"))
                .Returns(new Response<User> {Data = new User {UserId = 0}});
            var page = TestPageBuilder.BuildPage<LoginModel>(session.Object);
            page.FakeUserManager = fakeUserManager.Object;
            page.Username = "admin";
            page.Password = "admin";
            var result = page.OnPost();
            var outBytes = Encoding.UTF8.GetBytes("0");
            session.Verify(s => s.Set("userId", outBytes));
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirect = (RedirectToPageResult) result;
            Assert.AreEqual("Index", redirect.PageName);
        }

        [TestMethod]
        public void PostFailure()
        {
            var session = new Mock<ISession>();
            var fakeUserManager = new Mock<UserManager>();
            fakeUserManager.Setup(um => um.GetUserByCredentials("admin", "admin"))
                .Returns(new Response<User> {ErrorMessage = "Failed."});
            var page = TestPageBuilder.BuildPage<LoginModel>(session.Object);
            page.FakeUserManager = fakeUserManager.Object;
            page.Username = "admin";
            page.Password = "admin";
            var result = page.OnPost();
            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.AreEqual("Failed.", page.ErrorMessage);
        }

        [TestMethod]
        public void PostCreateAccountSuccess()
        {
            var session = new Mock<ISession>();

            var page = TestPageBuilder.BuildPage<LoginModel>(session.Object);

            var result = page.OnPostCreateAccount();

            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirect = (RedirectToPageResult) result;
            Assert.AreEqual("createaccount", redirect.PageName);
        }
    }
}