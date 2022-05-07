using System;
using System.Collections.Generic;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using CapstoneDesktop.ViewModels;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReactiveUI;

namespace CapstoneTest.DesktopTests.ViewModels.TestEditWaypoint
{
    [TestClass]
    public class TestEditWaypointCommand
    {
        [TestMethod]
        public void EditWaypointCommand_NullLocation_ReturnsErrorMessage()
        {
            Waypoint waypoint = new()
            {
                EndDate = DateTime.Today.AddDays(3),
                StartDate = DateTime.Today,
                Location = "Hilton",
                WaypointId = 1,
                TripId = 1,
                Notes = "notes"
            };
            var mockWaypointManager = new Mock<WaypointManager>();
            var mockScreen = new Mock<IScreen>();
            EditWaypointPageViewModel editWaypointPageViewModel =
                new(waypoint, mockScreen.Object) {WaypointManager = mockWaypointManager.Object};

            var testScheduler = new TestScheduler();

            editWaypointPageViewModel.Location = string.Empty;

            editWaypointPageViewModel.EditWaypointCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(Ui.ErrorMessages.EmptyLocation, editWaypointPageViewModel.ErrorMessage);
        }

        [TestMethod]
        public void EditWaypointCommand_InvalidDates_ReturnsErrorMessage()
        {
            Waypoint waypoint = new()
            {
                EndDate = DateTime.Today.AddDays(2),
                StartDate = DateTime.Today,
                Location = "Paris, Italy",
                WaypointId = 1,
                TripId = 1,
                Notes = "notes"
            };
            Waypoint updatedWaypoint = new()
            {
                EndDate = DateTime.Today,
                StartDate = DateTime.Today.AddDays(1),
                Location = "Paris, Italy",
                WaypointId = 1,
                TripId = 1,
                Notes = "notes"
            };
            var mockScreen = new Mock<IScreen>();


            var mockValidationManager = new Mock<ValidationManager>();
            mockValidationManager.Setup(vm =>
                    vm.DetermineIfValidEventDates(1, DateTime.Today.AddDays(1),
                        DateTime.Today))
                .Returns(new Response<bool>
                {
                    Data = true
                });
            var mockWaypointManager = new Mock<WaypointManager>();
            mockWaypointManager.Setup(um =>
                    um.EditWaypoint(updatedWaypoint))
                .Returns(new Response<bool> {Data = true});
            EditWaypointPageViewModel editWaypointPageViewModel =
                new(waypoint, mockScreen.Object)
                {
                    ValidationManager = mockValidationManager.Object,
                    WaypointManager = mockWaypointManager.Object
                };

            var testScheduler = new TestScheduler();


            editWaypointPageViewModel.StartDate = DateTime.Today.AddDays(1);
            editWaypointPageViewModel.EndDate = DateTime.Today;

            editWaypointPageViewModel.EditWaypointCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(Ui.ErrorMessages.InvalidStartDate,
                editWaypointPageViewModel.ErrorMessage);
        }

        [TestMethod]
        public void EditWaypointCommand_NullDates_ReturnsErrorMessage()
        {
            Waypoint waypoint = new()
            {
                EndDate = DateTime.Today.AddDays(3),
                StartDate = DateTime.Today,
                Location = "Hilton",
                WaypointId = 1,
                TripId = 1,
                Notes = "notes"
            };
            var mockScreen = new Mock<IScreen>();

            EditWaypointPageViewModel editWaypointPageViewModel =
                new(waypoint, mockScreen.Object);

            var testScheduler = new TestScheduler();

            editWaypointPageViewModel.Location = "Paris, Italy";
            editWaypointPageViewModel.Notes = "notes";
            editWaypointPageViewModel.StartDate = null;
            editWaypointPageViewModel.StartTime = null;

            editWaypointPageViewModel.EditWaypointCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(Ui.ErrorMessages.InvalidEventDate,
                editWaypointPageViewModel.ErrorMessage);
        }


        [TestMethod]
        public void EditWaypointCommand_InvalidEventDates_ReturnsErrorMessage()
        {
            var mockTrip = new Mock<Trip>
            {
                Object =
                {
                    StartDate = DateTime.Today.AddDays(-2),
                    EndDate = DateTime.Now
                }
            };
            Waypoint waypoint = new()
            {
                EndDate = DateTime.Today.AddDays(-3),
                StartDate = DateTime.Today,
                Location = "UWG library, Carrollton",
                WaypointId = 1,
                TripId = 1
            };
            var mockWaypointManager = new Mock<WaypointManager>();
            var mockScreen = new Mock<IScreen>();
            var mockValidationManager = new Mock<ValidationManager>();
            mockValidationManager.Setup(vm =>
                    vm.DetermineIfValidEventDates(1, DateTime.Today.AddDays(-1) + TimeSpan.Zero,
                        DateTime.Today + TimeSpan.Zero))
                .Returns(new Response<bool>
                {
                    ErrorMessage = Ui.ErrorMessages.EventStartDateBeforeTripStartDate +
                                   mockTrip.Object.StartDate.ToShortDateString(),
                    StatusCode = (uint) Ui.StatusCode.BadRequest
                });


            EditWaypointPageViewModel editWaypointPageViewModel =
                new(waypoint, mockScreen.Object)
                {
                    ValidationManager = mockValidationManager.Object,
                    WaypointManager = mockWaypointManager.Object
                };

            var testScheduler = new TestScheduler();

            editWaypointPageViewModel.Location = "UWG library, Carrollton";
            editWaypointPageViewModel.StartDate = DateTime.Now.AddDays(-1);
            editWaypointPageViewModel.StartTime = TimeSpan.Zero;
            editWaypointPageViewModel.EndDate = DateTime.Now;
            editWaypointPageViewModel.EndTime = TimeSpan.Zero;

            editWaypointPageViewModel.EditWaypointCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(
                Ui.ErrorMessages.EventStartDateBeforeTripStartDate + mockTrip.Object.StartDate.ToShortDateString(),
                editWaypointPageViewModel.ErrorMessage);
        }

