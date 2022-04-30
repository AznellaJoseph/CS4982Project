using System;
using System.Threading.Tasks;
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
            CreateLodgingPageViewModel createLodgingPageViewModel =
                new(mockTrip.Object, mockScreen.Object) {LodgingManager = mockLodgingManager.Object};

            var testScheduler = new TestScheduler();

            createLodgingPageViewModel.CreateLodgingCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(Ui.ErrorMessages.EmptyLocation, createLodgingPageViewModel.ErrorMessage);
        }

        [TestMethod]
        public void CreateLodgingCommand_InvalidLocation_ReturnsErrorMessage()
        {
            var mockTrip = new Mock<Trip>();
            var mockLodgingManager = new Mock<LodgingManager>();
            var mockScreen = new Mock<IScreen>();
            var mockValidationManager = new Mock<ValidationManager>();
            mockValidationManager.Setup(vm => vm.DetermineIfValidLocation("Some-Invalid-Location-Text"))
                .Returns(new Response<bool>
                {
                    ErrorMessage = Ui.ErrorMessages.InvalidLocation,
                    StatusCode = (uint)Ui.StatusCode.BadRequest
                });

            CreateLodgingPageViewModel createLodgingPageViewModel =
                new(mockTrip.Object, mockScreen.Object) {
                    LodgingManager = mockLodgingManager.Object,
                    ValidationManager = mockValidationManager.Object
                };

            var testScheduler = new TestScheduler();

            createLodgingPageViewModel.Location = "Some-Invalid-Location-Text";
            createLodgingPageViewModel.StartDate = DateTime.Today;
            createLodgingPageViewModel.StartTime = TimeSpan.Zero;

            createLodgingPageViewModel.CreateLodgingCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(Ui.ErrorMessages.InvalidLocation, createLodgingPageViewModel.ErrorMessage);
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
            mockValidationManager.Setup(vm => vm.DetermineIfValidLocation("Paris, Italy"))
                .Returns(new Response<bool>
                {
                    Data = true
                });

            CreateLodgingPageViewModel createLodgingPageViewModel =
                new(mockTrip.Object, mockScreen.Object)
                {
                    ValidationManager = mockValidationManager.Object,
                    LodgingManager = mockLodgingManager.Object
                };

            var testScheduler = new TestScheduler();

            createLodgingPageViewModel.Location = "Paris, Italy";
            createLodgingPageViewModel.Notes = "notes";
            createLodgingPageViewModel.StartDate = DateTime.Today.AddDays(1);
            createLodgingPageViewModel.StartTime = TimeSpan.Zero;
            createLodgingPageViewModel.EndDate = DateTime.Today;
            createLodgingPageViewModel.EndTime = TimeSpan.Zero;

            createLodgingPageViewModel.CreateLodgingCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(Ui.ErrorMessages.InvalidStartDate,
                createLodgingPageViewModel.ErrorMessage);
        }

        [TestMethod]
        public void CreateLodgingCommand_NullDates_ReturnsErrorMessage()
        {
            var mockTrip = new Mock<Trip>();
            var mockScreen = new Mock<IScreen>();

            CreateLodgingPageViewModel createLodgingPageViewModel =
                new(mockTrip.Object, mockScreen.Object);

            var testScheduler = new TestScheduler();

            createLodgingPageViewModel.Location = "Atlanta";
            createLodgingPageViewModel.Notes = "notes";

            createLodgingPageViewModel.CreateLodgingCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(Ui.ErrorMessages.InvalidEventDate,
                createLodgingPageViewModel.ErrorMessage);
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
            mockValidationManager.Setup(vm => vm.DetermineIfValidLocation("Paris, Italy"))
                .Returns(new Response<bool>
                {
                    Data = true
                });
            mockValidationManager.Setup(vm =>
                    vm.DetermineIfValidEventDates(0, DateTime.Today.AddDays(-3) + TimeSpan.Zero,
                        DateTime.Today + TimeSpan.Zero))
                .Returns(new Response<bool>
                {
                    ErrorMessage = Ui.ErrorMessages.EventStartDateBeforeTripStartDate +
                                   mockTrip.Object.StartDate.ToShortDateString(),
                    StatusCode = (uint) Ui.StatusCode.BadRequest
                });


            CreateLodgingPageViewModel createLodgingPageViewModel =
                new(mockTrip.Object, mockScreen.Object)
                {
                    ValidationManager = mockValidationManager.Object,
                    LodgingManager = mockLodgingManager.Object
                };

            var testScheduler = new TestScheduler();

            createLodgingPageViewModel.Location = "Paris, Italy";
            createLodgingPageViewModel.StartDate = DateTime.Now.AddDays(-3);
            createLodgingPageViewModel.StartTime = TimeSpan.Zero;
            createLodgingPageViewModel.EndDate = DateTime.Now;
            createLodgingPageViewModel.EndTime = TimeSpan.Zero;

            createLodgingPageViewModel.CreateLodgingCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(
                Ui.ErrorMessages.EventStartDateBeforeTripStartDate + mockTrip.Object.StartDate.ToShortDateString(),
                createLodgingPageViewModel.ErrorMessage);
        }

        [TestMethod]
        public void CreateLodgingCommand_SuccessfulCreation()
        {
            var mockLodgingManager = new Mock<LodgingManager>();
            mockLodgingManager.Setup(um => um.CreateLodging(0, "Paris, Italy", DateTime.Today, DateTime.Today, null))
                .Returns(new Response<int> {StatusCode = (uint) Ui.StatusCode.Success});
            var mockTrip = new Mock<Trip>();
            var mockScreen = new Mock<IScreen>();
            CreateLodgingPageViewModel createLodgingPageViewModel =
                new(mockTrip.Object, mockScreen.Object) {LodgingManager = mockLodgingManager.Object};

            var testScheduler = new TestScheduler();

            createLodgingPageViewModel.Location = "Paris, Italy";
            createLodgingPageViewModel.StartDate = DateTime.Today;
            createLodgingPageViewModel.EndDate = DateTime.Today;
            createLodgingPageViewModel.StartTime = DateTime.Today.TimeOfDay;
            createLodgingPageViewModel.EndTime = DateTime.Today.TimeOfDay;

            createLodgingPageViewModel.CreateLodgingCommand.ThrownExceptions.Subscribe();

            testScheduler.Start();

            Assert.AreEqual(string.Empty, createLodgingPageViewModel.ErrorMessage);
        }
    }
}