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
    public class TestTransportation
    {
        [TestMethod]
        public void Get_UserIdNotFound_RedirectToIndex()
        {
            var session = new Mock<ISession>();
            session.SetupGet(s => s.Keys).Returns(new List<string>());

            var page = TestPageBuilder.BuildPage<TransportationModel>(session.Object);
            var result = page.OnGet(1,8);

            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirect = (RedirectToPageResult)result;
            Assert.AreEqual("Index", redirect.PageName);
        }

        [TestMethod]
        public void Get_NonExistingTransportation_RedirectsToTrip()
        {
            var outBytes = Encoding.UTF8.GetBytes("50");
            var session = new Mock<ISession>();
            session.SetupGet(s => s.Keys).Returns(new List<string> { "userId" });
            session.Setup(s => s.TryGetValue("userId", out outBytes)).Returns(true);

            var mockTransportationManager = new Mock<TransportationManager>();
            mockTransportationManager.Setup(tm => tm.GetTransportationById(3))
                .Returns(new Response<Transportation> {Data = null});

            var page = TestPageBuilder.BuildPage<TransportationModel>(session.Object);
            page.TransportationManager = mockTransportationManager.Object;

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

            var mockTransportationManager = new Mock<TransportationManager>();
            mockTransportationManager.Setup(tm => tm.GetTransportationById(5))
                .Returns(new Response<Transportation> { Data = new Transportation{TripId = 3}});

            var page = TestPageBuilder.BuildPage<TransportationModel>(session.Object);
            page.TransportationManager = mockTransportationManager.Object;

            var result = page.OnGet(5, 8);

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


            var mockTransportationManager = new Mock<TransportationManager>();
            mockTransportationManager.Setup(tm => tm.GetTransportationById(3))
                .Returns(new Response<Transportation> { Data = new Transportation{TripId = 8} });

            var page = TestPageBuilder.BuildPage<TransportationModel>(session.Object);
            page.TransportationManager = mockTransportationManager.Object;

            var result = page.OnGet(3, 8);

            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.IsNotNull(page.CurrentTransportation);
        }

        [TestMethod]
        public void PostBack_RedirectsToTrip()
        {
            var session = new Mock<ISession>();
            session.SetupGet(s => s.Keys).Returns(new List<string> { "userId" });
            var page = TestPageBuilder.BuildPage<TransportationModel>(session.Object);

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
            var page = TestPageBuilder.BuildPage<TransportationModel>(session.Object);

            var result = page.OnPostLogout();
            session.Verify(s => s.Remove("userId"));
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));

            var redirect = (RedirectToPageResult)result;
            Assert.AreEqual("Index", redirect.PageName);
        }

    }
}
