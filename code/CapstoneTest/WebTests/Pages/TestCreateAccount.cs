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
        public void PostFailure()
        {
            var session = new Mock<ISession>();
            var fakeUserManager = new Mock<UserManager>();
            fakeUserManager.Setup(um => um.RegisterUser("admin", "admin", "admin", "admin"))
                .Returns(new Response<int> { StatusCode = 404, ErrorMessage = "Failed." });
            var page = TestPageBuilder.BuildPage<CreateAccountModel>(session.Object);
            page.FakeUserManager = fakeUserManager.Object;
            page.Username = "admin";
            page.Password = "admin";
            page.FirstName = "admin";
            page.LastName = "admin";
            var result = page.OnPost();
            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.AreEqual("Failed.", page.ErrorMessage);
        }
    }
}