using System;
using CapstoneBackend.Model;
using CapstoneDesktop.ViewModels;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CapstoneTest.DesktopTests.ViewModels.TestCreateTrip
{
    [TestClass]
    public class TestCreateTripCommand
    {
        [TestMethod]
        public void CreateTrip_EmptyTripName_ReturnsErrorMessage()
        {
            var mockTripManager = new Mock<TripManager>();

            CreateTripViewModel createTripWindowViewModel = new(mockTripManager.Object);
            var testScheduler = new TestScheduler();

            createTripWindowViewModel.TripName = "";

            createTripWindowViewModel.CreateTripCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual("You must enter a name for the trip", createTripWindowViewModel.ErrorMessage);
        }

        [TestMethod]
        public void CreateTrip_InvalidDates_ReturnsErrorMessage()
        {
            var mockTripManager = new Mock<TripManager>();
            mockTripManager.Setup(um => um.CreateTrip(0, "name", null, DateTime.Today.AddDays(1), DateTime.Today))
                .Returns(new Response<int>
                    {StatusCode = 400, ErrorMessage = "Start date of a trip cannot be after the end date."});

            CreateTripViewModel createTripWindowViewModel = new(mockTripManager.Object);
            var testScheduler = new TestScheduler();

            createTripWindowViewModel.TripName = "name";
            createTripWindowViewModel.StartDate = DateTime.Today.AddDays(1);
            createTripWindowViewModel.EndDate = DateTime.Today;

            createTripWindowViewModel.CreateTripCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual("Start date of a trip cannot be after the end date.",
                createTripWindowViewModel.ErrorMessage);
        }

        [TestMethod]
        public void CreateTrip_SuccessfulCreation()
        {
            var mockTripManager = new Mock<TripManager>();
            mockTripManager.Setup(um => um.CreateTrip(0, "name", "notes", DateTime.Today, DateTime.Today.AddDays(1)))
                .Returns(new Response<int> {StatusCode = 200});

            CreateTripViewModel createTripWindowViewModel = new(mockTripManager.Object);
            var testScheduler = new TestScheduler();

            createTripWindowViewModel.TripName = "name";
            createTripWindowViewModel.Notes = "notes";

            createTripWindowViewModel.CreateTripCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(string.Empty, createTripWindowViewModel.ErrorMessage);
        }
    }
}