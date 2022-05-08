using System;
using System.Collections.Generic;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using CapstoneDesktop.ViewModels;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReactiveUI;

namespace CapstoneTest.DesktopTests.ViewModels.TestEditTransportation
{
    [TestClass]
    public class TestEditTransportationCommand
    {
        [TestMethod]
        public void EditTransportationCommand_NullMethod_ReturnsErrorMessage()
        {
            Transportation transportation = new()
            {
                EndDate = DateTime.Today.AddDays(3),
                StartDate = DateTime.Today,
                Method = "Plane",
                TransportationId = 1,
                TripId = 1,
                Notes = "notes"
            };
            var mockTransportationManager = new Mock<TransportationManager>();
            var mockScreen = new Mock<IScreen>();
            EditTransportationPageViewModel editTransportationPageViewModel =
                new(transportation, mockScreen.Object) {TransportationManager = mockTransportationManager.Object};

            var testScheduler = new TestScheduler();

            editTransportationPageViewModel.Method = null;

            editTransportationPageViewModel.EditTransportationCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(Ui.ErrorMessages.EmptyTransportationMethod, editTransportationPageViewModel.ErrorMessage);
        }

        [TestMethod]
        public void EditTransportationCommand_InvalidDates_ReturnsErrorMessage()
        {
            Transportation transportation = new()
            {
                EndDate = DateTime.Today.AddDays(2),
                StartDate = DateTime.Today,
                Method = "Plane",
                TransportationId = 1,
                TripId = 1,
                Notes = "notes"
            };
            Transportation updatedTransportation = new()
            {
                EndDate = DateTime.Today,
                StartDate = DateTime.Today.AddDays(1),
                Method = "Plane",
                TransportationId = 1,
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
            var mockTransportationManager = new Mock<TransportationManager>();
            mockTransportationManager.Setup(um =>
                    um.EditTransportation(updatedTransportation))
                .Returns(new Response<bool> {Data = true});
            EditTransportationPageViewModel editTransportationPageViewModel =
                new(transportation, mockScreen.Object)
                {
                    ValidationManager = mockValidationManager.Object,
                    TransportationManager = mockTransportationManager.Object
                };

            var testScheduler = new TestScheduler();


            editTransportationPageViewModel.StartDate = DateTime.Today.AddDays(1);
            editTransportationPageViewModel.EndDate = DateTime.Today;

            editTransportationPageViewModel.EditTransportationCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(Ui.ErrorMessages.InvalidStartDate,
                editTransportationPageViewModel.ErrorMessage);
        }

        [TestMethod]
        public void EditTransportationCommand_NullDates_ReturnsErrorMessage()
        {
            Transportation transportation = new()
            {
                EndDate = DateTime.Today.AddDays(3),
                StartDate = DateTime.Today,
                Method = "Plane",
                TransportationId = 1,
                TripId = 1,
                Notes = "notes"
            };
            var mockScreen = new Mock<IScreen>();

            EditTransportationPageViewModel editTransportationPageViewModel =
                new(transportation, mockScreen.Object);

            var testScheduler = new TestScheduler();

            editTransportationPageViewModel.Method = "Plane";
            editTransportationPageViewModel.Notes = "notes";
            editTransportationPageViewModel.StartDate = null;
            editTransportationPageViewModel.StartTime = null;

            editTransportationPageViewModel.EditTransportationCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(Ui.ErrorMessages.InvalidEventDate,
                editTransportationPageViewModel.ErrorMessage);
        }


        [TestMethod]
        public void EditTransportationCommand_ClashingEvent_ReturnsErrorMessage()
        {
            Transportation transportation = new()
            {
                EndDate = DateTime.Today.AddDays(3),
                StartDate = DateTime.Today.AddDays(1),
                Method = "Plane",
                TransportationId = 1,
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
                        new Transportation {StartDate = DateTime.Today.AddDays(1), EndDate = DateTime.Today.AddDays(2)}
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


            EditTransportationPageViewModel editTransportationViewModel =
                new(transportation, mockScreen.Object)
                {
                    ValidationManager = mockValidationManager.Object
                };

            var testScheduler = new TestScheduler();

            editTransportationViewModel.Method = "Plane";
            editTransportationViewModel.StartDate = DateTime.Today.AddDays(1);
            editTransportationViewModel.StartTime = TimeSpan.Zero;
            editTransportationViewModel.EndDate = DateTime.Today.AddDays(3);
            editTransportationViewModel.EndTime = TimeSpan.Zero;

            editTransportationViewModel.EditTransportationCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(
                $"{Ui.ErrorMessages.ClashingEventDates} {DateTime.Today.AddDays(1)} to {DateTime.Today.AddDays(2)}.",
                editTransportationViewModel.ErrorMessage);
        }

        [TestMethod]
        public void EditTransportationCommand_InvalidEventDates_ReturnsErrorMessage()
        {
            var mockTrip = new Mock<Trip>
            {
                Object =
                {
                    StartDate = DateTime.Today.AddDays(-2),
                    EndDate = DateTime.Now
                }
            };
            Transportation transportation = new()
            {
                EndDate = DateTime.Today.AddDays(-3),
                StartDate = DateTime.Today,
                Method = "Plane",
                TransportationId = 1,
                TripId = 1
            };
            var mockTransportationManager = new Mock<TransportationManager>();
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


            EditTransportationPageViewModel editTransportationPageViewModel =
                new(transportation, mockScreen.Object)
                {
                    ValidationManager = mockValidationManager.Object,
                    TransportationManager = mockTransportationManager.Object
                };

            var testScheduler = new TestScheduler();

            editTransportationPageViewModel.Method = "Plane";
            editTransportationPageViewModel.StartDate = DateTime.Now.AddDays(-1);
            editTransportationPageViewModel.StartTime = TimeSpan.Zero;
            editTransportationPageViewModel.EndDate = DateTime.Now;
            editTransportationPageViewModel.EndTime = TimeSpan.Zero;

            editTransportationPageViewModel.EditTransportationCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(
                Ui.ErrorMessages.EventStartDateBeforeTripStartDate + mockTrip.Object.StartDate.ToShortDateString(),
                editTransportationPageViewModel.ErrorMessage);
        }

        [TestMethod]
        public void EditTransportationCommand_SuccessfulCreation()
        {
            Transportation transportation = new()
            {
                EndDate = DateTime.Today.AddDays(3),
                StartDate = DateTime.Today,
                Method = "Plane",
                TransportationId = 1,
                TripId = 1,
                Notes = "notes"
            };
            var mockTransportationManager = new Mock<TransportationManager>();
            mockTransportationManager.Setup(um => um.EditTransportation(transportation))
                .Returns(new Response<bool> {StatusCode = (uint) Ui.StatusCode.Success});
            var mockScreen = new Mock<IScreen>();
            EditTransportationPageViewModel editTransportationPageViewModel =
                new(transportation, mockScreen.Object) {TransportationManager = mockTransportationManager.Object};

            var testScheduler = new TestScheduler();

            editTransportationPageViewModel.Method = "Plane";
            editTransportationPageViewModel.StartDate = DateTime.Today;
            editTransportationPageViewModel.EndDate = DateTime.Today;
            editTransportationPageViewModel.StartTime = DateTime.Today.TimeOfDay;
            editTransportationPageViewModel.EndTime = DateTime.Today.TimeOfDay;

            editTransportationPageViewModel.EditTransportationCommand.ThrownExceptions.Subscribe();

            testScheduler.Start();

            Assert.AreEqual(string.Empty, editTransportationPageViewModel.ErrorMessage);
        }



    }
}