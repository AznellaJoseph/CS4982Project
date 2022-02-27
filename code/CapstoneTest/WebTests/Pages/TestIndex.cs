using System.Collections.Generic;
using System.Text;
using CapstoneBackend.Model;
using CapstoneWeb.Pages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CapstoneTest.WebTests.Pages
{
    [TestClass]
    public class TestIndex
    {
        [TestMethod]
        public void GetIndex_WithNoUserId()
        {
            var session = new Mock<ISession>();
            session.SetupGet(s => s.Keys).Returns(new List<string>());
            var page = TestPageBuilder.BuildPage<IndexModel>(session.Object);
            var result = page.OnGet();
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirect = (RedirectToPageResult) result;
            Assert.AreEqual("Login", redirect.PageName);
        }

        [TestMethod]
        public void GetIndex_WithUserId()
        {
            var outBytes = Encoding.UTF8.GetBytes("0");
            var session = new Mock<ISession>();
            var mockTripManager = new Mock<TripManager>();
            session.SetupGet(s => s.Keys).Returns(new List<string> {"userId"});
            session.Setup(s => s.TryGetValue("userId", out outBytes)).Returns(true);
            mockTripManager.Setup(tm => tm.GetTripsByUser(0))
                .Returns(new Response<IList<Trip>> {Data = new List<Trip> { new() }});
            var page = TestPageBuilder.BuildPage<IndexModel>(session.Object);
            page.FakeTripManager = mockTripManager.Object;
            var result = page.OnGet();
            Assert.AreEqual(1, page.Trips.Count);
            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.AreEqual(0, page.UserId);
        }

        [TestMethod]
        public void PostLogout()
        {
            var session = new Mock<ISession>();
            session.SetupGet(s => s.Keys).Returns(new List<string> {"userId"});
            var page = TestPageBuilder.BuildPage<IndexModel>(session.Object);
            var result = page.OnPostLogout();
            session.Verify(s => s.Remove("userId"));
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirect = (RedirectToPageResult) result;
            Assert.AreEqual("Index", redirect.PageName);
        }
        
        [TestMethod]
        public void PostCreate()
        {
            var session = new Mock<ISession>();
            var page = TestPageBuilder.BuildPage<IndexModel>(session.Object);
            var result = page.OnPostCreate();
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirect = (RedirectToPageResult) result;
            Assert.AreEqual("CreateTrip", redirect.PageName);
        }
    }
}