using System;
using System.Collections.Generic;
using System.Text;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using CapstoneWeb.Pages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CapstoneTest.WebTests.Pages
{
    [TestClass]
    public class TestTrip
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

            var page = TestPageBuilder.BuildPage<TripModel>(session.Object);
            page.FakeTripManager = fakeTripManager.Object;
            var result = page.OnGet(1);
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

            var page = TestPageBuilder.BuildPage<TripModel>(session.Object);
            page.FakeTripManager = fakeTripManager.Object;

            var result = page.OnGet(1);
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
                StatusCode = 404U
            });
            var page = TestPageBuilder.BuildPage<TripModel>(session.Object);
            page.FakeTripManager = fakeTripManager.Object;

            var result = page.OnGet(8);
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));

            var redirect = (RedirectToPageResult)result;
            Assert.AreEqual("Index", redirect.PageName);
        }
        
        [TestMethod]
        public void GetFailure_UserIdNotFound_RedirectToIndex()
        {
            var session = new Mock<ISession>();
            session.SetupGet(s => s.Keys).Returns(new List<string> {  });

            var page = TestPageBuilder.BuildPage<TripModel>(session.Object);
            var result = page.OnGet(8);
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirect = (RedirectToPageResult) result;
            Assert.AreEqual("Index", redirect.PageName);
        }
        
        [TestMethod]
        public void GetAjax()
        {
            var session = new Mock<ISession>();
            var page = TestPageBuilder.BuildPage<TripModel>(session.Object);
            var selectedDate = DateTime.Now;
            var mockWaypointManager = new Mock<WaypointManager>();
            mockWaypointManager.Setup(wm => wm.GetWaypointsOnDate(1, selectedDate)).Returns(new Response<IList<Waypoint>>{Data = new List<Waypoint>()});
            page.FakeWaypointManager = mockWaypointManager.Object;
            var result = page.OnGetAjax(1,selectedDate.ToShortDateString());
            Assert.IsInstanceOfType(result, typeof(JsonResult));
        }
        
        [TestMethod]
        public void PostCreateWaypoint()
        {
            var session = new Mock<ISession>();
            session.SetupGet(s => s.Keys).Returns(new List<string> {"userId"});
            var page = TestPageBuilder.BuildPage<TripModel>(session.Object);
            var result = page.OnPostCreateWaypoint(1);
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirect = (RedirectToPageResult) result;
            Assert.IsTrue(redirect.RouteValues.ContainsKey("tripId"));
            Assert.AreEqual(1, redirect.RouteValues["tripId"]);
            Assert.AreEqual("CreateWaypoint", redirect.PageName);
        }
        
        [TestMethod]
        public void PostCreateTransportation()
        {
            var session = new Mock<ISession>();
            session.SetupGet(s => s.Keys).Returns(new List<string> {"userId"});
            var page = TestPageBuilder.BuildPage<TripModel>(session.Object);
            var result = page.OnPostCreateTransportation(1);
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirect = (RedirectToPageResult) result;
            Assert.IsTrue(redirect.RouteValues.ContainsKey("tripId"));
            Assert.AreEqual(1, redirect.RouteValues["tripId"]);
            Assert.AreEqual("CreateTransportation", redirect.PageName);
        }
        
        [TestMethod]
        public void PostBack()
        {
            var session = new Mock<ISession>();
            session.SetupGet(s => s.Keys).Returns(new List<string> {"userId"});
            var page = TestPageBuilder.BuildPage<TripModel>(session.Object);
            var result = page.OnPostBack();
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirect = (RedirectToPageResult) result;
            Assert.AreEqual("Index", redirect.PageName);
        }
        
        [TestMethod]
        public void PostLogout()
        {
            var session = new Mock<ISession>();
            session.SetupGet(s => s.Keys).Returns(new List<string> {"userId"});
            var page = TestPageBuilder.BuildPage<TripModel>(session.Object);
            var result = page.OnPostLogout();
            session.Verify(s => s.Remove("userId"));
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirect = (RedirectToPageResult) result;
            Assert.AreEqual("Index", redirect.PageName);
        }
    }
}