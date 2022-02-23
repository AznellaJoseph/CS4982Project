using System;
using System.Collections.Generic;
using System.Text;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using CapstoneWeb.Pages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CapstoneTest.WebTests.Pages
{
    [TestClass]
    public class TestTripOverview
    {
        [TestMethod]
        public void GetSuccess()
        {
            var outBytes = Encoding.UTF8.GetBytes("1");
            var session = new Mock<ISession>();
            session.SetupGet(s => s.Keys).Returns(new List<string> { "userId" });
            session.Setup(s => s.TryGetValue("userId", out outBytes)).Returns(true);

            var fakeTripManager = new Mock<TripManager>();
            var startDate = DateTime.Now;
            var endDate = DateTime.Now;
            fakeTripManager.Setup(mngr => mngr.GetTripByTripId(1)).Returns(new Response<Trip>
            {
                StatusCode = (uint)Ui.StatusCode.Success,
                Data = new Trip
                {
                    UserId = 1,
                    Name = "test",
                    Notes = "notes",
                    StartDate = startDate,
                    EndDate = endDate
                }
            });

            var page = TestPageBuilder.BuildPage<TripOverviewModel>(session.Object);
            page.FakeTripManager = fakeTripManager.Object;
            page.HttpContext.Request.QueryString = new QueryString("?id=1");

            page.OnGet();
            var resultTrip = page.CurrentTrip;

            Assert.AreEqual(1, page.UserId);
            Assert.IsNotNull(resultTrip);
            Assert.AreEqual(1, resultTrip.UserId);
            Assert.AreEqual("test", resultTrip.Name);
            Assert.AreEqual("notes", resultTrip.Notes);
            Assert.AreEqual(startDate, resultTrip.StartDate);
            Assert.AreEqual(endDate, resultTrip.EndDate);
        }

        [TestMethod]
        public void GetFailure_WrongUser_RedirectToIndex()
        {
            var outBytes = Encoding.UTF8.GetBytes("50");
            var session = new Mock<ISession>();
            session.SetupGet(s => s.Keys).Returns(new List<string> { "userId" });
            session.Setup(s => s.TryGetValue("userId", out outBytes)).Returns(true);

            var fakeTripManager = new Mock<TripManager>();
            var startDate = DateTime.Now;
            var endDate = DateTime.Now;
            fakeTripManager.Setup(mngr => mngr.GetTripByTripId(1)).Returns(new Response<Trip>
            {
                StatusCode = (uint)Ui.StatusCode.Success,
                Data = new Trip
                {
                    UserId = 1,
                    Name = "test",
                    Notes = "notes",
                    StartDate = startDate,
                    EndDate = endDate
                }
            });

            var page = TestPageBuilder.BuildPage<TripOverviewModel>(session.Object);
            page.FakeTripManager = fakeTripManager.Object;
            page.HttpContext.Request.QueryString = new QueryString("?id=1");

            var result = page.OnGet();
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));

            var redirect = (RedirectToPageResult)result;
            Assert.AreEqual("Index", redirect.PageName);
        }

        [TestMethod]
        public void GetFailure_InvalidTripId_RedirectToIndex()
        {
            var outBytes = Encoding.UTF8.GetBytes("1");
            var session = new Mock<ISession>();
            session.SetupGet(s => s.Keys).Returns(new List<string> { "userId" });
            session.Setup(s => s.TryGetValue("userId", out outBytes)).Returns(true);

            var fakeTripManager = new Mock<TripManager>();

            var page = TestPageBuilder.BuildPage<TripOverviewModel>(session.Object);
            page.FakeTripManager = fakeTripManager.Object;
            page.HttpContext.Request.QueryString = new QueryString("?id=NotANumber");

            var result = page.OnGet();
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));

            var redirect = (RedirectToPageResult)result;
            Assert.AreEqual("Index", redirect.PageName);
        }

        [TestMethod]
        public void GetFailure_NonExistingTrip_RedirectToIndex()
        {
            var outBytes = Encoding.UTF8.GetBytes("1");
            var session = new Mock<ISession>();
            session.SetupGet(s => s.Keys).Returns(new List<string> { "userId" });
            session.Setup(s => s.TryGetValue("userId", out outBytes)).Returns(true);

            var fakeTripManager = new Mock<TripManager>();
            fakeTripManager.Setup(mngr => mngr.GetTripByTripId(8)).Returns(new Response<Trip>
            {
                StatusCode = (uint)Ui.StatusCode.DataNotFound
            });
            var page = TestPageBuilder.BuildPage<TripOverviewModel>(session.Object);
            page.FakeTripManager = fakeTripManager.Object;
            page.HttpContext.Request.QueryString = new QueryString("?id=8");
            page.HttpContext.Session.SetString("userId", "1");

            var result = page.OnGet();
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));

            var redirect = (RedirectToPageResult)result;
            Assert.AreEqual("Index", redirect.PageName);
        }
    }
}