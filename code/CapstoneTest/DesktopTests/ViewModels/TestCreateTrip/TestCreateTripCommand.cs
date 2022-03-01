using System;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
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

            Assert.AreEqual(Ui.ErrorMessages.EmptyTripName, createTripWindowViewModel.ErrorMessage);
        }

        [TestMethod]
        public void CreateTrip_InvalidDates_ReturnsErrorMessage()
        {
            var mockUser = new Mock<User>();
            var mockTripManager = new Mock<TripManager>();
            var mockScreen = new Mock<IScreen>();
            CreateTripPageViewModel createTripWindowViewModel =
                new(mockUser.Object, mockTripManager.Object, mockScreen.Object);
            mockTripManager.Setup(um => um.CreateTrip(0, "name", "notes", DateTime.Today.AddDays(1), DateTime.Today))
                .Returns(new Response<int>
                { StatusCode = (uint)Ui.StatusCode.BadRequest, ErrorMessage = Ui.ErrorMessages.InvalidStartDate });
            var testScheduler = new TestScheduler();

            createTripWindowViewModel.TripName = "name";
            createTripWindowViewModel.Notes = "notes";
            createTripWindowViewModel.StartDate = DateTime.Today.AddDays(1);
            createTripWindowViewModel.EndDate = DateTime.Today;

            createTripWindowViewModel.CreateTripCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(Ui.ErrorMessages.InvalidStartDate,
                createTripWindowViewModel.ErrorMessage);
        }

        [TestMethod]
        public void CreateTrip_NullStartDate_ReturnsErrorMessage()
        {
            var mockUser = new Mock<User>();
            var mockTripManager = new Mock<TripManager>();
            var mockScreen = new Mock<IScreen>();
            var endDate = DateTime.Today.AddDays(1);
            CreateTripPageViewModel createTripWindowViewModel =
                new(mockUser.Object, mockTripManager.Object, mockScreen.Object);

            var testScheduler = new TestScheduler();

            createTripWindowViewModel.TripName = "name";
            createTripWindowViewModel.EndDate = endDate;
            createTripWindowViewModel.Notes = "notes";

            createTripWindowViewModel.CreateTripCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(Ui.ErrorMessages.NullTripDate, createTripWindowViewModel.ErrorMessage);
        }

        [TestMethod]
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
                .Returns(new Response<int> { StatusCode = (uint)Ui.StatusCode.Success });
            var testScheduler = new TestScheduler();

            createTripWindowViewModel.TripName = "name";
            createTripWindowViewModel.StartDate = startDate;
            createTripWindowViewModel.EndDate = endDate;
            createTripWindowViewModel.Notes = "notes";

            createTripWindowViewModel.CreateTripCommand.ThrownExceptions.Subscribe();

            testScheduler.Start();

            Assert.AreEqual(string.Empty, createTripWindowViewModel.ErrorMessage);
        }

    }
}