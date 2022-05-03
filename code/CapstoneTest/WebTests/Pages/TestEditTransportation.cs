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
    public class TestEditTransportation
    {
        [TestMethod]
        public void Get_Success_ReturnsPageResult()
        {
            var outBytes = Encoding.UTF8.GetBytes("50");
            var session = new Mock<ISession>();
            session.SetupGet(s => s.Keys).Returns(new List<string> { "userId" });
            session.Setup(s => s.TryGetValue("userId", out outBytes)).Returns(true);

            var currentTime = DateTime.Now;
            var fakeTransportation = new Transportation
            {
                TripId = 8,
                TransportationId = 3,
                Method = "TestMethod",
                StartDate = currentTime,
                EndDate = currentTime,
                Notes = "TestNotes"
            };

            var mockTransportationManager = new Mock<TransportationManager>();
            mockTransportationManager.Setup(tm => tm.GetTransportationById(3))
                .Returns(new Response<Transportation> { Data = fakeTransportation });
            var page = TestPageBuilder.BuildPage<EditTransportationModel>(session.Object);
            page.TransportationManager = mockTransportationManager.Object;

            var result = page.OnGet(3, 8);

            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.AreEqual(fakeTransportation.Method, page.Method);
            Assert.AreEqual(fakeTransportation.StartDate, page.StartDate);
            Assert.AreEqual(fakeTransportation.EndDate, page.EndDate);
            Assert.AreEqual(fakeTransportation.Notes, page.Notes);
        }

        [TestMethod]
        public void Get_UserIdNotFound_RedirectToIndex()
        {
            var session = new Mock<ISession>();
            session.SetupGet(s => s.Keys).Returns(new List<string>());

            var page = TestPageBuilder.BuildPage<EditTransportationModel>(session.Object);
            var result = page.OnGet(1, 8);

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
                .Returns(new Response<Transportation> { Data = null });

            var page = TestPageBuilder.BuildPage<EditTransportationModel>(session.Object);
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
                .Returns(new Response<Transportation> { Data = new Transportation { TripId = 3 } });

            var page = TestPageBuilder.BuildPage<EditTransportationModel>(session.Object);
            page.TransportationManager = mockTransportationManager.Object;

            var result = page.OnGet(5, 8);

            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirect = (RedirectToPageResult)result;
            Assert.AreEqual("Trip", redirect.PageName);
        }

        [TestMethod]
        public void Post_Success()
        {
            var session = new Mock<ISession>();
            var fakeTransportationManager = new Mock<TransportationManager>();
            var currentTime = DateTime.Now;

            var fakeTransportation = new Transportation
            {
                Method = "Car",
                StartDate = currentTime,
                EndDate = currentTime,
                Notes = "TestNotes"
            };

            fakeTransportationManager.Setup(um => um.EditTransportation(It.IsAny<Transportation>()))
                .Returns(new Response<bool> { Data = true, StatusCode = (uint) Ui.StatusCode.Success });
            var fakeValidationManager = new Mock<ValidationManager>();
            fakeValidationManager.Setup(vm => vm.DetermineIfValidEventDates(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(new Response<bool> { Data = true });
            fakeValidationManager.Setup(vm => vm.FindClashingEvent(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(new Response<IEvent> { Data = null });

            var page = TestPageBuilder.BuildPage<EditTransportationModel>(session.Object);
            page.TransportationManager = fakeTransportationManager.Object;
            page.ValidationManager = fakeValidationManager.Object;

            var result = page.OnPost(0, 0);
            
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            
            var redirect = (RedirectToPageResult)result;
            Assert.AreEqual("Trip", redirect.PageName); ;
            //Assert.AreEqual(fakeTransportation.Method, page.Method);
            //Assert.AreEqual(fakeTransportation.StartDate, page.StartDate);
            //Assert.AreEqual(fakeTransportation.EndDate, page.EndDate);
            //Assert.AreEqual(fakeTransportation.Notes, page.Notes);
        }

        [TestMethod]
        public void Post_InvalidStartDate_ReturnsErrorMessage()
        {
            var session = new Mock<ISession>();
            var manager = new Mock<TransportationManager>();
            var currentTime = DateTime.Now;
            manager.Setup(um =>
                    um.EditTransportation(It.IsAny<Transportation>()))
                .Returns(new Response<bool>
                { StatusCode = (uint)Ui.StatusCode.BadRequest, ErrorMessage = Ui.ErrorMessages.InvalidStartDate });
            var fakeValidationManager = new Mock<ValidationManager>();
            fakeValidationManager.Setup(vm => vm.DetermineIfValidEventDates(0, currentTime.AddDays(1), currentTime))
                .Returns(new Response<bool> { Data = true });
            fakeValidationManager.Setup(vm => vm.FindClashingEvent(0, currentTime.AddDays(1), currentTime))
                .Returns(new Response<IEvent> { Data = null });

            var page = TestPageBuilder.BuildPage<EditTransportationModel>(session.Object);
            page.TransportationManager = manager.Object;
            page.ValidationManager = fakeValidationManager.Object;

            page.Method = "Car";
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

            var page = TestPageBuilder.BuildPage<EditTransportationModel>(session.Object);
            page.ValidationManager = fakeValidationManager.Object;
            page.Method = "Car";
            page.StartDate = currentTime;
            page.EndDate = currentTime.AddDays(2);

            var result = page.OnPost(0, 0);

            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.AreEqual($"{Ui.ErrorMessages.EventStartDateBeforeTripStartDate} {DateTime.Now.AddDays(1)}", page.ErrorMessage);
        }

        [TestMethod]
        public void Post_ClashingEvent_ReturnsErrorMessage()
        {
            var session = new Mock<ISession>();
            var currentTime = DateTime.Now;

            var fakeValidationManager = new Mock<ValidationManager>();
            fakeValidationManager.Setup(vm => vm.DetermineIfValidEventDates(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(new Response<bool> { Data = true });
            fakeValidationManager.Setup(vm => vm.FindClashingEvent(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(new Response<IEvent>
                {
                    Data = new Transportation {TransportationId = 1 },
                    ErrorMessage = $"{Ui.ErrorMessages.ClashingEventDates} {currentTime} {currentTime.AddDays(1)}"
                });

            var page = TestPageBuilder.BuildPage<EditTransportationModel>(session.Object);
            page.ValidationManager = fakeValidationManager.Object;
            page.Method = "Car";
            page.StartDate = currentTime;
            page.EndDate = currentTime.AddDays(2);

            var result = page.OnPost(0, 0);

            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.IsFalse(string.IsNullOrEmpty(page.ErrorMessage));
        }

        [TestMethod]
        public void PostCancel_Success()
        {
            var session = new Mock<ISession>();
            var page = TestPageBuilder.BuildPage<EditTransportationModel>(session.Object);

            var result = page.OnPostCancel(1);

            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirect = (RedirectToPageResult)result;
            Assert.AreEqual("Trip", redirect.PageName);
            Assert.AreEqual(1, redirect.RouteValues["tripId"]);
        }
    }
}