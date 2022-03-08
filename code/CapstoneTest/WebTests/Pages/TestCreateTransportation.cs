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
    public class TestCreateTransportation
    {
        [TestMethod]
        public void Post_Success()
        {
            var session = new Mock<ISession>();
            var manager = new Mock<TransportationManager>();
            var currentTime = DateTime.Now;
            manager.Setup(um => um.CreateTransportation(0, "Car", currentTime, currentTime, null))
                .Returns(new Response<int> { Data = 0 });
            var page = TestPageBuilder.BuildPage<CreateTransportationModel>(session.Object);
            page.FakeTransportationManager = manager.Object;

            page.Method = "Car";
            page.StartDate = currentTime;
            page.EndDate = currentTime;
            var result = page.OnPost(0);
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirect = (RedirectToPageResult)result;
            Assert.AreEqual("Trip", redirect.PageName);
        }

        [TestMethod]
        public void Post_InvalidStartDate_ReturnsErrorMessage()
        {
            var session = new Mock<ISession>();
            var manager = new Mock<TransportationManager>();
            var currentTime = DateTime.Now;
            manager.Setup(um =>
                    um.CreateTransportation(0, "Car", currentTime.AddDays(1), currentTime, null))
                .Returns(new Response<int>
                { StatusCode = (uint)Ui.StatusCode.BadRequest, ErrorMessage = Ui.ErrorMessages.InvalidStartDate });
            var page = TestPageBuilder.BuildPage<CreateTransportationModel>(session.Object);
            page.FakeTransportationManager = manager.Object;

            page.Method = "Car";
            page.StartDate = currentTime.AddDays(1);
            page.EndDate = currentTime;

            var result = page.OnPost(0);
            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.AreEqual(Ui.ErrorMessages.InvalidStartDate, page.ErrorMessage);
        }

        [TestMethod]
        public void Post_ClashingEventExists_ReturnsErrorMessage()
        {
            var session = new Mock<ISession>();
            var manager = new Mock<TransportationManager>();
            var eventManager = new Mock<EventManager>();
            var currentTime = DateTime.Now;
            eventManager.Setup(em => em.FindClashingEvent(0, currentTime, currentTime.AddDays(2)))
                .Returns(new Response<IEvent> { Data = new Transportation { StartDate = currentTime, EndDate = currentTime.AddHours(2) } });
            var page = TestPageBuilder.BuildPage<CreateTransportationModel>(session.Object);
            page.FakeTransportationManager = manager.Object;
            page.EventManager = eventManager.Object;

            page.Method = "Car";
            page.StartDate = currentTime;
            page.EndDate = currentTime.AddDays(2);

            var result = page.OnPost(0);
            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.AreEqual($"{Ui.ErrorMessages.ClashingEventDates} {currentTime} to {currentTime.AddHours(2)}.", page.ErrorMessage);
        }

        [TestMethod]
        public void PostCancel_Success()
        {
            var session = new Mock<ISession>();
            var page = TestPageBuilder.BuildPage<CreateTransportationModel>(session.Object);

            var result = page.OnPostCancel(1);

            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirect = (RedirectToPageResult)result;
            Assert.AreEqual("Trip", redirect.PageName);
            Assert.AreEqual(1, redirect.RouteValues["tripId"]);
        }
    }
}