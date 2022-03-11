using System;
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
                new(mockTrip.Object, mockTransportationManager.Object, mockScreen.Object);

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
                new(mockTrip.Object, mockTransportationManager.Object, mockScreen.Object);

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
                new(mockTrip.Object, mockTransportationManager.Object, mockScreen.Object);

            var testScheduler = new TestScheduler();

            createTransportationViewModel.Method = "Plane";
            createTransportationViewModel.StartDate = DateTimeOffset.Now.AddDays(-2);
            createTransportationViewModel.StartTime = TimeSpan.Zero;

            createTransportationViewModel.CreateTransportationCommand.ThrownExceptions.Subscribe();

            testScheduler.Start();

            Assert.AreEqual(string.Empty, createTransportationViewModel.ErrorMessage);
        }

        [TestMethod]
        public void CreateTransportationCommand_EventStartBeforeTripStart_ReturnsErrorMessage()
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
                new(mockTrip.Object, mockTransportationManager.Object, mockScreen.Object)
                {
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
        public void CreateTransportationCommand_EventEndBeforeTripStart_ReturnsErrorMessage()
        {
            var mockTrip = new Mock<Trip>
            {
                Object =
                {
                    StartDate = DateTime.Today.AddDays(-2),
                    EndDate = DateTime.Today
                }
            };
            var mockTransportationManager = new Mock<TransportationManager>();
            var mockScreen = new Mock<IScreen>();
            var mockValidationManager = new Mock<ValidationManager>();
            mockValidationManager.Setup(vm => vm.DetermineIfValidEventDates(0,
                    DateTime.Today.AddDays(-1) + TimeSpan.Zero, DateTime.Today.AddDays(-3) + TimeSpan.Zero))
                .Returns(new Response<bool>
                {
                    ErrorMessage = Ui.ErrorMessages.EventEndDateBeforeTripStartDate +
                                   mockTrip.Object.StartDate.ToShortDateString(),
                    StatusCode = (uint) Ui.StatusCode.BadRequest
                });


            CreateTransportationPageViewModel createTransportationViewModel =
                new(mockTrip.Object, mockTransportationManager.Object, mockScreen.Object)
                {
                    ValidationManager = mockValidationManager.Object
                };

            var testScheduler = new TestScheduler();

            createTransportationViewModel.Method = "Plane";
            createTransportationViewModel.StartDate = DateTime.Today.AddDays(-1);
            createTransportationViewModel.StartTime = TimeSpan.Zero;
            createTransportationViewModel.EndDate = DateTime.Today.AddDays(-3);
            createTransportationViewModel.EndTime = TimeSpan.Zero;

            createTransportationViewModel.CreateTransportationCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(
                Ui.ErrorMessages.EventEndDateBeforeTripStartDate + mockTrip.Object.StartDate.ToShortDateString(),
                createTransportationViewModel.ErrorMessage);
        }

        [TestMethod]
        public void CreateTransportationCommand_EventStartAfterTripEnd_ReturnsErrorMessage()
        {
            var mockTrip = new Mock<Trip>
            {
                Object =
                {
                    StartDate = DateTime.Today.AddDays(-2),
                    EndDate = DateTime.Today
                }
            };
            var mockTransportationManager = new Mock<TransportationManager>();
            var mockScreen = new Mock<IScreen>();
            var mockValidationManager = new Mock<ValidationManager>();
            mockValidationManager.Setup(vm =>
                    vm.DetermineIfValidEventDates(0, DateTime.Today.AddDays(1) + TimeSpan.Zero,
                        DateTime.Today + TimeSpan.Zero))
                .Returns(new Response<bool>
                {
                    ErrorMessage = Ui.ErrorMessages.EventStartDateAfterTripEndDate +
                                   mockTrip.Object.EndDate.ToShortDateString(),
                    StatusCode = (uint) Ui.StatusCode.BadRequest
                });


            CreateTransportationPageViewModel createTransportationViewModel =
                new(mockTrip.Object, mockTransportationManager.Object, mockScreen.Object)
                {
                    ValidationManager = mockValidationManager.Object
                };

            var testScheduler = new TestScheduler();

            createTransportationViewModel.Method = "Plane";
            createTransportationViewModel.StartDate = DateTimeOffset.Now.AddDays(1);
            createTransportationViewModel.StartTime = TimeSpan.Zero;
            createTransportationViewModel.EndDate = DateTime.Today;
            createTransportationViewModel.EndTime = TimeSpan.Zero;

            createTransportationViewModel.CreateTransportationCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(
                Ui.ErrorMessages.EventStartDateAfterTripEndDate + mockTrip.Object.EndDate.ToShortDateString(),
                createTransportationViewModel.ErrorMessage);
        }

        [TestMethod]
        public void CreateTransportationCommand_EventEndAfterTripEnd_ReturnsErrorMessage()
        {
            var mockTrip = new Mock<Trip>
            {
                Object =
                {
                    StartDate = DateTime.Today.AddDays(-2),
                    EndDate = DateTime.Today
                }
            };
            var mockTransportationManager = new Mock<TransportationManager>();
            var mockScreen = new Mock<IScreen>();
            var mockValidationManager = new Mock<ValidationManager>();
            mockValidationManager.Setup(vm => vm.DetermineIfValidEventDates(0,
                    DateTime.Today.AddDays(-1) + TimeSpan.Zero, DateTime.Today.AddDays(3) + TimeSpan.Zero))
                .Returns(new Response<bool>
                {
                    ErrorMessage = Ui.ErrorMessages.EventEndDateAfterTripEndDate +
                                   mockTrip.Object.EndDate.ToShortDateString(),
                    StatusCode = (uint) Ui.StatusCode.BadRequest
                });


            CreateTransportationPageViewModel createTransportationViewModel =
                new(mockTrip.Object, mockTransportationManager.Object, mockScreen.Object)
                {
                    ValidationManager = mockValidationManager.Object
                };

            var testScheduler = new TestScheduler();

            createTransportationViewModel.Method = "Plane";
            createTransportationViewModel.StartDate = DateTimeOffset.Now.AddDays(-1);
            createTransportationViewModel.StartTime = TimeSpan.Zero;
            createTransportationViewModel.EndDate = DateTime.Today.AddDays(3);
            createTransportationViewModel.EndTime = TimeSpan.Zero;

            createTransportationViewModel.CreateTransportationCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(Ui.ErrorMessages.EventEndDateAfterTripEndDate + mockTrip.Object.EndDate.ToShortDateString(),
                createTransportationViewModel.ErrorMessage);
        }

        [TestMethod]
        public void CreateTransportationCommand_SuccessfulCreation()
        {
            var mockTransportationManager = new Mock<TransportationManager>();
            mockTransportationManager.Setup(um => um.CreateTransportation(0, "Plane",
                    DateTime.Today + DateTime.Today.TimeOfDay, DateTime.Today + DateTime.Today.TimeOfDay, null))
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
                new(mockTrip.Object, mockTransportationManager.Object, mockScreen.Object);

            var testScheduler = new TestScheduler();

            createTransportationViewModel.Method = "Plane";
            createTransportationViewModel.StartDate = DateTime.Today;
            createTransportationViewModel.StartTime = DateTime.Today.TimeOfDay;
            createTransportationViewModel.EndDate = DateTime.Today;
            createTransportationViewModel.EndTime = DateTime.Today.TimeOfDay;

            createTransportationViewModel.CreateTransportationCommand.ThrownExceptions.Subscribe();

            testScheduler.Start();

            Assert.AreEqual(string.Empty, createTransportationViewModel.ErrorMessage);
        }
    }
}