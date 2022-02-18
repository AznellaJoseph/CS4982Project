using CapstoneBackend.Model;
using CapstoneWeb.Pages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapstoneTest.WebTests.Pages
{
    [TestClass]
    public class TestTripOverview
    {

        [TestMethod]
        public void GetSuccess()
        {
            var session = new Mock<ISession>();
            var fakeTripManager = new Mock<TripManager>();
            var startDate = DateTime.Now;
            var endDate = DateTime.Now;
            fakeTripManager.Setup(mngr => mngr.GetTripByTripId(1)).Returns(new Response<Trip> { 
                StatusCode = 200U,
                Data = new Trip
                {
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
            var resultTrip = page.CurrentTrip;

            Assert.AreEqual("test", resultTrip.Name);
            Assert.AreEqual("notes", resultTrip.Notes);
            Assert.AreEqual(startDate, resultTrip.StartDate);
            Assert.AreEqual(endDate, resultTrip.EndDate);
        }

        [TestMethod]
        public void GetFailure_InvalidTripId_RedirectToIndex()
        {
            var session = new Mock<ISession>();
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
            var session = new Mock<ISession>();
            var fakeTripManager = new Mock<TripManager>();
            var startDate = DateTime.Now;
            var endDate = DateTime.Now;
            fakeTripManager.Setup(mngr => mngr.GetTripByTripId(8)).Returns(new Response<Trip>
            {
                StatusCode = 404U,
            });
            var page = TestPageBuilder.BuildPage<TripOverviewModel>(session.Object);
            page.FakeTripManager = fakeTripManager.Object;
            page.HttpContext.Request.QueryString = new QueryString("?id=8");

            var result = page.OnGet();
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));

            var redirect = (RedirectToPageResult)result;
            Assert.AreEqual("Index", redirect.PageName);
        }

    }
}
