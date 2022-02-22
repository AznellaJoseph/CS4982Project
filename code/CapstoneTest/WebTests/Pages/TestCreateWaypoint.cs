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
        public void Post_Success()
        {
            var session = new Mock<ISession>();
            var fakeWaypointManager = new Mock<WaypointManager>();
            var currentTime = DateTime.Now;
            fakeWaypointManager.Setup(um => um.CreateWaypoint(0, "1601 Maple St", currentTime, currentTime, "notes"))
                .Returns(new Response<int> { Data = 0 });
            var page = TestPageBuilder.BuildPage<CreateWaypointModel>(session.Object);
            page.WaypointManager = fakeWaypointManager.Object;

            page.HttpContext.Session.SetString("tripId", "0");
            page.Location = "1601 Maple St";
            page.Notes = "notes";
            page.StartDate = currentTime;
            page.EndDate = currentTime;

            var result = page.OnPost();
            var outBytes = Encoding.UTF8.GetBytes("0");
            session.Verify(s => s.Set("waypointId", outBytes));
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirect = (RedirectToPageResult)result;
            Assert.AreEqual("index", redirect.PageName);
        }

        [TestMethod]
        public void Post_Failure()
        {
            var session = new Mock<ISession>();
            var fakeWaypointManager = new Mock<WaypointManager>();
            var currentTime = DateTime.Now;
            fakeWaypointManager.Setup(um =>
                    um.CreateWaypoint(0, "1601 Maple St", currentTime.AddDays(1), currentTime, "notes"))
                .Returns(new Response<int> { ErrorMessage = Ui.ErrorMessages.InvalidStartDate });
            var page = TestPageBuilder.BuildPage<CreateWaypointModel>(session.Object);
            page.WaypointManager = fakeWaypointManager.Object;

            page.HttpContext.Session.SetString("tripId", "0");
            page.Location = "1601 Maple St";
            page.Notes = "notes";
            page.StartDate = currentTime.AddDays(1);
            page.EndDate = currentTime;

            var result = page.OnPost();
            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.AreEqual(Ui.ErrorMessages.InvalidStartDate, page.ErrorMessage);
        }
    }
}