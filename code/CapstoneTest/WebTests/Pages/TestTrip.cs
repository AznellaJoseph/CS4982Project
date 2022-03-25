using System;
using System.Collections.Generic;
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
    public class TestTrip
    {
        [TestMethod]
        public void Get_Success_ReturnsPageResult()
        {
            var outBytes = Encoding.UTF8.GetBytes("1");
            var session = new Mock<ISession>();
            session.SetupGet(s => s.Keys).Returns(new List<string> {"userId"});
            session.Setup(s => s.TryGetValue("userId", out outBytes)).Returns(true);

            var fakeTripManager = new Mock<TripManager>();
            var startDate = DateTime.Now;
            var endDate = DateTime.Now;
            fakeTripManager.Setup(mngr => mngr.GetTripByTripId(1)).Returns(new Response<Trip>
            {
                StatusCode = (uint) Ui.StatusCode.Success,
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
            page.TripManager = fakeTripManager.Object;
            var result = page.OnGet(1);
            var resultTrip = page.CurrentTrip;

            Assert.AreEqual(1, page.UserId);
            Assert.AreEqual(1, resultTrip.UserId);
            Assert.AreEqual("test", resultTrip.Name);
            Assert.AreEqual("notes", resultTrip.Notes);
            Assert.AreEqual(startDate, resultTrip.StartDate);
            Assert.AreEqual(endDate, resultTrip.EndDate);
            Assert.AreEqual(0, page.Lodgings.Count);
            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.IsNotNull(resultTrip);
        }

        [TestMethod]
        public void Get_WrongUser_RedirectToIndex()
        {
            var outBytes = Encoding.UTF8.GetBytes("50");
            var session = new Mock<ISession>();
            session.SetupGet(s => s.Keys).Returns(new List<string> {"userId"});
            session.Setup(s => s.TryGetValue("userId", out outBytes)).Returns(true);

            var fakeTripManager = new Mock<TripManager>();
            var startDate = DateTime.Now;
            var endDate = DateTime.Now;
            fakeTripManager.Setup(tm => tm.GetTripByTripId(1)).Returns(new Response<Trip>
            {
                StatusCode = (uint) Ui.StatusCode.Success,
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
            page.TripManager = fakeTripManager.Object;

            var result = page.OnGet(1);
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));

            var redirect = (RedirectToPageResult) result;
            Assert.AreEqual("Index", redirect.PageName);
        }

        [TestMethod]
        public void Get_NonExistingTrip_RedirectToIndex()
        {
            var outBytes = Encoding.UTF8.GetBytes("1");
            var session = new Mock<ISession>();
            session.SetupGet(s => s.Keys).Returns(new List<string> {"userId"});
            session.Setup(s => s.TryGetValue("userId", out outBytes)).Returns(true);

            var fakeTripManager = new Mock<TripManager>();
            fakeTripManager.Setup(tm => tm.GetTripByTripId(8)).Returns(new Response<Trip>
            {
                StatusCode = (uint) Ui.StatusCode.DataNotFound
            });
            var page = TestPageBuilder.BuildPage<TripModel>(session.Object);
            page.TripManager = fakeTripManager.Object;

            var result = page.OnGet(8);
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));

            var redirect = (RedirectToPageResult) result;
            Assert.AreEqual("Index", redirect.PageName);
        }

        [TestMethod]
        public void Get_UserIdNotFound_RedirectToIndex()
        {
            var session = new Mock<ISession>();
            session.SetupGet(s => s.Keys).Returns(new List<string>());

            var page = TestPageBuilder.BuildPage<TripModel>(session.Object);
            var result = page.OnGet(8);

            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirect = (RedirectToPageResult) result;
            Assert.AreEqual("Index", redirect.PageName);
        }

        [TestMethod]
        public void GetEvents_ReturnsJSONResult()
        {
            var session = new Mock<ISession>();
            var page = TestPageBuilder.BuildPage<TripModel>(session.Object);
            var selectedDate = DateTime.Now;
            var mockEventManager = new Mock<EventManager>();
            mockEventManager.Setup(wm => wm.GetEventsOnDate(1, selectedDate))
                .Returns(new Response<IList<IEvent>> {Data = new List<IEvent>()});

            page.EventManager = mockEventManager.Object;

            var result = page.OnGetEvents(1, selectedDate.ToShortDateString());
            Assert.IsInstanceOfType(result, typeof(JsonResult));
        }

        [TestMethod]
        public void GetRemoveLodging_RedirectsToTrip()
        {
            var session = new Mock<ISession>();
            var page = TestPageBuilder.BuildPage<TripModel>(session.Object);
            var mockLodgingManager = new Mock<LodgingManager>();
            page.LodgingManager = mockLodgingManager.Object;
            mockLodgingManager.Setup(wm => wm.RemoveLodging(1)).Returns(new Response<bool>
            {
                Data = true
            });
            var result = page.OnPostRemoveLodging(1, 1);
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirect = (RedirectToPageResult) result;
            Assert.AreEqual("Trip", redirect.PageName);
        }

        [TestMethod]
        public void GetRemoveWaypoint_ReturnsJSONResult()
        {
            var session = new Mock<ISession>();
            var page = TestPageBuilder.BuildPage<TripModel>(session.Object);
            var mockWaypointManager = new Mock<WaypointManager>();
            page.WaypointManager = mockWaypointManager.Object;
            mockWaypointManager.Setup(wm => wm.RemoveWaypoint(1)).Returns(new Response<bool>
            {
                Data = true
            });
            var result = page.OnGetRemoveWaypoint(1);
            Assert.IsInstanceOfType(result, typeof(JsonResult));
        }

        [TestMethod]
        public void GetRemoveTransportation_ReturnsJSONResult()
        {
            var session = new Mock<ISession>();
            var page = TestPageBuilder.BuildPage<TripModel>(session.Object);
            var mockTransportationManager = new Mock<TransportationManager>();
            page.TransportationManager = mockTransportationManager.Object;
            mockTransportationManager.Setup(wm => wm.RemoveTransportation(1)).Returns(new Response<bool>
            {
                Data = true
            });
            var result = page.OnGetRemoveTransportation(1);
            Assert.IsInstanceOfType(result, typeof(JsonResult));
        }

        [TestMethod]
        public void GetViewWaypoint_RedirectToWaypoint()
        {
            var session = new Mock<ISession>();
            var page = TestPageBuilder.BuildPage<TripModel>(session.Object);

            var result = page.OnGetViewWaypoint(1, 1);
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirect = (RedirectToPageResult) result;
            Assert.AreEqual("Waypoint", redirect.PageName);
        }

        [TestMethod]
        public void GetViewTransportation_RedirectToTransportation()
        {
            var session = new Mock<ISession>();
            var page = TestPageBuilder.BuildPage<TripModel>(session.Object);
            ;
            var result = page.OnGetViewTransportation(1, 1);
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirect = (RedirectToPageResult) result;
            Assert.AreEqual("Transportation", redirect.PageName);
        }

        [TestMethod]
        public void PostCreateLodging_RedirectsToCreateLodging()
        {
            var session = new Mock<ISession>();
            session.SetupGet(s => s.Keys).Returns(new List<string> {"userId"});
            var page = TestPageBuilder.BuildPage<TripModel>(session.Object);

            var result = page.OnPostCreateLodging(1);
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));

            var redirect = (RedirectToPageResult) result;

            Assert.IsTrue(redirect.RouteValues.ContainsKey("tripId"));
            Assert.AreEqual(1, redirect.RouteValues["tripId"]);
            Assert.AreEqual("CreateLodging", redirect.PageName);
        }

        [TestMethod]
        public void PostCreateWaypoint_RedirectsToCreateWaypoint()
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
        public void PostCreateTransportation_RedirectsToCreateTransportation()
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
        public void PostBack_RedirectsToIndex()
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
        public void PostLogout_RedirectsToIndexWithoutUserId()
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