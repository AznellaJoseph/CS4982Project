using System;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CapstoneTest.BackendTests.Model.TestValidationManager
{
    [TestClass]
    public class TestDetermineIfValidEventDates
    {
        [TestMethod]
        public void DetermineIfValidEventDates_NullTripFound_ReturnsErrorMessage()
        {
            var mockTripManager = new Mock<TripManager>();
            mockTripManager.Setup(tm => tm.GetTripByTripId(1)).Returns(new Response<Trip> {Data = null});

            var validationManager = new ValidationManager
            {
                TripManager = mockTripManager.Object
            };

            var validDatesResponse =
                validationManager.DetermineIfValidEventDates(1, DateTime.Today, DateTime.Today.AddDays(2));

            Assert.AreEqual((uint) Ui.StatusCode.DataNotFound, validDatesResponse.StatusCode);
            Assert.AreEqual(Ui.ErrorMessages.TripNotFound, validDatesResponse.ErrorMessage);
        }

        [TestMethod]
        public void DetermineIfValidEventDates_EventStartBeforeTripStart_ReturnsErrorMessage()
        {
            var mockTripManager = new Mock<TripManager>();
            mockTripManager.Setup(tm => tm.GetTripByTripId(1)).Returns(new Response<Trip>
                {Data = new Trip {TripId = 1, StartDate = DateTime.Today.AddDays(-2), EndDate = DateTime.Today}});

            var validationManager = new ValidationManager
            {
                TripManager = mockTripManager.Object
            };

            var validDatesResponse =
                validationManager.DetermineIfValidEventDates(1, DateTime.Today.AddDays(-5), DateTime.Today);

            Assert.AreEqual((uint) Ui.StatusCode.BadRequest, validDatesResponse.StatusCode);
            Assert.AreEqual(Ui.ErrorMessages.EventStartDateBeforeTripStartDate + DateTime.Today.AddDays(-2).ToShortDateString(),
                validDatesResponse.ErrorMessage);
        }

        [TestMethod]
        public void DetermineIfValidEventDates_EventStartAfterTripEnd_ReturnsErrorMessage()
        {
            var mockTripManager = new Mock<TripManager>();
            mockTripManager.Setup(tm => tm.GetTripByTripId(1)).Returns(new Response<Trip>
                {Data = new Trip {TripId = 1, StartDate = DateTime.Today.AddDays(-2), EndDate = DateTime.Today}});

            var validationManager = new ValidationManager
            {
                TripManager = mockTripManager.Object
            };

            var validDatesResponse =
                validationManager.DetermineIfValidEventDates(1, DateTime.Today.AddDays(5), DateTime.Today);

            Assert.AreEqual((uint) Ui.StatusCode.BadRequest, validDatesResponse.StatusCode);
            Assert.AreEqual(Ui.ErrorMessages.EventStartDateAfterTripEndDate + DateTime.Today.ToShortDateString(),
                validDatesResponse.ErrorMessage);
        }

        [TestMethod]
        public void DetermineIfValidEventDates_EventEndBeforeTripStart_ReturnsErrorMessage()
        {
            var mockTripManager = new Mock<TripManager>();
            mockTripManager.Setup(tm => tm.GetTripByTripId(1)).Returns(new Response<Trip>
                {Data = new Trip {TripId = 1, StartDate = DateTime.Today.AddDays(-2), EndDate = DateTime.Today}});

            var validationManager = new ValidationManager
            {
                TripManager = mockTripManager.Object
            };

            var validDatesResponse =
                validationManager.DetermineIfValidEventDates(1, DateTime.Today, DateTime.Today.AddDays(-5));

            Assert.AreEqual((uint) Ui.StatusCode.BadRequest, validDatesResponse.StatusCode);
            Assert.AreEqual(Ui.ErrorMessages.EventEndDateBeforeTripStartDate + DateTime.Today.AddDays(-2).ToShortDateString(),
                validDatesResponse.ErrorMessage);
        }

        [TestMethod]
        public void DetermineIfValidEventDates_EventEndAfterTripEnd_ReturnsErrorMessage()
        {
            var mockTripManager = new Mock<TripManager>();
            mockTripManager.Setup(tm => tm.GetTripByTripId(1)).Returns(new Response<Trip>
                {Data = new Trip {TripId = 1, StartDate = DateTime.Today.AddDays(-2), EndDate = DateTime.Today}});

            var validationManager = new ValidationManager
            {
                TripManager = mockTripManager.Object
            };

            var validDatesResponse =
                validationManager.DetermineIfValidEventDates(1, DateTime.Today, DateTime.Today.AddDays(5));

            Assert.AreEqual((uint) Ui.StatusCode.BadRequest, validDatesResponse.StatusCode);
            Assert.AreEqual(Ui.ErrorMessages.EventEndDateAfterTripEndDate + DateTime.Today.ToShortDateString(),
                validDatesResponse.ErrorMessage);
        }

        [TestMethod]
        public void DetermineIfValidEventDates_Success()
        {
            var mockTripManager = new Mock<TripManager>();
            mockTripManager.Setup(tm => tm.GetTripByTripId(1)).Returns(new Response<Trip>
                {Data = new Trip {TripId = 1, StartDate = DateTime.Today.AddDays(-2), EndDate = DateTime.Today.AddDays(1)}});

            var validationManager = new ValidationManager
            {
                TripManager = mockTripManager.Object
            };

            var validDatesResponse =
                validationManager.DetermineIfValidEventDates(1, DateTime.Today, DateTime.Today.AddHours(1));

            Assert.IsTrue(validDatesResponse.Data);
        }
    }
}