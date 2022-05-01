using System;
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
    public class TestCreateWaypoint
    {
        [TestMethod]
        public void Post_Success_RedirectsToTrip()
        {
            var session = new Mock<ISession>();
            var fakeWaypointManager = new Mock<WaypointManager>();
            var currentTime = DateTime.Now;
            fakeWaypointManager.Setup(um => um.CreateWaypoint(0, "1601 Maple St", currentTime, currentTime, "notes"))
                .Returns(new Response<int> {Data = 0});
            var page = TestPageBuilder.BuildPage<CreateWaypointModel>(session.Object);
            var fakeValidationManager = new Mock<ValidationManager>();
            fakeValidationManager.Setup(vm => vm.DetermineIfValidEventDates(0, currentTime, currentTime))
                .Returns(new Response<bool> {Data = true});
            fakeValidationManager.Setup(vm => vm.FindClashingEvent(0, currentTime, currentTime))
                .Returns(new Response<IEvent> {Data = null});
            fakeValidationManager.Setup(vm => vm.DetermineIfValidLocation("1601 Maple St"))
                .Returns(new Response<bool> { Data = true });

            page.WaypointManager = fakeWaypointManager.Object;
            page.ValidationManager = fakeValidationManager.Object;

            page.Location = "1601 Maple St";
            page.Notes = "notes";
            page.StartDate = currentTime;
            page.EndDate = currentTime;
            var result = page.OnPost(0);
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
                    um.CreateWaypoint(0, "1601 Maple St", currentTime.AddDays(1), currentTime, "notes"))
                .Returns(new Response<int>
                    {StatusCode = (uint) Ui.StatusCode.BadRequest, ErrorMessage = Ui.ErrorMessages.InvalidStartDate});
            var fakeValidationManager = new Mock<ValidationManager>();
            fakeValidationManager.Setup(vm => vm.DetermineIfValidEventDates(0, currentTime.AddDays(1), currentTime))
                .Returns(new Response<bool> {Data = true});
            fakeValidationManager.Setup(vm => vm.FindClashingEvent(0, currentTime.AddDays(1), currentTime))
                .Returns(new Response<IEvent> {Data = null});
            fakeValidationManager.Setup(vm => vm.DetermineIfValidLocation("1601 Maple St"))
                .Returns(new Response<bool> { Data = true });

            var page = TestPageBuilder.BuildPage<CreateWaypointModel>(session.Object);
            page.WaypointManager = fakeWaypointManager.Object;
            page.ValidationManager = fakeValidationManager.Object;

            page.Location = "1601 Maple St";
            page.Notes = "notes";
            page.StartDate = currentTime.AddDays(1);
            page.EndDate = currentTime;

            var result = page.OnPost(0);
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

            var page = TestPageBuilder.BuildPage<CreateWaypointModel>(session.Object);
            page.ValidationManager = fakeValidationManager.Object;
            page.Location = "1601 Maple St";
            page.StartDate = currentTime;
            page.EndDate = currentTime.AddDays(2);

            var result = page.OnPost(0);

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
            fakeValidationManager.Setup(vm => vm.FindClashingEvent(0, currentTime, currentTime.AddDays(2)))
                .Returns(new Response<IEvent>
                    {ErrorMessage = $"{Ui.ErrorMessages.ClashingEventDates} {currentTime} {currentTime.AddDays(1)}"});

            var page = TestPageBuilder.BuildPage<CreateWaypointModel>(session.Object);
            page.ValidationManager = fakeValidationManager.Object;
            page.Location = "Hilton";
            page.StartDate = currentTime;
            page.EndDate = currentTime.AddDays(2);

            var result = page.OnPost(0);

            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.AreEqual($"{Ui.ErrorMessages.ClashingEventDates} {DateTime.Now} {DateTime.Now.AddDays(1)}",
                page.ErrorMessage);
        }

        [TestMethod]
        public void Post_InvalidLocation_ReturnsErrorMessage()
        {
            var session = new Mock<ISession>();
            var currentTime = DateTime.Now;

            var fakeValidationManager = new Mock<ValidationManager>();
            fakeValidationManager.Setup(vm => vm.DetermineIfValidEventDates(0, currentTime, currentTime))
                .Returns(new Response<bool> { Data = true });
            fakeValidationManager.Setup(vm => vm.FindClashingEvent(0, currentTime, currentTime))
                .Returns(new Response<IEvent> { Data = null });
            fakeValidationManager.Setup(vm => vm.DetermineIfValidLocation("Hilton"))
                .Returns(new Response<bool> { Data = false, ErrorMessage = Ui.ErrorMessages.InvalidLocation });

            var page = TestPageBuilder.BuildPage<CreateWaypointModel>(session.Object);
            page.ValidationManager = fakeValidationManager.Object;
            page.Location = "Hilton";
            page.StartDate = currentTime;
            page.EndDate = currentTime;

            var result = page.OnPost(0);

            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.AreEqual(Ui.ErrorMessages.InvalidLocation, page.ErrorMessage);
        }

        [TestMethod]
        public void PostCancel_Success_RedirectToTrip()
        {
            var session = new Mock<ISession>();
            var page = TestPageBuilder.BuildPage<CreateWaypointModel>(session.Object);

            var result = page.OnPostCancel(1);

            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirect = (RedirectToPageResult) result;
            Assert.AreEqual("Trip", redirect.PageName);
            Assert.AreEqual(1, redirect.RouteValues["tripId"]);
        }
    }
}