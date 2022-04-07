using System;
using System.Collections.Generic;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using CapstoneDesktop.ViewModels;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReactiveUI;

namespace CapstoneTest.DesktopTests.ViewModels.TestCreateTransportation
{
    [TestClass]
    public class TestCreateTransportationCommand
    {
        [TestMethod]
        public void CreateTransportationCommand_NullMethod_ReturnsErrorMessage()
        {
            var mockTrip = new Mock<Trip>();
            var mockTransportationManager = new Mock<TransportationManager>();
            var mockScreen = new Mock<IScreen>();

            CreateTransportationPageViewModel createTransportationViewModel =
                new(mockTrip.Object, mockScreen.Object) {TransportationManager = mockTransportationManager.Object};

            var testScheduler = new TestScheduler();

            createTransportationViewModel.CreateTransportationCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(Ui.ErrorMessages.EmptyTransportationMethod, createTransportationViewModel.ErrorMessage);
        }

        [TestMethod]
        public void CreateTransportationCommand_NullStartDate_ReturnsErrorMessage()
        {
            var mockTrip = new Mock<Trip>();
            var mockTransportationManager = new Mock<TransportationManager>();
            var mockScreen = new Mock<IScreen>();
            CreateTransportationPageViewModel createTransportationViewModel =
                new(mockTrip.Object, mockScreen.Object) {TransportationManager = mockTransportationManager.Object};

            var testScheduler = new TestScheduler();

            createTransportationViewModel.Method = "Plane";

            createTransportationViewModel.CreateTransportationCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(Ui.ErrorMessages.InvalidEventDate, createTransportationViewModel.ErrorMessage);
        }

        [TestMethod]
        public void CreateTransportationCommand_NullEndDate_SuccessfulCreation()
        {
            var mockTrip = new Mock<Trip>
            {
                Object =
                {
                    TripId = 0,
                    EndDate = DateTime.Today
                }
            };
            var mockTransportationManager = new Mock<TransportationManager>();
            var mockScreen = new Mock<IScreen>();
            mockTransportationManager.Setup(um =>
                    um.CreateTransportation(0, "Plane", DateTime.Today.AddDays(-2), DateTime.Today, null))
                .Returns(new Response<int> {StatusCode = (uint) Ui.StatusCode.Success});
            CreateTransportationPageViewModel createTransportationViewModel =
                new(mockTrip.Object, mockScreen.Object) {TransportationManager = mockTransportationManager.Object};

            var testScheduler = new TestScheduler();

            createTransportationViewModel.Method = "Plane";
            createTransportationViewModel.StartDate = DateTimeOffset.Now.AddDays(-2);
            createTransportationViewModel.StartTime = TimeSpan.Zero;

            createTransportationViewModel.CreateTransportationCommand.ThrownExceptions.Subscribe();

            testScheduler.Start();

            Assert.AreEqual(string.Empty, createTransportationViewModel.ErrorMessage);
        }

        [TestMethod]
        public void CreateTransportationCommand_InvalidEventDates_ReturnsErrorMessage()
        {
            var mockTrip = new Mock<Trip>
            {
                Object =
                {
                    TripId = 0,
                    StartDate = DateTime.Today.AddDays(-2),
                    EndDate = DateTime.Today
                }
            };
            var mockTransportationManager = new Mock<TransportationManager>();
            var mockScreen = new Mock<IScreen>();
            var mockValidationManager = new Mock<ValidationManager>();
            mockValidationManager.Setup(vm =>
                    vm.DetermineIfValidEventDates(0, DateTime.Today.AddDays(-3) + TimeSpan.Zero,
                        DateTime.Today + TimeSpan.Zero))
                .Returns(new Response<bool>
                {
                    ErrorMessage = Ui.ErrorMessages.EventStartDateBeforeTripStartDate +
                                   mockTrip.Object.StartDate.ToShortDateString(),
                    StatusCode = (uint) Ui.StatusCode.BadRequest
                });


            CreateTransportationPageViewModel createTransportationViewModel =
                new(mockTrip.Object, mockScreen.Object)
                {
                    TransportationManager = mockTransportationManager.Object,

                    ValidationManager = mockValidationManager.Object
                };

            var testScheduler = new TestScheduler();

            createTransportationViewModel.Method = "Plane";
            createTransportationViewModel.StartDate = DateTime.Today.AddDays(-3);
            createTransportationViewModel.StartTime = TimeSpan.Zero;
            createTransportationViewModel.EndDate = DateTime.Today;
            createTransportationViewModel.EndTime = TimeSpan.Zero;

            createTransportationViewModel.CreateTransportationCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(
                Ui.ErrorMessages.EventStartDateBeforeTripStartDate + mockTrip.Object.StartDate.ToShortDateString(),
                createTransportationViewModel.ErrorMessage);
        }

