using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class TestEditLodging
    {
        [TestMethod]
        public void Get_Success_ReturnsPageResult()
        {
            var outBytes = Encoding.UTF8.GetBytes("50");
            var session = new Mock<ISession>();
            session.SetupGet(s => s.Keys).Returns(new List<string> { "userId" });
            session.Setup(s => s.TryGetValue("userId", out outBytes)).Returns(true);

            var currentTime = DateTime.Now;
            var fakeLodging = new Lodging
            {
                TripId = 1,
                LodgingId = 1,
                Location = "TestLocation",
                StartDate = currentTime,
                EndDate = currentTime,
                Notes = "notes"
            };

            var mockLodgingManager = new Mock<LodgingManager>();
            mockLodgingManager.Setup(tm => tm.GetLodgingById(It.IsAny<int>()))
                .Returns(new Response<Lodging> { Data = fakeLodging });

            var page = TestPageBuilder.BuildPage<EditLodgingModel>(session.Object);
            page.LodgingManager = mockLodgingManager.Object;

            var result = page.OnGet(1, 1);

            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.AreEqual(fakeLodging.Location, page.Location);
            Assert.AreEqual(fakeLodging.StartDate, page.StartDate);
            Assert.AreEqual(fakeLodging.EndDate, page.EndDate);
            Assert.AreEqual(fakeLodging.Notes, page.Notes);
        }

        [TestMethod]
        public void Get_UserIdNotFound_RedirectToIndex()
        {
            var session = new Mock<ISession>();
            session.SetupGet(s => s.Keys).Returns(new List<string>());

            var page = TestPageBuilder.BuildPage<EditLodgingModel>(session.Object);
            var result = page.OnGet(1, 1);

            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirect = (RedirectToPageResult)result;
            Assert.AreEqual("Index", redirect.PageName);
        }

        [TestMethod]
        public void Get_NonExistingLodging_RedirectsToTrip()
        {
            var outBytes = Encoding.UTF8.GetBytes("50");
            var session = new Mock<ISession>();
            session.SetupGet(s => s.Keys).Returns(new List<string> { "userId" });
            session.Setup(s => s.TryGetValue("userId", out outBytes)).Returns(true);

            var mockLodgingManager = new Mock<LodgingManager>();
            mockLodgingManager.Setup(tm => tm.GetLodgingById(It.IsAny<int>()))
                .Returns(new Response<Lodging> { Data = null });

            var page = TestPageBuilder.BuildPage<EditLodgingModel>(session.Object);
            page.LodgingManager = mockLodgingManager.Object;

            var result = page.OnGet(1, 1);

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

            var mockLodgingManager = new Mock<LodgingManager>();
            mockLodgingManager.Setup(tm => tm.GetLodgingById(3))
                .Returns(new Response<Lodging> { Data = new Lodging { TripId = 2 } });

            var page = TestPageBuilder.BuildPage<EditLodgingModel>(session.Object);
            page.LodgingManager = mockLodgingManager.Object;

            var result = page.OnGet(3, 8);

            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirect = (RedirectToPageResult)result;
            Assert.AreEqual("Trip", redirect.PageName);
        }


        [TestMethod]
        public void Post_Success()
        {
            var session = new Mock<ISession>();
            var mockLodgingManager = new Mock<LodgingManager>();
            var mockValidationManager = new Mock<ValidationManager>();
            var currentTime = DateTime.Now;

            var fakeLodging = new Lodging
            {
                TripId = 0,
                LodgingId = 0,
                Location = "TestLocation",
                StartDate = currentTime,
                EndDate = currentTime,
                Notes = "notes"
            };

            mockValidationManager.Setup(vm => vm.DetermineIfValidEventDates(0, currentTime, currentTime))
                .Returns(new Response<bool> { Data = true, ErrorMessage = null });
            mockValidationManager.Setup(vm => vm.DetermineIfValidLocation(fakeLodging.Location))
                .Returns(new Response<bool> { Data = true, ErrorMessage = null });
            mockLodgingManager.Setup(lm => lm.EditLodging(It.IsAny<Lodging>()))
                .Returns(new Response<bool> { Data = true, StatusCode = (uint) Ui.StatusCode.Success });
    

            var page = TestPageBuilder.BuildPage<EditLodgingModel>(session.Object);

            page.ValidationManager = mockValidationManager.Object;
            page.LodgingManager = mockLodgingManager.Object;
            page.Location = fakeLodging.Location;
            page.StartDate = fakeLodging.StartDate;
            page.EndDate = fakeLodging.EndDate;
            page.Notes = fakeLodging.Notes;
            
            var result = page.OnPost(0, 0);

            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirect = (RedirectToPageResult)result;
            Assert.AreEqual("Trip", redirect.PageName);
        }

        [TestMethod]
        public void Post_InvalidEventDates_ReturnsErrorMessage()
        {
            var session = new Mock<ISession>();
            var currentTime = DateTime.Now;

            var fakeValidationManager = new Mock<ValidationManager>();
            fakeValidationManager.Setup(vm => vm.DetermineIfValidEventDates(0, currentTime, currentTime.AddDays(2)))
                .Returns(new Response<bool> { ErrorMessage = $"{Ui.ErrorMessages.ClashingEventDates} {currentTime} {currentTime.AddDays(1)}"});
            fakeValidationManager.Setup(vm => vm.DetermineIfValidLocation("Hilton"))
                .Returns(new Response<bool> { Data = true });

            var page = TestPageBuilder.BuildPage<EditLodgingModel>(session.Object);
            page.ValidationManager = fakeValidationManager.Object;
            page.Location = "Hilton";
            page.StartDate = currentTime;
            page.EndDate = currentTime.AddDays(2);

            var result = page.OnPost(0, 0);

            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.AreEqual($"{Ui.ErrorMessages.ClashingEventDates} {currentTime} {currentTime.AddDays(1)}", page.ErrorMessage);
        }

        [TestMethod]
        public void Post_InvalidLocation_ReturnsErrorMessage()
        {
            var session = new Mock<ISession>();
            var currentTime = DateTime.Now;

            var fakeValidationManager = new Mock<ValidationManager>();
            fakeValidationManager.Setup(vm => vm.DetermineIfValidEventDates(0, currentTime, currentTime))
                .Returns(new Response<bool> { Data = true });
            fakeValidationManager.Setup(vm => vm.DetermineIfValidLocation("Hilton"))
                .Returns(new Response<bool> { Data = false, ErrorMessage = Ui.ErrorMessages.InvalidLocation });

            var page = TestPageBuilder.BuildPage<EditLodgingModel>(session.Object);
            page.ValidationManager = fakeValidationManager.Object;
            page.Location = "Hilton";
            page.StartDate = currentTime;
            page.EndDate = currentTime;

            var result = page.OnPost(0, 0);

            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.AreEqual(Ui.ErrorMessages.InvalidLocation, page.ErrorMessage);
        }

        [TestMethod]
        public void PostCancel_Success()
        {
            var session = new Mock<ISession>();
            var page = TestPageBuilder.BuildPage<EditLodgingModel>(session.Object);

            var result = page.OnPostCancel(1);

            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirect = (RedirectToPageResult)result;
            Assert.AreEqual("Trip", redirect.PageName);
            Assert.AreEqual(1, redirect.RouteValues["tripId"]);
        }

    }
}
