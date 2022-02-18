using System;
using System.Text;
using CapstoneBackend.Model;
using CapstoneWeb.Pages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CapstoneTest.WebTests.Pages
{
    [TestClass]
    public class TestCreateTrip
    {
        [TestMethod]
        public void Post_Success()
        {
            var session = new Mock<ISession>();
            var fakeTripManager = new Mock<TripManager>();
            fakeTripManager
                .Setup(um => um.CreateTrip(0, "vacation", "notes", DateTime.Today, DateTime.Today.AddDays(1)))
                .Returns(new Response<int> {Data = 0});
            var page = TestPageBuilder.BuildPage<CreateTripModel>(session.Object);
            page.TripManager = fakeTripManager.Object;

            page.HttpContext.Session.SetString("userId", "0");
            page.Name = "vacation";
            page.Notes = "notes";
            page.EndDate = DateTime.Today.AddDays(1);

            var result = page.OnPost();
            var outBytes = Encoding.UTF8.GetBytes("0");
            session.Verify(s => s.Set("tripId", outBytes));
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirect = (RedirectToPageResult) result;
            Assert.AreEqual("index", redirect.PageName);
        }

        [TestMethod]
        public void Post_Failure()
        {
            var session = new Mock<ISession>();
            var fakeTripManager = new Mock<TripManager>();
            fakeTripManager.Setup(um =>
                    um.CreateTrip(0, "vacation", "notes", DateTime.Today.AddDays(1), DateTime.Today))
                .Returns(new Response<int> {ErrorMessage = "Start date must not be before end date."});
            var page = TestPageBuilder.BuildPage<CreateTripModel>(session.Object);
            page.TripManager = fakeTripManager.Object;

            page.HttpContext.Session.SetString("userId", "0");
            page.Name = "vacation";
            page.Notes = "notes";
            page.StartDate = DateTime.Today.AddDays(1);

            var result = page.OnPost();
            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.AreEqual("Start date must not be before end date.", page.ErrorMessage);
        }
    }
}