using System;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using CapstoneDesktop.ViewModels;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReactiveUI;

namespace CapstoneTest.DesktopTests.ViewModels.TestCreateLodging
{
    [TestClass]
    public class TestCreateLodgingCommand
    {
        [TestMethod]
        public void CreateLodgingCommand_NullLocation_ReturnsErrorMessage()
        {
            var mockTrip = new Mock<Trip>();
            var mockLodgingManager = new Mock<LodgingManager>();
            var mockScreen = new Mock<IScreen>();
            CreateLodgingPageViewModel createWaypointWindowViewModel =
                new(mockTrip.Object, mockScreen.Object) {LodgingManager = mockLodgingManager.Object};

            var testScheduler = new TestScheduler();

            createWaypointWindowViewModel.CreateLodgingCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(Ui.ErrorMessages.EmptyLocation, createWaypointWindowViewModel.ErrorMessage);
        }

        [TestMethod]
        public void CreateLodgingCommand_InvalidDates_ReturnsErrorMessage()
        {
            var mockTrip = new Mock<Trip>
            {
                Object =
                {
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddDays(2)
                }
            };
            var mockLodgingManager = new Mock<LodgingManager>();
            var mockScreen = new Mock<IScreen>();
            mockLodgingManager.Setup(um =>
                    um.CreateLodging(0, "Paris, Italy", DateTime.Today.AddDays(1) + TimeSpan.Zero,
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

            CreateLodgingPageViewModel createWaypointWindowViewModel =
                new(mockTrip.Object, mockScreen.Object)
                {
                    ValidationManager = mockValidationManager.Object,
                    LodgingManager = mockLodgingManager.Object
                };

            var testScheduler = new TestScheduler();

            createWaypointWindowViewModel.Location = "Paris, Italy";
            createWaypointWindowViewModel.Notes = "notes";
            createWaypointWindowViewModel.StartDate = DateTime.Today.AddDays(1);
            createWaypointWindowViewModel.StartTime = TimeSpan.Zero;
            createWaypointWindowViewModel.EndDate = DateTime.Today;
            createWaypointWindowViewModel.EndTime = TimeSpan.Zero;

            createWaypointWindowViewModel.CreateLodgingCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(Ui.ErrorMessages.InvalidStartDate,
                createWaypointWindowViewModel.ErrorMessage);
        }

        [TestMethod]
        public void CreateLodgingCommand_NullDates_ReturnsErrorMessage()
        {
            var mockTrip = new Mock<Trip>();
            var mockScreen = new Mock<IScreen>();

            CreateLodgingPageViewModel createWaypointWindowViewModel =
                new(mockTrip.Object, mockScreen.Object);

            var testScheduler = new TestScheduler();

            createWaypointWindowViewModel.Location = "Paris, Italy";
            createWaypointWindowViewModel.Notes = "notes";

            createWaypointWindowViewModel.CreateLodgingCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(Ui.ErrorMessages.InvalidEventDate,
                createWaypointWindowViewModel.ErrorMessage);
        }


        [TestMethod]
        public void CreateLodgingCommand_InvalidEventDates_ReturnsErrorMessage()
        {
            var mockTrip = new Mock<Trip>
            {
                Object =
                {
                    StartDate = DateTime.Today.AddDays(-2),
                    EndDate = DateTime.Now
                }
            };
            var mockLodgingManager = new Mock<LodgingManager>();
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


            CreateLodgingPageViewModel createWaypointViewModel =
                new(mockTrip.Object, mockScreen.Object)
                {
                    ValidationManager = mockValidationManager.Object,
                    LodgingManager = mockLodgingManager.Object
                };

            var testScheduler = new TestScheduler();

            createWaypointViewModel.Location = "Plane";
            createWaypointViewModel.StartDate = DateTime.Now.AddDays(-3);
            createWaypointViewModel.StartTime = TimeSpan.Zero;
            createWaypointViewModel.EndDate = DateTime.Now;
            createWaypointViewModel.EndTime = TimeSpan.Zero;

            createWaypointViewModel.CreateLodgingCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(
                Ui.ErrorMessages.EventStartDateBeforeTripStartDate + mockTrip.Object.StartDate.ToShortDateString(),
                createWaypointViewModel.ErrorMessage);
        }

        [TestMethod]
        public void CreateLodgingCommand_EventEndBeforeTripStart_ReturnsErrorMessage()
        {
            var mockTrip = new Mock<Trip>
            {
                Object =
                {
                    StartDate = DateTime.Today.AddDays(-2),
                    EndDate = DateTime.Now
                }
            };
            var mockLodgingManager = new Mock<LodgingManager>();
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


            CreateLodgingPageViewModel createWaypointViewModel =
                new(mockTrip.Object, mockScreen.Object)
                {
                    ValidationManager = mockValidationManager.Object,
                    LodgingManager = mockLodgingManager.Object
                };

            var testScheduler = new TestScheduler();

            createWaypointViewModel.Location = "Airport";
            createWaypointViewModel.StartDate = DateTimeOffset.Now.AddDays(-1);
            createWaypointViewModel.StartTime = TimeSpan.Zero;
            createWaypointViewModel.EndDate = DateTime.Today.AddDays(-3);
            createWaypointViewModel.EndTime = TimeSpan.Zero;

            createWaypointViewModel.CreateLodgingCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(
                Ui.ErrorMessages.EventEndDateBeforeTripStartDate + mockTrip.Object.StartDate.ToShortDateString(),
                createWaypointViewModel.ErrorMessage);
        }

        [TestMethod]
        public void CreateLodgingCommand_SuccessfulCreation()
        {
            var mockLodgingManager = new Mock<LodgingManager>();
            mockLodgingManager.Setup(um => um.CreateLodging(0, "Paris, Italy", DateTime.Today, DateTime.Today, null))
                .Returns(new Response<int> {StatusCode = (uint) Ui.StatusCode.Success});
            var mockTrip = new Mock<Trip>();
            var mockScreen = new Mock<IScreen>();
            CreateLodgingPageViewModel createWaypointWindowViewModel =
                new(mockTrip.Object, mockScreen.Object) {LodgingManager = mockLodgingManager.Object};

            var testScheduler = new TestScheduler();

            createWaypointWindowViewModel.Location = "Paris, Italy";
            createWaypointWindowViewModel.StartDate = DateTime.Today;
            createWaypointWindowViewModel.EndDate = DateTime.Today;
            createWaypointWindowViewModel.StartTime = DateTime.Today.TimeOfDay;
            createWaypointWindowViewModel.EndTime = DateTime.Today.TimeOfDay;

            createWaypointWindowViewModel.CreateLodgingCommand.ThrownExceptions.Subscribe();

            testScheduler.Start();

            Assert.AreEqual(string.Empty, createWaypointWindowViewModel.ErrorMessage);
        }
    }
}