using System;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using CapstoneDesktop.ViewModels;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReactiveUI;

namespace CapstoneTest.DesktopTests.ViewModels.TestCreateWaypoint
{
    [TestClass]
    public class TestCreateWaypointCommand
    {
        [TestMethod]
        public void CreateWaypointCommand_NullLocation_ReturnsErrorMessage()
        {
            var mockTrip = new Mock<Trip>();
            var mockWaypointManager = new Mock<WaypointManager>();
            var mockScreen = new Mock<IScreen>();
            CreateWaypointPageViewModel createWaypointWindowViewModel =
                new(mockTrip.Object, mockWaypointManager.Object, mockScreen.Object);

            var testScheduler = new TestScheduler();

            createWaypointWindowViewModel.CreateWaypointCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(Ui.ErrorMessages.EmptyWaypointLocation, createWaypointWindowViewModel.ErrorMessage);
        }

        [TestMethod]
        public void CreateWaypointCommand_InvalidDates_ReturnsErrorMessage()
        {
            var mockTrip = new Mock<Trip>();
            mockTrip.Object.StartDate = DateTime.Today;
            mockTrip.Object.EndDate = DateTime.Today.AddDays(2);
            var mockWaypointManager = new Mock<WaypointManager>();
            var mockScreen = new Mock<IScreen>();
            mockWaypointManager.Setup(um =>
                    um.CreateWaypoint(0, "Paris, Italy", DateTime.Today.AddDays(1) + TimeSpan.Zero,
                        DateTime.Today + TimeSpan.Zero, "notes"))
                .Returns(new Response<int> {ErrorMessage = Ui.ErrorMessages.InvalidStartDate});
            CreateWaypointPageViewModel createWaypointWindowViewModel =
                new(mockTrip.Object, mockWaypointManager.Object, mockScreen.Object);

            var testScheduler = new TestScheduler();

            createWaypointWindowViewModel.Location = "Paris, Italy";
            createWaypointWindowViewModel.Notes = "notes";
            createWaypointWindowViewModel.StartDate = DateTime.Today.AddDays(1);
            createWaypointWindowViewModel.StartTime = TimeSpan.Zero;
            createWaypointWindowViewModel.EndDate = DateTime.Today;
            createWaypointWindowViewModel.EndTime = TimeSpan.Zero;

            createWaypointWindowViewModel.CreateWaypointCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(Ui.ErrorMessages.InvalidStartDate,
                createWaypointWindowViewModel.ErrorMessage);
        }

        [TestMethod]
        public void CreateWaypointCommand_NullDates_ReturnsErrorMessage()
        {
            var mockTrip = new Mock<Trip>();
            var mockWaypointManager = new Mock<WaypointManager>();
            var mockScreen = new Mock<IScreen>();

            CreateWaypointPageViewModel createWaypointWindowViewModel =
                new(mockTrip.Object, mockWaypointManager.Object, mockScreen.Object);

            var testScheduler = new TestScheduler();

            createWaypointWindowViewModel.Location = "Paris, Italy";
            createWaypointWindowViewModel.Notes = "notes";

            createWaypointWindowViewModel.CreateWaypointCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(Ui.ErrorMessages.InvalidEventDate,
                createWaypointWindowViewModel.ErrorMessage);
        }


        [TestMethod]
        public void CreateWaypointCommand_NullEndDate_SuccessfulCreation()
        {
            var mockTrip = new Mock<Trip>();
            mockTrip.Object.TripId = 0;
            mockTrip.Object.EndDate = DateTime.Today;
            var mockWaypointManager = new Mock<WaypointManager>();
            var mockScreen = new Mock<IScreen>();
            mockWaypointManager.Setup(um =>
                    um.CreateWaypoint(0, "Paris, Italy", DateTime.Today.AddDays(-2), DateTime.Today, "notes"))
                .Returns(new Response<int> {StatusCode = (uint) Ui.StatusCode.Success});
            CreateWaypointPageViewModel createWaypointWindowViewModel =
                new(mockTrip.Object, mockWaypointManager.Object, mockScreen.Object);

            var testScheduler = new TestScheduler();

            createWaypointWindowViewModel.Location = "Paris, Italy";
            createWaypointWindowViewModel.Notes = "notes";
            createWaypointWindowViewModel.StartDate = DateTimeOffset.Now.AddDays(-2);
            createWaypointWindowViewModel.StartTime = TimeSpan.Zero;
            createWaypointWindowViewModel.EndTime = TimeSpan.Zero;

            createWaypointWindowViewModel.CreateWaypointCommand.ThrownExceptions.Subscribe();

            testScheduler.Start();

            Assert.AreEqual(string.Empty,
                createWaypointWindowViewModel.ErrorMessage);
        }

