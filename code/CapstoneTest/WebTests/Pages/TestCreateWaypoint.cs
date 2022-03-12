using System;
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
    public class TestCreateWaypoint
    {
        [TestMethod]
        public void Post_Success_RedirectsToTrip()
        {
            var session = new Mock<ISession>();
            var fakeWaypointManager = new Mock<WaypointManager>();
            var currentTime = DateTime.Now;
            fakeWaypointManager.Setup(um => um.CreateWaypoint(0, "1601 Maple St", currentTime, currentTime, "notes"))
                .Returns(new Response<int> { Data = 0 });
            var page = TestPageBuilder.BuildPage<CreateWaypointModel>(session.Object);
            page.WaypointManager = fakeWaypointManager.Object;
            
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
                .Returns(new Response<int> {StatusCode = (uint)Ui.StatusCode.BadRequest, ErrorMessage = Ui.ErrorMessages.InvalidStartDate});
            var page = TestPageBuilder.BuildPage<CreateWaypointModel>(session.Object);
            page.WaypointManager = fakeWaypointManager.Object;
            
            page.Location = "1601 Maple St";
            page.Notes = "notes";
            page.StartDate = currentTime.AddDays(1);
            page.EndDate = currentTime;

            var result = page.OnPost(0);
            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.AreEqual(Ui.ErrorMessages.InvalidStartDate, page.ErrorMessage);
        }

        [TestMethod]
        public void Post_InvalidLocation_ReturnsErrorMessage()
        {
            var session = new Mock<ISession>();
            var currentTime = DateTime.Now;

            var page = TestPageBuilder.BuildPage<CreateWaypointModel>(session.Object);

            page.Notes = "notes";
            page.StartDate = currentTime.AddDays(1);
            page.EndDate = currentTime;

            var result = page.OnPost(0);
            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.AreEqual(Ui.ErrorMessages.EmptyWaypointLocation, page.ErrorMessage);
        }

        [TestMethod]
        public void PostCancel_Success_RedirectToTrip()
        {
            var session = new Mock<ISession>();
            var page = TestPageBuilder.BuildPage<CreateWaypointModel>(session.Object);

            var result = page.OnPostCancel(1);

            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirect = (RedirectToPageResult)result;
            Assert.AreEqual("Trip", redirect.PageName);
            Assert.AreEqual(1, redirect.RouteValues["tripId"]);
        }
    }
}