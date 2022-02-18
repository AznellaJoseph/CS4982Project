using System;
using CapstoneBackend.Model;
using CapstoneDesktop.ViewModels;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReactiveUI;

namespace CapstoneTest.DesktopTests.ViewModels.TestCreateTrip
{
    [TestClass]
    public class TestCreateTripCommand
    {
        [TestMethod]
        public void CreateTrip_EmptyTripName_ReturnsErrorMessage()
        {
            var mockUser = new Mock<User>();
            var mockTripManager = new Mock<TripManager>();
            var mockScreen = new Mock<IScreen>();
            CreateTripPageViewModel createTripWindowViewModel =
                new(mockUser.Object, mockTripManager.Object, mockScreen.Object);
            var testScheduler = new TestScheduler();

            createTripWindowViewModel.TripName = "";

            createTripWindowViewModel.CreateTripCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual("You must enter a name for the trip.", createTripWindowViewModel.ErrorMessage);
        }

        [TestMethod]
        public void CreateTrip_InvalidDates_ReturnsErrorMessage()
        {
            var mockUser = new Mock<User>();
            var mockTripManager = new Mock<TripManager>();
            var mockScreen = new Mock<IScreen>();
            CreateTripPageViewModel createTripWindowViewModel =
                new(mockUser.Object, mockTripManager.Object, mockScreen.Object);
            mockTripManager.Setup(um => um.CreateTrip(0, "name", null, DateTime.Today.AddDays(1), DateTime.Today))
                .Returns(new Response<int>
                    {StatusCode = 400, ErrorMessage = "Start date of a trip cannot be after the end date."});
            var testScheduler = new TestScheduler();

            createTripWindowViewModel.TripName = "name";
            createTripWindowViewModel.StartDate = DateTime.Today.AddDays(1);
            createTripWindowViewModel.EndDate = DateTime.Today;

            createTripWindowViewModel.CreateTripCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual("Start date of a trip cannot be after the end date.",
                createTripWindowViewModel.ErrorMessage);
        }

        public void CreateTrip_SuccessfulCreation()
        {
            var mockUser = new Mock<User>();
            var mockTripManager = new Mock<TripManager>();
            var mockScreen = new Mock<IScreen>();
            var startDate = DateTime.Today;
            var endDate = DateTime.Today.AddDays(1);
            CreateTripPageViewModel createTripWindowViewModel =
                new(mockUser.Object, mockTripManager.Object, mockScreen.Object);
            mockTripManager.Setup(um => um.CreateTrip(0, "name", "notes", startDate, endDate))
                .Returns(new Response<int> {StatusCode = 200});
            var testScheduler = new TestScheduler();

            createTripWindowViewModel.TripName = "name";
            createTripWindowViewModel.StartDate = startDate;
            createTripWindowViewModel.EndDate = endDate;
            createTripWindowViewModel.Notes = "notes";

            createTripWindowViewModel.CreateTripCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(string.Empty, createTripWindowViewModel.ErrorMessage);
        }
    }
}