        [TestMethod]
        public void CreateWaypointCommand_NullEndTime_SuccessfulCreation()
        {
            var mockTrip = new Mock<Trip>();
            mockTrip.Object.TripId = 0;
            mockTrip.Object.EndDate = DateTime.Today.AddHours(2);
            var mockWaypointManager = new Mock<WaypointManager>();
            var mockScreen = new Mock<IScreen>();
            mockWaypointManager.Setup(um =>
                    um.CreateWaypoint(0, "Paris, Italy", DateTime.Today.AddDays(-2), DateTime.Today, "notes"))
                .Returns(new Response<int> { StatusCode = (uint)Ui.StatusCode.Success });
            CreateWaypointPageViewModel createWaypointWindowViewModel =
                new(mockTrip.Object, mockWaypointManager.Object, mockScreen.Object);

            var testScheduler = new TestScheduler();

            createWaypointWindowViewModel.Location = "Paris, Italy";
            createWaypointWindowViewModel.Notes = "notes";
            createWaypointWindowViewModel.StartDate = DateTimeOffset.Now.AddDays(-2);
            createWaypointWindowViewModel.StartTime = TimeSpan.Zero;
            createWaypointWindowViewModel.EndDate = DateTime.Today;

            createWaypointWindowViewModel.CreateWaypointCommand.ThrownExceptions.Subscribe();

            testScheduler.Start();

            Assert.AreEqual(string.Empty,
                createWaypointWindowViewModel.ErrorMessage);
        }

        [TestMethod]
        public void CreateWaypointCommand_NullEndDateAndTime_SuccessfulCreation()
        {
            var mockTrip = new Mock<Trip>();
            mockTrip.Object.TripId = 0;
            mockTrip.Object.EndDate = DateTime.Today;
            var mockWaypointManager = new Mock<WaypointManager>();
            var mockScreen = new Mock<IScreen>();
            mockWaypointManager.Setup(um =>
                    um.CreateWaypoint(0, "Paris, Italy", DateTime.Today.AddDays(-2), DateTime.Today, "notes"))
                .Returns(new Response<int> { StatusCode = (uint)Ui.StatusCode.Success });
            CreateWaypointPageViewModel createWaypointWindowViewModel =
                new(mockTrip.Object, mockWaypointManager.Object, mockScreen.Object);

            var testScheduler = new TestScheduler();

            createWaypointWindowViewModel.Location = "Paris, Italy";
            createWaypointWindowViewModel.Notes = "notes";
            createWaypointWindowViewModel.StartDate = DateTimeOffset.Now.AddDays(-2);
            createWaypointWindowViewModel.StartTime = TimeSpan.Zero;

            createWaypointWindowViewModel.CreateWaypointCommand.ThrownExceptions.Subscribe();

            testScheduler.Start();

            Assert.AreEqual(string.Empty,
                createWaypointWindowViewModel.ErrorMessage);
        }


        [TestMethod]
        public void CreateWaypointCommand_InvalidEnteredDates_ReturnsErrorMessage()
        {
            var mockTrip = new Mock<Trip>();
            mockTrip.Object.StartDate = DateTime.Today.AddDays(-2);
            mockTrip.Object.EndDate = DateTime.Now;
            var mockWaypointManager = new Mock<WaypointManager>();
            var mockScreen = new Mock<IScreen>();

            CreateWaypointPageViewModel createWaypointWindowViewModel =
                new(mockTrip.Object, mockWaypointManager.Object, mockScreen.Object);

            var testScheduler = new TestScheduler();

            createWaypointWindowViewModel.Location = "Paris, Italy";
            createWaypointWindowViewModel.Notes = "notes";
            createWaypointWindowViewModel.StartDate = DateTimeOffset.Now.AddDays(-3);
            createWaypointWindowViewModel.StartTime = TimeSpan.Zero;
            createWaypointWindowViewModel.EndDate = DateTimeOffset.Now;
            createWaypointWindowViewModel.EndTime = TimeSpan.Zero;

            createWaypointWindowViewModel.CreateWaypointCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(
                Ui.ErrorMessages.EventStartDateBeforeTripStartDate + mockTrip.Object.StartDate.ToShortDateString(),
                createWaypointWindowViewModel.ErrorMessage);
        }

        [TestMethod]
        public void CreateWaypointCommand_EventStartBeforeTripStart_ReturnsErrorMessage()
        {
            var mockTrip = new Mock<Trip>();
            mockTrip.Object.StartDate = DateTime.Today.AddDays(-2);
            mockTrip.Object.EndDate = DateTime.Now;
            var mockWaypointManager = new Mock<WaypointManager>();
            var mockScreen = new Mock<IScreen>();

            CreateWaypointPageViewModel createWaypointViewModel =
                new(mockTrip.Object, mockWaypointManager.Object, mockScreen.Object);

            var testScheduler = new TestScheduler();

            createWaypointViewModel.Location = "Plane";
            createWaypointViewModel.StartDate = DateTimeOffset.Now.AddDays(-3);
            createWaypointViewModel.StartTime = TimeSpan.Zero;
            createWaypointViewModel.EndDate = DateTimeOffset.Now;
            createWaypointViewModel.EndTime = TimeSpan.Zero;

            createWaypointViewModel.CreateWaypointCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(
                Ui.ErrorMessages.EventStartDateBeforeTripStartDate + mockTrip.Object.StartDate.ToShortDateString(),
                createWaypointViewModel.ErrorMessage);
        }

