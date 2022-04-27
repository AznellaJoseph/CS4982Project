using System;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using CapstoneDesktop.ViewModels;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReactiveUI;

namespace CapstoneTest.DesktopTests.ViewModels.TestEditLodging
{
    [TestClass]
    public class TestEditLodgingCommand
    {
        [TestMethod]
        public void EditLodgingCommand_NullLocation_ReturnsErrorMessage()
        {
            Lodging lodging = new()
            {
                EndDate = DateTime.Today.AddDays(3),
                StartDate = DateTime.Today,
                Location = "Hilton",
                LodgingId = 1,
                TripId = 1,
                Notes = "notes"
            };
            var mockLodgingManager = new Mock<LodgingManager>();
            var mockScreen = new Mock<IScreen>();
            EditLodgingPageViewModel editLodgingPageViewModel =
                new(lodging, mockScreen.Object) {LodgingManager = mockLodgingManager.Object};

            var testScheduler = new TestScheduler();

            editLodgingPageViewModel.Location = null;

            editLodgingPageViewModel.EditLodgingCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(Ui.ErrorMessages.EmptyLocation, editLodgingPageViewModel.ErrorMessage);
        }

        [TestMethod]
        public void EditLodgingCommand_InvalidDates_ReturnsErrorMessage()
        {
            Lodging lodging = new()
            {
                EndDate = DateTime.Today.AddDays(2),
                StartDate = DateTime.Today,
                Location = "Paris, Italy",
                LodgingId = 1,
                TripId = 1,
                Notes = "notes"
            };
            Lodging updatedLodging = new()
            {
                EndDate = DateTime.Today,
                StartDate = DateTime.Today.AddDays(1),
                Location = "Paris, Italy",
                LodgingId = 1,
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
            var mockLodgingManager = new Mock<LodgingManager>();
            mockLodgingManager.Setup(um =>
                    um.EditLodging(updatedLodging))
                .Returns(new Response<bool> {Data = true});
            EditLodgingPageViewModel editLodgingPageViewModel =
                new(lodging, mockScreen.Object)
                {
                    ValidationManager = mockValidationManager.Object,
                    LodgingManager = mockLodgingManager.Object
                };

            var testScheduler = new TestScheduler();


            editLodgingPageViewModel.StartDate = DateTime.Today.AddDays(1);
            editLodgingPageViewModel.EndDate = DateTime.Today;

            editLodgingPageViewModel.EditLodgingCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(Ui.ErrorMessages.InvalidStartDate,
                editLodgingPageViewModel.ErrorMessage);
        }

        [TestMethod]
        public void EditLodgingCommand_NullDates_ReturnsErrorMessage()
        {
            Lodging lodging = new()
            {
                EndDate = DateTime.Today.AddDays(3),
                StartDate = DateTime.Today,
                Location = "Hilton",
                LodgingId = 1,
                TripId = 1,
                Notes = "notes"
            };
            var mockScreen = new Mock<IScreen>();

            EditLodgingPageViewModel editLodgingPageViewModel =
                new(lodging, mockScreen.Object);

            var testScheduler = new TestScheduler();

            editLodgingPageViewModel.Location = "Paris, Italy";
            editLodgingPageViewModel.Notes = "notes";
            editLodgingPageViewModel.StartDate = null;
            editLodgingPageViewModel.StartTime = null;

            editLodgingPageViewModel.EditLodgingCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(Ui.ErrorMessages.InvalidEventDate,
                editLodgingPageViewModel.ErrorMessage);
        }


        [TestMethod]
        public void EditLodgingCommand_InvalidEventDates_ReturnsErrorMessage()
        {
            var mockTrip = new Mock<Trip>
            {
                Object =
                {
                    StartDate = DateTime.Today.AddDays(-2),
                    EndDate = DateTime.Now
                }
            };
            Lodging lodging = new()
            {
                EndDate = DateTime.Today.AddDays(-3),
                StartDate = DateTime.Today,
                Location = "UWG library, Carrollton",
                LodgingId = 1,
                TripId = 1
            };
            var mockLodgingManager = new Mock<LodgingManager>();
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


            EditLodgingPageViewModel editLodgingPageViewModel =
                new(lodging, mockScreen.Object)
                {
                    ValidationManager = mockValidationManager.Object,
                    LodgingManager = mockLodgingManager.Object
                };

            var testScheduler = new TestScheduler();

            editLodgingPageViewModel.Location = "UWG library, Carrollton";
            editLodgingPageViewModel.StartDate = DateTime.Now.AddDays(-1);
            editLodgingPageViewModel.StartTime = TimeSpan.Zero;
            editLodgingPageViewModel.EndDate = DateTime.Now;
            editLodgingPageViewModel.EndTime = TimeSpan.Zero;

            editLodgingPageViewModel.EditLodgingCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(
                Ui.ErrorMessages.EventStartDateBeforeTripStartDate + mockTrip.Object.StartDate.ToShortDateString(),
                editLodgingPageViewModel.ErrorMessage);
        }

        [TestMethod]
        public void EditLodgingCommand_SuccessfulCreation()
        {
            Lodging lodging = new()
            {
                EndDate = DateTime.Today.AddDays(3),
                StartDate = DateTime.Today,
                Location = "Hilton",
                LodgingId = 1,
                TripId = 1,
                Notes = "notes"
            };
            var mockLodgingManager = new Mock<LodgingManager>();
            mockLodgingManager.Setup(um => um.EditLodging(lodging))
                .Returns(new Response<bool> {StatusCode = (uint) Ui.StatusCode.Success});
            var mockValidationManager = new Mock<ValidationManager>();
            mockValidationManager
                .Setup(vm => vm.DetermineIfValidEventDates(1, DateTime.Today, DateTime.Today.AddDays(3)))
                .Returns(new Response<bool> {Data = true});
            var mockScreen = new Mock<IScreen>();

            EditLodgingPageViewModel editLodgingPageViewModel =
                new(lodging, mockScreen.Object) {ValidationManager = mockValidationManager.Object, LodgingManager = mockLodgingManager.Object};

            var testScheduler = new TestScheduler();

            editLodgingPageViewModel.Location = "Paris, Italy";

            editLodgingPageViewModel.EditLodgingCommand.ThrownExceptions.Subscribe();

            testScheduler.Start();

            Assert.AreEqual(string.Empty, editLodgingPageViewModel.ErrorMessage);
        }
    }
}