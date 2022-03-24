using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class TestLodging
    {
        [TestMethod]
        public void Get_UserIdNotFound_RedirectToIndex()
        {
            var session = new Mock<ISession>();
            session.SetupGet(s => s.Keys).Returns(new List<string>());

            var page = TestPageBuilder.BuildPage<LodgingModel>(session.Object);
            var result = page.OnGet(1, 8);

            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirect = (RedirectToPageResult)result;
            Assert.AreEqual("Index", redirect.PageName);
        }

        [TestMethod]
        public void Get_NonExistingLodging_RedirectsToTrip()
        {
            var outBytes = Encoding.UTF8.GetBytes("50");
            var session = new Mock<ISession>();
            session.SetupGet(s => s.Keys).Returns(new List<string> { "userId" });
            session.Setup(s => s.TryGetValue("userId", out outBytes)).Returns(true);

            var mockLodgingManager = new Mock<LodgingManager>();
            mockLodgingManager.Setup(tm => tm.GetLodgingById(3))
                .Returns(new Response<Lodging> { Data = null });

            var page = TestPageBuilder.BuildPage<LodgingModel>(session.Object);
            page.LodgingManager = mockLodgingManager.Object;

            var result = page.OnGet(3, 8);

            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirect = (RedirectToPageResult)result;
            Assert.AreEqual("Trip", redirect.PageName);
        }

        [TestMethod]
        public void Get_TripIdsDoNotMatch_RedirectsToTrip()
        {
            var outBytes = Encoding.UTF8.GetBytes("50");
            var session = new Mock<ISession>();
            session.SetupGet(s => s.Keys).Returns(new List<string> { "userId" });
            session.Setup(s => s.TryGetValue("userId", out outBytes)).Returns(true);

            var mockLodgingManager = new Mock<LodgingManager>();
            mockLodgingManager.Setup(tm => tm.GetLodgingById(3))
                .Returns(new Response<Lodging> { Data = new Lodging{TripId = 2} });

            var page = TestPageBuilder.BuildPage<LodgingModel>(session.Object);
            page.LodgingManager = mockLodgingManager.Object;

            var result = page.OnGet(3, 8);

            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirect = (RedirectToPageResult)result;
            Assert.AreEqual("Trip", redirect.PageName);

        }

        [TestMethod]
        public void Get_Success_ReturnsPageResult()
        {
            var outBytes = Encoding.UTF8.GetBytes("50");
            var session = new Mock<ISession>();
            session.SetupGet(s => s.Keys).Returns(new List<string> { "userId" });
            session.Setup(s => s.TryGetValue("userId", out outBytes)).Returns(true);


            var mockLodgingManager = new Mock<LodgingManager>();
            mockLodgingManager.Setup(tm => tm.GetLodgingById(3))
                .Returns(new Response<Lodging> { Data = new Lodging{TripId = 8} });

            var page = TestPageBuilder.BuildPage<LodgingModel>(session.Object);
            page.LodgingManager = mockLodgingManager.Object;

            var result = page.OnGet(3, 8);

            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.IsNotNull(page.CurrentLodging);
        }

        [TestMethod]
        public void PostBack_RedirectsToTrip()
        {
            var session = new Mock<ISession>();
            session.SetupGet(s => s.Keys).Returns(new List<string> { "userId" });
            var page = TestPageBuilder.BuildPage<LodgingModel>(session.Object);

            var result = page.OnPostBack(0);
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));

            var redirect = (RedirectToPageResult)result;
            Assert.AreEqual("Trip", redirect.PageName);
        }

        [TestMethod]
        public void PostLogout_RedirectsToIndexWithoutUserId()
        {
            var session = new Mock<ISession>();
            session.SetupGet(s => s.Keys).Returns(new List<string> { "userId" });
            var page = TestPageBuilder.BuildPage<LodgingModel>(session.Object);

            var result = page.OnPostLogout();
            session.Verify(s => s.Remove("userId"));
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));

            var redirect = (RedirectToPageResult)result;
            Assert.AreEqual("Index", redirect.PageName);
        }
    }
}
