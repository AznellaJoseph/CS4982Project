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
    public class TestEditWaypoint
    {
        [TestMethod]
        public void Get_Success_ReturnsPageResult()
        {
            var outBytes = Encoding.UTF8.GetBytes("50");
            var session = new Mock<ISession>();
            session.SetupGet(s => s.Keys).Returns(new List<string> { "userId" });
            session.Setup(s => s.TryGetValue("userId", out outBytes)).Returns(true);

            var fakeWaypoint = new Waypoint
            {
                TripId = 0,
                WaypointId = 0,
                Location = "TestLocation",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                Notes = "TestNotes"
            };


            var mockWaypointManager = new Mock<WaypointManager>();
            mockWaypointManager.Setup(tm => tm.GetWaypointById(It.IsAny<int>()))
                .Returns(new Response<Waypoint> { Data = fakeWaypoint });

            var page = TestPageBuilder.BuildPage<EditWaypointModel>(session.Object);
            page.WaypointManager = mockWaypointManager.Object;

            var result = page.OnGet(0, 0);

            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.AreEqual(fakeWaypoint.Location, page.Location);
            Assert.AreEqual(fakeWaypoint.StartDate, page.StartDate);
            Assert.AreEqual(fakeWaypoint.EndDate, page.EndDate);
            Assert.AreEqual(fakeWaypoint.Notes, page.Notes);
        }

        [TestMethod]
        public void Get_UserIdNotFound_RedirectToIndex()
        {
            var session = new Mock<ISession>();
            session.SetupGet(s => s.Keys).Returns(new List<string>());

            var page = TestPageBuilder.BuildPage<EditWaypointModel>(session.Object);
            var result = page.OnGet(1, 8);

            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirect = (RedirectToPageResult)result;
            Assert.AreEqual("Index", redirect.PageName);
        }

        [TestMethod]
        public void Get_NonExistingWaypoint_RedirectsToTrip()
        {
            var outBytes = Encoding.UTF8.GetBytes("50");
            var session = new Mock<ISession>();
            session.SetupGet(s => s.Keys).Returns(new List<string> { "userId" });
            session.Setup(s => s.TryGetValue("userId", out outBytes)).Returns(true);

            var mockWaypointManager = new Mock<WaypointManager>();
            mockWaypointManager.Setup(tm => tm.GetWaypointById(3))
                .Returns(new Response<Waypoint> { Data = null });

            var page = TestPageBuilder.BuildPage<EditWaypointModel>(session.Object);
            page.WaypointManager = mockWaypointManager.Object;

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

            var mockWaypointManager = new Mock<WaypointManager>();
            mockWaypointManager.Setup(tm => tm.GetWaypointById(3))
                .Returns(new Response<Waypoint> { Data = new Waypoint { TripId = 2 } });

            var page = TestPageBuilder.BuildPage<EditWaypointModel>(session.Object);
            page.WaypointManager = mockWaypointManager.Object;

            var result = page.OnGet(3, 8);

            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirect = (RedirectToPageResult)result;
            Assert.AreEqual("Trip", redirect.PageName);
        }


        [TestMethod]
        public void Post_Success_RedirectsToTrip()
        {
            var session = new Mock<ISession>();
            var fakeWaypointManager = new Mock<WaypointManager>();
            var currentTime = DateTime.Now;
            
            fakeWaypointManager.Setup(wm => wm.EditWaypoint(It.IsAny<Waypoint>()))
                .Returns(new Response<bool> {Data = true});
            var page = TestPageBuilder.BuildPage<EditWaypointModel>(session.Object);
            var fakeValidationManager = new Mock<ValidationManager>();
            fakeValidationManager.Setup(vm => vm.DetermineIfValidEventDates(0, currentTime, currentTime))
                .Returns(new Response<bool> {Data = true});
            fakeValidationManager.Setup(vm => vm.FindClashingEvents(0, currentTime, currentTime, null))
                .Returns(new Response<IList<IEvent>> { Data = null});
            fakeValidationManager.Setup(vm => vm.DetermineIfValidLocation("1601 Maple St"))
                .Returns(new Response<bool> { Data = true });

            page.WaypointManager = fakeWaypointManager.Object;
            page.ValidationManager = fakeValidationManager.Object;

            page.Location = "1601 Maple St";
            page.Notes = "notes";
            page.StartDate = currentTime;
            page.EndDate = currentTime;
            var result = page.OnPost(0, 0);

            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirect = (RedirectToPageResult) result;
            Assert.AreEqual("Trip", redirect.PageName);
        }

        [TestMethod]
        public void Post_InvalidStartDate_ReturnsErrorMessage()
        {
            var session = new Mock<ISession>();
            var fakeWaypointManager = new Mock<WaypointManager>();
            var currentTime = DateTime.Now;
            fakeWaypointManager.Setup(um =>
                    um.EditWaypoint(It.IsAny<Waypoint>()))
                .Returns(new Response<bool>
                    {StatusCode = (uint) Ui.StatusCode.BadRequest, ErrorMessage = Ui.ErrorMessages.InvalidStartDate});
            var fakeValidationManager = new Mock<ValidationManager>();
            fakeValidationManager.Setup(vm => vm.DetermineIfValidEventDates(0, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(new Response<bool> {Data = true});
            fakeValidationManager.Setup(vm => vm.FindClashingEvents(0, It.IsAny<DateTime>(), It.IsAny<DateTime>(), null))
                .Returns(new Response<IList<IEvent>> { Data = null});
            fakeValidationManager.Setup(vm => vm.DetermineIfValidLocation(It.IsAny<string>()))
                .Returns(new Response<bool> { Data = true });

            var page = TestPageBuilder.BuildPage<EditWaypointModel>(session.Object);
            page.WaypointManager = fakeWaypointManager.Object;
            page.ValidationManager = fakeValidationManager.Object;

            page.Location = "1601 Maple St";
            page.Notes = "notes";
            page.StartDate = currentTime.AddDays(1);
            page.EndDate = currentTime;

            var result = page.OnPost(0, 0);
            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.AreEqual(Ui.ErrorMessages.InvalidStartDate, page.ErrorMessage);
        }

        [TestMethod]
        public void Post_InvalidEventDates_ReturnsErrorMessage()
        {
            var session = new Mock<ISession>();
            var currentTime = DateTime.Now;

            var fakeValidationManager = new Mock<ValidationManager>();
            fakeValidationManager.Setup(vm => vm.DetermineIfValidEventDates(0, currentTime, currentTime.AddDays(2)))
                .Returns(new Response<bool> { ErrorMessage = $"{Ui.ErrorMessages.EventStartDateBeforeTripStartDate} {DateTime.Now.AddDays(1)}" });
            fakeValidationManager.Setup(vm => vm.DetermineIfValidLocation("1601 Maple St"))
                .Returns(new Response<bool> { Data = true });

            var page = TestPageBuilder.BuildPage<EditWaypointModel>(session.Object);
            page.ValidationManager = fakeValidationManager.Object;
            page.Location = "1601 Maple St";
            page.StartDate = currentTime;
            page.EndDate = currentTime.AddDays(2);

            var result = page.OnPost(0, 0);

            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.AreEqual($"{Ui.ErrorMessages.EventStartDateBeforeTripStartDate} {DateTime.Now.AddDays(1)}",
                page.ErrorMessage);
        }

        [TestMethod]
        public void Post_ClashingEvent_ReturnsErrorMessage()
        {
            var session = new Mock<ISession>();
            var currentTime = DateTime.Now;

            var fakeValidationManager = new Mock<ValidationManager>();
            fakeValidationManager.Setup(vm => vm.DetermineIfValidLocation("Hilton"))
                .Returns(new Response<bool> { Data = true });
            fakeValidationManager.Setup(vm => vm.DetermineIfValidEventDates(0, currentTime, currentTime.AddDays(2)))
                .Returns(new Response<bool> {Data = true});
            fakeValidationManager.Setup(vm => vm.FindClashingEvents(0, currentTime, currentTime.AddDays(2), null))
                .Returns(new Response<IList<IEvent>>
                {
                    Data = new List<IEvent>{ new Waypoint() { WaypointId = 1 }},
                    ErrorMessage = $"{Ui.ErrorMessages.ClashingEventDates} {currentTime} {currentTime.AddDays(1)}"
                });

            var page = TestPageBuilder.BuildPage<EditWaypointModel>(session.Object);
            page.ValidationManager = fakeValidationManager.Object;
            page.Location = "Hilton";
            page.StartDate = currentTime;
            page.EndDate = currentTime.AddDays(2);

            var result = page.OnPost(0, 0);

            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.IsFalse(string.IsNullOrEmpty(page.ErrorMessage));
        }

        [TestMethod]
        public void Post_InvalidLocation_ReturnsErrorMessage()
        {
            var session = new Mock<ISession>();
            var currentTime = DateTime.Now;

            var fakeValidationManager = new Mock<ValidationManager>();
            fakeValidationManager.Setup(vm => vm.DetermineIfValidEventDates(0, currentTime, currentTime))
                .Returns(new Response<bool> { Data = true });
            fakeValidationManager.Setup(vm => vm.FindClashingEvents(0, currentTime, currentTime, null))
                .Returns(new Response<IList<IEvent>> { Data = null });
            fakeValidationManager.Setup(vm => vm.DetermineIfValidLocation("Hilton"))
                .Returns(new Response<bool> { Data = false, ErrorMessage = Ui.ErrorMessages.InvalidLocation });

            var page = TestPageBuilder.BuildPage<EditWaypointModel>(session.Object);
            page.ValidationManager = fakeValidationManager.Object;
            page.Location = "Hilton";
            page.StartDate = currentTime;
            page.EndDate = currentTime;

            var result = page.OnPost(0, 0);

            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.AreEqual(Ui.ErrorMessages.InvalidLocation, page.ErrorMessage);
        }

        [TestMethod]
        public void PostCancel_Success_RedirectToTrip()
        {
            var session = new Mock<ISession>();
            var page = TestPageBuilder.BuildPage<EditWaypointModel>(session.Object);

            var result = page.OnPostCancel(1);

            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirect = (RedirectToPageResult) result;
            Assert.AreEqual("Trip", redirect.PageName);
            Assert.AreEqual(1, redirect.RouteValues["tripId"]);
        }
    }
}