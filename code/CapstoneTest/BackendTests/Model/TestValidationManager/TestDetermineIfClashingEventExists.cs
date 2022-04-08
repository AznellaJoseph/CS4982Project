using System;
using System.Collections.Generic;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CapstoneTest.BackendTests.Model.TestValidationManager
{
    [TestClass]
    public class TestDetermineIfClashingEventExists
    {
        [TestMethod]
        public void FindClashingEvent_NoClashingEvent()
        {
            var mockEventManager = new Mock<EventManager>();
            mockEventManager.Setup(em => em.GetEventsOnDate(1, DateTime.Today.AddDays(-5))).Returns(new Response<IList<IEvent>>
            { Data = new List<IEvent>() });

            var validationManager = new ValidationManager { EventManager = mockEventManager.Object };

            var clashingTripResponse =
                validationManager.DetermineIfClashingEventExists(1, DateTime.Today.AddDays(-5), DateTime.Today.AddDays(-5));

            Assert.IsFalse(clashingTripResponse.Data);
        }

        [TestMethod]
        public void FindClashingEvent_ClashingEventExists_ReturnsErrorMessage()
        {
            var mockEventManager = new Mock<EventManager>();
            mockEventManager.Setup(em => em.GetEventsOnDate(1, DateTime.Today)).Returns(new Response<IList<IEvent>>
            {
                Data = new List<IEvent>
                    {new Transportation {StartDate = DateTime.Today, EndDate = DateTime.Today.AddDays(4)}}
            });

            var validationManager = new ValidationManager { EventManager = mockEventManager.Object };

            var clashingTripResponse =
                validationManager.DetermineIfClashingEventExists(1, DateTime.Today, DateTime.Today.AddHours(3));

            Assert.AreEqual((uint)Ui.StatusCode.BadRequest, clashingTripResponse.StatusCode);
            Assert.AreEqual($"{Ui.ErrorMessages.ClashingEventDates} {DateTime.Today} to {DateTime.Today.AddDays(4)}.",
                clashingTripResponse.ErrorMessage);
        }
    }
}