        [TestMethod]
        public void CreateTransportationCommand_ClashingEvent_ReturnsErrorMessage()
        {
            var mockTrip = new Mock<Trip>
            {
                Object =
                {
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddDays(4)
                }
            };
            var mockScreen = new Mock<IScreen>();
            var mockEventManager = new Mock<EventManager>();

            var mockValidationManager = new Mock<ValidationManager> {Object = {EventManager = mockEventManager.Object}};
            mockEventManager.Setup(em => em.GetEventsOnDate(0, DateTime.Today.AddDays(1))).Returns(
                new Response<IList<IEvent>>
                {
                    Data = new List<IEvent>
                    {
                        new Transportation {StartDate = DateTime.Today.AddDays(1), EndDate = DateTime.Today.AddDays(2)}
                    }
                });
            mockValidationManager
                .Setup(vm => vm.DetermineIfValidEventDates(0, DateTime.Today.AddDays(1), DateTime.Today.AddDays(3)))
                .Returns(new Response<bool> {Data = true});
            mockValidationManager.Setup(vm => vm.DetermineIfClashingEventExists(0,
                    DateTime.Today.AddDays(1) + TimeSpan.Zero, DateTime.Today.AddDays(3) + TimeSpan.Zero))
                .Returns(new Response<bool>
                {
                    ErrorMessage =
                        $"{Ui.ErrorMessages.ClashingEventDates} {DateTime.Today.AddDays(1)} to {DateTime.Today.AddDays(2)}."
                });


            CreateTransportationPageViewModel createTransportationViewModel =
                new(mockTrip.Object, mockScreen.Object)
                {
                    ValidationManager = mockValidationManager.Object
                };

            var testScheduler = new TestScheduler();

            createTransportationViewModel.Method = "Plane";
            createTransportationViewModel.StartDate = DateTime.Today.AddDays(1);
            createTransportationViewModel.StartTime = TimeSpan.Zero;
            createTransportationViewModel.EndDate = DateTime.Today.AddDays(3);
            createTransportationViewModel.EndTime = TimeSpan.Zero;

            createTransportationViewModel.CreateTransportationCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(
                $"{Ui.ErrorMessages.ClashingEventDates} {DateTime.Today.AddDays(1)} to {DateTime.Today.AddDays(2)}.",
                createTransportationViewModel.ErrorMessage);
        }

        [TestMethod]
        public void CreateTransportationCommand_InvalidStartDate_ReturnsErrorMessage()
        {
            var mockTrip = new Mock<Trip>
            {
                Object =
                {
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddDays(2)
                }
            };
            var mockTransportationManager = new Mock<TransportationManager>();
            var mockScreen = new Mock<IScreen>();
            mockTransportationManager.Setup(um =>
                    um.CreateTransportation(0, "taxi", DateTime.Today.AddDays(1) + TimeSpan.Zero,
                        DateTime.Today + TimeSpan.Zero, "notes"))
                .Returns(new Response<int> {ErrorMessage = Ui.ErrorMessages.InvalidStartDate});
            var mockValidationManager = new Mock<ValidationManager>();
            mockValidationManager.Setup(vm =>
                    vm.DetermineIfValidEventDates(0, DateTime.Today.AddDays(1) + TimeSpan.Zero,
                        DateTime.Today + TimeSpan.Zero))
                .Returns(new Response<bool>
                {
                    Data = true
                });
            mockValidationManager.Setup(vm => vm.DetermineIfClashingEventExists(0, DateTime.Today.AddDays(1), DateTime.Today))
                .Returns(new Response<bool> {Data = false});

            CreateTransportationPageViewModel createTransportationWindowViewModel =
                new(mockTrip.Object, mockScreen.Object)
                {
                    ValidationManager = mockValidationManager.Object,
                    TransportationManager = mockTransportationManager.Object
                };

            var testScheduler = new TestScheduler();

            createTransportationWindowViewModel.Method = "taxi";
            createTransportationWindowViewModel.Notes = "notes";
            createTransportationWindowViewModel.StartDate = DateTime.Today.AddDays(1);
            createTransportationWindowViewModel.StartTime = TimeSpan.Zero;
            createTransportationWindowViewModel.EndDate = DateTime.Today;
            createTransportationWindowViewModel.EndTime = TimeSpan.Zero;

            createTransportationWindowViewModel.CreateTransportationCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(Ui.ErrorMessages.InvalidStartDate,
                createTransportationWindowViewModel.ErrorMessage);
        }

        [TestMethod]
        public void CreateTransportationCommand_Success()
        {
            var mockTransportationManager = new Mock<TransportationManager>();
            mockTransportationManager.Setup(um => um.CreateTransportation(0, "Plane",
                    DateTime.Today + DateTime.Today.TimeOfDay, DateTime.Today + DateTime.Today.TimeOfDay, "notes"))
                .Returns(new Response<int> {StatusCode = (uint) Ui.StatusCode.Success});
            var mockTrip = new Mock<Trip>
            {
                Object =
                {
                    TripId = 0,
                    StartDate = DateTime.Today.AddDays(-2),
                    EndDate = DateTime.Today.AddDays(3)
                }
            };
            var mockScreen = new Mock<IScreen>();
            CreateTransportationPageViewModel createTransportationViewModel =
                new(mockTrip.Object, mockScreen.Object) {TransportationManager = mockTransportationManager.Object};

            var testScheduler = new TestScheduler();

            createTransportationViewModel.Method = "Plane";
            createTransportationViewModel.StartDate = DateTime.Today;
            createTransportationViewModel.StartTime = DateTime.Today.TimeOfDay;
            createTransportationViewModel.EndDate = DateTime.Today;
            createTransportationViewModel.EndTime = DateTime.Today.TimeOfDay;
            createTransportationViewModel.Notes = "notes";

            createTransportationViewModel.CreateTransportationCommand.ThrownExceptions.Subscribe();

            testScheduler.Start();

            Assert.AreEqual(string.Empty, createTransportationViewModel.ErrorMessage);
        }
    }
}