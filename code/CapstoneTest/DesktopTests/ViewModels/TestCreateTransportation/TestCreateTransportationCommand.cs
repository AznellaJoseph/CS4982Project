using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using CapstoneDesktop.ViewModels;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReactiveUI;
using System;

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

            Assert.AreEqual(Ui.ErrorMessages.NullWaypointStartDate, createTransportationViewModel.ErrorMessage);
        }

        [TestMethod]
        public void CreateTransportationCommand_NullEndDate_SuccessfulCreation()
        {
            var mockTrip = new Mock<Trip>();
            mockTrip.Object.TripId = 0;
            mockTrip.Object.EndDate = DateTime.Today;
            var mockTransportationManager = new Mock<TransportationManager>();
            var mockScreen = new Mock<IScreen>();
            mockTransportationManager.Setup(um => um.CreateTransportation(0, "Plane", DateTime.Today.AddDays(-2), DateTime.Today))
                .Returns(new Response<int> { StatusCode = (uint)Ui.StatusCode.Success });
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
        public void CreateTransportationCommand_InvalidEnteredDates_ReturnsErrorMessage()
        {
            var mockTrip = new Mock<Trip>();
            mockTrip.Object.StartDate = DateTime.Today.AddDays(-2);
            mockTrip.Object.EndDate = DateTime.Now;
            var mockTransportationManager = new Mock<TransportationManager>();
            var mockScreen = new Mock<IScreen>();

            CreateTransportationPageViewModel createTransportationViewModel =
                new(mockTrip.Object, mockTransportationManager.Object, mockScreen.Object);

            var testScheduler = new TestScheduler();

            createTransportationViewModel.Method = "Plane";
            createTransportationViewModel.StartDate = DateTimeOffset.Now.AddDays(-3);
            createTransportationViewModel.StartTime = TimeSpan.Zero;
            createTransportationViewModel.EndDate = DateTimeOffset.Now;
            createTransportationViewModel.EndTime = TimeSpan.Zero;

            createTransportationViewModel.CreateTransportationCommand.Execute().Subscribe();

            testScheduler.Start();

            Assert.AreEqual(Ui.ErrorMessages.InvalidTransportationDate,
                createTransportationViewModel.ErrorMessage);
        }

        [TestMethod]
        public void CreateTransportationCommand_SuccessfulCreation()
        {
            var mockTransportationManager = new Mock<TransportationManager>();
            mockTransportationManager.Setup(um => um.CreateTransportation(0, "Plane", DateTime.Today, DateTime.Today))
                .Returns(new Response<int> { StatusCode = (uint)Ui.StatusCode.Success });
            var mockTrip = new Mock<Trip>();
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
