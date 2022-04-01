using System;
using System.Collections.Generic;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CapstoneTest.BackendTests.Model.TestValidationManager
{
    [TestClass]
    public class TestFindClashingTrip
    {
        [TestMethod]
        public void FindClashingTrip_UserHasNoTrips_NullClashingTrip()
        {
            var mockTripManager = new Mock<TripManager>();
            mockTripManager.Setup(tm => tm.GetTripsByUser(1)).Returns(new Response<IList<Trip>> {Data = null});

            var validationManager = new ValidationManager {TripManager = mockTripManager.Object};

            var clashingTripResponse = validationManager.DetermineIfClashingTripExists(1, DateTime.Today, DateTime.Today.AddDays(1));

            Assert.IsNull(clashingTripResponse.Data);
        }

        [TestMethod]
        public void FindClashingTrip_NoClashingTrip()
        {
            var mockTripManager = new Mock<TripManager>();
            mockTripManager.Setup(tm => tm.GetTripsByUser(1)).Returns(new Response<IList<Trip>>
                {Data = new List<Trip> {new() {StartDate = DateTime.Today, EndDate = DateTime.Today.AddDays(4)}}});

            var validationManager = new ValidationManager {TripManager = mockTripManager.Object};

            var clashingTripResponse =
                validationManager.DetermineIfClashingTripExists(1, DateTime.Today.AddDays(-5), DateTime.Today.AddDays(-3));

            Assert.IsNull(clashingTripResponse.Data);
        }

        [TestMethod]
        public void FindClashingTrip_ClashingTripExists_ReturnsErrorMessage()
        {
            var mockTripManager = new Mock<TripManager>();
            mockTripManager.Setup(tm => tm.GetTripsByUser(1)).Returns(new Response<IList<Trip>>
                {Data = new List<Trip> {new() {StartDate = DateTime.Today, EndDate = DateTime.Today.AddDays(4)}}});

            var validationManager = new ValidationManager {TripManager = mockTripManager.Object};

            var clashingTripResponse =
                validationManager.DetermineIfClashingTripExists(1, DateTime.Today.AddDays(2), DateTime.Today.AddDays(3));

            Assert.AreEqual((uint) Ui.StatusCode.BadRequest, clashingTripResponse.StatusCode);
            Assert.AreEqual(
                $"{Ui.ErrorMessages.ClashingTripDates} {DateTime.Today.ToShortDateString()} to {DateTime.Today.AddDays(4).ToShortDateString()}.",
                clashingTripResponse.ErrorMessage);
        }
    }
}