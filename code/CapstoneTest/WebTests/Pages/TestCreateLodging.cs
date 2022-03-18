﻿using System;
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
    public class TestCreateLodging
    {
        [TestMethod]
        public void Post_Success()
        {
            var session = new Mock<ISession>();
            var manager = new Mock<LodgingManager>();
            var currentTime = DateTime.Now;
            manager.Setup(lm => lm.CreateLodging(0, "Grand Hyatt", currentTime, currentTime, null))
                .Returns(new Response<int> { Data = 0 });
            var fakeValidationManager = new Mock<ValidationManager>();
            fakeValidationManager.Setup(vm => vm.DetermineIfValidEventDates(0, currentTime, currentTime))
                .Returns(new Response<bool> { Data = true });

            var page = TestPageBuilder.BuildPage<CreateLodgingModel>(session.Object);
            page.LodgingManager = manager.Object;
            page.ValidationManager = fakeValidationManager.Object;
            page.Location = "Grand Hyatt";
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
            var manager = new Mock<LodgingManager>();
            var currentTime = DateTime.Now;
            manager.Setup(lm => lm.CreateLodging(0, "Grand Hyatt", currentTime.AddDays(1), currentTime, null))
                .Returns(new Response<int>
                { StatusCode = (uint)Ui.StatusCode.BadRequest, ErrorMessage = Ui.ErrorMessages.InvalidStartDate });
            var fakeValidationManager = new Mock<ValidationManager>();
            fakeValidationManager.Setup(vm => vm.DetermineIfValidEventDates(0, currentTime.AddDays(1), currentTime))
                .Returns(new Response<bool> { Data = true });

            var page = TestPageBuilder.BuildPage<CreateLodgingModel>(session.Object);
            page.LodgingManager = manager.Object;
            page.ValidationManager = fakeValidationManager.Object;
            page.Location = "Grand Hyatt";
            page.StartDate = currentTime.AddDays(1);
            page.EndDate = currentTime;

            var result = page.OnPost(0);

            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.AreEqual(Ui.ErrorMessages.InvalidStartDate, page.ErrorMessage);
        }

        [TestMethod]
        public void PostCancel_Success()
        {
            var session = new Mock<ISession>();
            var page = TestPageBuilder.BuildPage<CreateLodgingModel>(session.Object);

            var result = page.OnPostCancel(1);

            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirect = (RedirectToPageResult)result;
            Assert.AreEqual("Trip", redirect.PageName);
            Assert.AreEqual(1, redirect.RouteValues["tripId"]);
        }

    }
}