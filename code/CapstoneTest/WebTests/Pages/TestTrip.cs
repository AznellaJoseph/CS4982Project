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
    public class TestTrip
    {
        [TestMethod]
        public void GetSuccess()
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
            page.FakeTripManager = fakeTripManager.Object;

            var result = page.OnGet(1);
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));

            var redirect = (RedirectToPageResult) result;
            Assert.AreEqual("Index", redirect.PageName);
        }

        [TestMethod]
        public void GetFailure_NonExistingTrip_RedirectToIndex()
        {
            var outBytes = Encoding.UTF8.GetBytes("1");
            var session = new Mock<ISession>();
            session.SetupGet(s => s.Keys).Returns(new List<string> {"userId"});
            session.Setup(s => s.TryGetValue("userId", out outBytes)).Returns(true);

            var fakeTripManager = new Mock<TripManager>();
            fakeTripManager.Setup(mngr => mngr.GetTripByTripId(8)).Returns(new Response<Trip>
            {
                StatusCode = (uint) Ui.StatusCode.DataNotFound
            });
            var page = TestPageBuilder.BuildPage<TripModel>(session.Object);
            page.FakeTripManager = fakeTripManager.Object;

            var result = page.OnGet(8);
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));

            var redirect = (RedirectToPageResult) result;
            Assert.AreEqual("Index", redirect.PageName);
        }

        [TestMethod]
        public void GetFailure_UserIdNotFound_RedirectToIndex()
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
        public void GetEvents()
        {
            var session = new Mock<ISession>();
            var page = TestPageBuilder.BuildPage<TripModel>(session.Object);
            var selectedDate = DateTime.Now;
            var mockEventManager = new Mock<EventManager>();
            mockEventManager.Setup(wm => wm.GetEventsOnDate(1, selectedDate))
                .Returns(new Response<IList<IEvent>> {Data = new List<IEvent>()});
            page.FakeEventManager = mockEventManager.Object;
            var result = page.OnGetEvents(1, selectedDate.ToShortDateString());
            Assert.IsInstanceOfType(result, typeof(JsonResult));
        }
        
        [TestMethod]
        public void GetRemoveWaypoint()
        {
            var session = new Mock<ISession>();
            var page = TestPageBuilder.BuildPage<TripModel>(session.Object);
            var mockWaypointManager = new Mock<WaypointManager>();
            page.FakeWaypointManager = mockWaypointManager.Object;
            mockWaypointManager.Setup(wm => wm.RemoveWaypoint(1)).Returns(new Response<bool>
            {
                Data = true
            });
            var result = page.OnGetRemoveWaypoint(1, 1);
            Assert.IsInstanceOfType(result, typeof(JsonResult));
        }
        
        [TestMethod]
        public void GetRemoveTransportation()
        {
            var session = new Mock<ISession>();
            var page = TestPageBuilder.BuildPage<TripModel>(session.Object);
            var mockTransportationManager = new Mock<TransportationManager>();
            page.FakeTransportationManager = mockTransportationManager.Object;
            mockTransportationManager.Setup(wm => wm.RemoveTransportation(1)).Returns(new Response<bool>
            {
                Data = true
            });
            var result = page.OnGetRemoveTransportation(1, 1);
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