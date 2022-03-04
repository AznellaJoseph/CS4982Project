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
    public class TestCreateTrip
    {
        [TestMethod]
        public void Post_Success_Redirects()
        {
            var session = new Mock<ISession>();
            var fakeTripManager = new Mock<TripManager>();
            fakeTripManager
                .Setup(um => um.CreateTrip(0, "vacation", "notes", DateTime.Today, DateTime.Today.AddDays(1)))
                .Returns(new Response<int> { Data = 0 });
            var page = TestPageBuilder.BuildPage<CreateTripModel>(session.Object);
            page.TripManager = fakeTripManager.Object;

            page.HttpContext.Session.SetString("userId", "0");
            page.TripName = "vacation";
            page.Notes = "notes";
            page.EndDate = DateTime.Today.AddDays(1);

            var result = page.OnPost();
            var outBytes = Encoding.UTF8.GetBytes("0");
            session.Verify(s => s.Set("tripId", outBytes));
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirect = (RedirectToPageResult)result;
            Assert.AreEqual("index", redirect.PageName);
        }

        [TestMethod]
        public void Post_InvalidStartDate_ReturnsErrorMessage()
        {
            var session = new Mock<ISession>();
            var fakeTripManager = new Mock<TripManager>();
            fakeTripManager.Setup(um =>
                    um.CreateTrip(0, "vacation", "notes", DateTime.Today.AddDays(1), DateTime.Today))
                .Returns(new Response<int> { ErrorMessage = Ui.ErrorMessages.InvalidStartDate });
            var page = TestPageBuilder.BuildPage<CreateTripModel>(session.Object);
            page.TripManager = fakeTripManager.Object;

            page.HttpContext.Session.SetString("userId", "0");
            page.TripName = "vacation";
            page.Notes = "notes";
            page.StartDate = DateTime.Today.AddDays(1);

            var result = page.OnPost();
            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.AreEqual(Ui.ErrorMessages.InvalidStartDate, page.ErrorMessage);
        }

        [TestMethod]
        public void Post_NullTripName_ReturnsErrorMessage()
        {
            var session = new Mock<ISession>();

            var page = TestPageBuilder.BuildPage<CreateTripModel>(session.Object);

            page.HttpContext.Session.SetString("userId", "0");
            page.Notes = "notes";
            page.StartDate = DateTime.Today.AddDays(1);

            var result = page.OnPost();
            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.AreEqual(Ui.ErrorMessages.EmptyTripName, page.ErrorMessage);
        }

        [TestMethod]
        public void PostCancel_Success_RedirectsToIndex()
        {
            var session = new Mock<ISession>();
            var page = TestPageBuilder.BuildPage<CreateTripModel>(session.Object);

            var result = page.OnPostCancel();

            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirect = (RedirectToPageResult)result;
            Assert.AreEqual("index", redirect.PageName);
        }
    }
}