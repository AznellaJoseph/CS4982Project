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

            Assert.AreEqual(ErrorMessages.EmptyWaypointLocation, createWaypointWindowViewModel.ErrorMessage);
        }

        [TestMethod]
        public void CreateWaypointCommand_InvalidDates_ReturnsErrorMessage()
        {
            var mockTrip = new Mock<Trip>();
            var mockWaypointManager = new Mock<WaypointManager>();
            var mockScreen = new Mock<IScreen>();
            mockWaypointManager.Setup(um =>
                    um.CreateWaypoint(0, "Paris, Italy", DateTime.Today.AddDays(1) + TimeSpan.Zero,
                        DateTime.Today + TimeSpan.Zero, "notes"))
                .Returns(new Response<int> { ErrorMessage = ErrorMessages.InvalidStartDate });
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

            Assert.AreEqual(ErrorMessages.InvalidStartDate,
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

            Assert.AreEqual(ErrorMessages.NullDate,
                createWaypointWindowViewModel.ErrorMessage);
        }

        [TestMethod]
        public void CreateWaypointCommand_SuccessfulCreation()
        {
            var mockWaypointManager = new Mock<WaypointManager>();
            mockWaypointManager.Setup(um => um.CreateWaypoint(0, "Paris, Italy", DateTime.Today, DateTime.Today, null))
                .Returns(new Response<int> { StatusCode = 200 });
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