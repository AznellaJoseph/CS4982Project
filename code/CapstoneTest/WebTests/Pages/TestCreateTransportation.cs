﻿using System;
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
    public class TestCreateTransportation
    {
        [TestMethod]
        public void Post_Success()
        {
            var session = new Mock<ISession>();
            var manager = new Mock<TransportationManager>();
            var currentTime = DateTime.Now;
            manager.Setup(um => um.CreateTransportation(0, "Car", currentTime, currentTime))
                .Returns(new Response<int> { Data = 0 });
            var page = TestPageBuilder.BuildPage<CreateTransportationModel>(session.Object);
            page.FakeTransportationManager = manager.Object;
            
            page.Method = "Car";
            page.StartDate = currentTime;
            page.EndDate = currentTime;
            var result = page.OnPost(0);
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirect = (RedirectToPageResult) result;
            Assert.AreEqual("Trip", redirect.PageName);
        }

        [TestMethod]
        public void Post_Failure()
        {
            var session = new Mock<ISession>();
            var manager = new Mock<TransportationManager>();
            var currentTime = DateTime.Now;
            manager.Setup(um =>
                    um.CreateTransportation(0, "Car", currentTime.AddDays(1), currentTime))
                .Returns(new Response<int> {StatusCode = 400U, ErrorMessage = Ui.ErrorMessages.InvalidStartDate});
            var page = TestPageBuilder.BuildPage<CreateTransportationModel>(session.Object);
            page.FakeTransportationManager = manager.Object;
            
            page.Method = "Car";
            page.StartDate = currentTime.AddDays(1);
            page.EndDate = currentTime;

            var result = page.OnPost(0);
            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.AreEqual(Ui.ErrorMessages.InvalidStartDate, page.ErrorMessage);
        }
    }
}