        [TestMethod]
        public void EditWaypointCommand_ClashingEvent_ReturnsErrorMessage()
        {
            Waypoint waypoint = new()
            {
                EndDate = DateTime.Today.AddDays(3),
                StartDate = DateTime.Today.AddDays(1),
                Location = "UWG Library, Carrollton",
                WaypointId = 1,
                TripId = 1,
                Notes = "notes"
            };
            var mockScreen = new Mock<IScreen>();
            var mockEventManager = new Mock<EventManager>();

            var mockValidationManager = new Mock<ValidationManager> { Object = { EventManager = mockEventManager.Object } };
            mockEventManager.Setup(em => em.GetEventsOnDate(1, DateTime.Today.AddDays(1))).Returns(
                new Response<IList<IEvent>>
                {
                    Data = new List<IEvent>
                    {
                        new Waypoint {StartDate = DateTime.Today.AddDays(1), EndDate = DateTime.Today.AddDays(2)}
                    }
                });
            mockValidationManager
                .Setup(vm => vm.DetermineIfValidEventDates(1, DateTime.Today.AddDays(1), DateTime.Today.AddDays(3)))
                .Returns(new Response<bool> { Data = true });
            mockValidationManager.Setup(vm => vm.FindClashingEvents(1,
                    DateTime.Today.AddDays(1) + TimeSpan.Zero, DateTime.Today.AddDays(3) + TimeSpan.Zero, null))
                .Returns(new Response<IList<IEvent>>
                {
                    ErrorMessage =
                        $"{Ui.ErrorMessages.ClashingEventDates} {DateTime.Today.AddDays(1)} to {DateTime.Today.AddDays(2)}."
                });


            EditWaypointPageViewModel editWaypointViewModel =
                new(waypoint, mockScreen.Object)
                {
                    ValidationManager = mockValidationManager.Object
                };

            var testScheduler = new TestScheduler();

            editWaypointViewModel.Location = "UWG Library, Carrollton";
            editWaypointViewModel.StartDate = DateTime.Today.AddDays(1);
            editWaypointViewModel.StartTime = TimeSpan.Zero;
            editWaypointViewModel.EndDate = DateTime.Today.AddDays(3);
            editWaypointViewModel.EndTime = TimeSpan.Zero;

            editWaypointViewModel.EditWaypointCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(
                $"{Ui.ErrorMessages.ClashingEventDates} {DateTime.Today.AddDays(1)} to {DateTime.Today.AddDays(2)}.",
                editWaypointViewModel.ErrorMessage);
        }

        [TestMethod]
        public void EditWaypointCommand_SuccessfulCreation()
        {
            Waypoint waypoint = new()
            {
                EndDate = DateTime.Today.AddDays(3),
                StartDate = DateTime.Today,
                Location = "Hilton",
                WaypointId = 1,
                TripId = 1,
                Notes = "notes"
            };
            var mockWaypointManager = new Mock<WaypointManager>();
            mockWaypointManager.Setup(um => um.EditWaypoint(waypoint))
                .Returns(new Response<bool> {StatusCode = (uint) Ui.StatusCode.Success});
            var mockScreen = new Mock<IScreen>();
            EditWaypointPageViewModel editWaypointPageViewModel =
                new(waypoint, mockScreen.Object) {WaypointManager = mockWaypointManager.Object};

            var testScheduler = new TestScheduler();

            editWaypointPageViewModel.Location = "Paris, Italy";
            editWaypointPageViewModel.StartDate = DateTime.Today;
            editWaypointPageViewModel.EndDate = DateTime.Today;
            editWaypointPageViewModel.StartTime = DateTime.Today.TimeOfDay;
            editWaypointPageViewModel.EndTime = DateTime.Today.TimeOfDay;

            editWaypointPageViewModel.EditWaypointCommand.ThrownExceptions.Subscribe();

            testScheduler.Start();

            Assert.AreEqual(string.Empty, editWaypointPageViewModel.ErrorMessage);
        }
    }
}