        [TestMethod]
        public void CreateWaypointCommand_EventEndBeforeTripStart_ReturnsErrorMessage()
        {
            var mockTrip = new Mock<Trip>();
            mockTrip.Object.StartDate = DateTime.Today.AddDays(-2);
            mockTrip.Object.EndDate = DateTime.Now;
            var mockWaypointManager = new Mock<WaypointManager>();
            var mockScreen = new Mock<IScreen>();

            CreateWaypointPageViewModel createTransportationViewModel =
                new(mockTrip.Object, mockWaypointManager.Object, mockScreen.Object);

            var testScheduler = new TestScheduler();

            createTransportationViewModel.Location = "Airport";
            createTransportationViewModel.StartDate = DateTimeOffset.Now.AddDays(-1);
            createTransportationViewModel.StartTime = TimeSpan.Zero;
            createTransportationViewModel.EndDate = DateTime.Today.AddDays(-3);
            createTransportationViewModel.EndTime = TimeSpan.Zero;

            createTransportationViewModel.CreateWaypointCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(
                Ui.ErrorMessages.EventEndDateBeforeTripStartDate + mockTrip.Object.StartDate.ToShortDateString(),
                createTransportationViewModel.ErrorMessage);
        }

        [TestMethod]
        public void CreateWaypointCommand_EventStartAfterTripEnd_ReturnsErrorMessage()
        {
            var mockTrip = new Mock<Trip>();
            mockTrip.Object.StartDate = DateTime.Today.AddDays(-2);
            mockTrip.Object.EndDate = DateTime.Now;
            var mockWaypointManager = new Mock<WaypointManager>();
            var mockScreen = new Mock<IScreen>();

            CreateWaypointPageViewModel createTransportationViewModel =
                new(mockTrip.Object, mockWaypointManager.Object, mockScreen.Object);

            var testScheduler = new TestScheduler();

            createTransportationViewModel.Location = "Airport";
            createTransportationViewModel.StartDate = DateTimeOffset.Now.AddDays(1);
            createTransportationViewModel.StartTime = TimeSpan.Zero;
            createTransportationViewModel.EndDate = DateTime.Today;
            createTransportationViewModel.EndTime = TimeSpan.Zero;

            createTransportationViewModel.CreateWaypointCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(
                Ui.ErrorMessages.EventStartDateAfterTripEndDate + mockTrip.Object.EndDate.ToShortDateString(),
                createTransportationViewModel.ErrorMessage);
        }

        [TestMethod]
        public void CreateTransportationCommand_EventEndAfterTripEnd_ReturnsErrorMessage()
        {
            var mockTrip = new Mock<Trip>();
            mockTrip.Object.StartDate = DateTime.Today.AddDays(-2);
            mockTrip.Object.EndDate = DateTime.Now;
            var mockWaypointManager = new Mock<WaypointManager>();
            var mockScreen = new Mock<IScreen>();

            CreateWaypointPageViewModel createTransportationViewModel =
                new(mockTrip.Object, mockWaypointManager.Object, mockScreen.Object);

            var testScheduler = new TestScheduler();

            createTransportationViewModel.Location = "Airport";
            createTransportationViewModel.StartDate = DateTimeOffset.Now.AddDays(-1);
            createTransportationViewModel.StartTime = TimeSpan.Zero;
            createTransportationViewModel.EndDate = DateTime.Today.AddDays(3);
            createTransportationViewModel.EndTime = TimeSpan.Zero;

            createTransportationViewModel.CreateWaypointCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(Ui.ErrorMessages.EventEndDateAfterTripEndDate + mockTrip.Object.EndDate.ToShortDateString(),
                createTransportationViewModel.ErrorMessage);
        }


        [TestMethod]
        public void CreateWaypointCommand_SuccessfulCreation()
        {
            var mockWaypointManager = new Mock<WaypointManager>();
            mockWaypointManager.Setup(um => um.CreateWaypoint(0, "Paris, Italy", DateTime.Today, DateTime.Today, null))
                .Returns(new Response<int> {StatusCode = (uint) Ui.StatusCode.Success});
            var mockTrip = new Mock<Trip>();
            var mockScreen = new Mock<IScreen>();
            CreateWaypointPageViewModel createWaypointWindowViewModel =
                new(mockTrip.Object, mockWaypointManager.Object, mockScreen.Object);

            var testScheduler = new TestScheduler();

            createWaypointWindowViewModel.Location = "Paris, Italy";
            createWaypointWindowViewModel.StartDate = DateTime.Today;
            createWaypointWindowViewModel.EndDate = DateTime.Today;
            createWaypointWindowViewModel.StartTime = DateTime.Today.TimeOfDay;
            createWaypointWindowViewModel.EndTime = DateTime.Today.TimeOfDay;

            createWaypointWindowViewModel.CreateWaypointCommand.ThrownExceptions.Subscribe();

            testScheduler.Start();

            Assert.AreEqual(string.Empty, createWaypointWindowViewModel.ErrorMessage);
        }
    }
}