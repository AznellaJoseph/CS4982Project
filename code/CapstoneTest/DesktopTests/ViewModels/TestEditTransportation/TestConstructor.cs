using System;
using CapstoneBackend.Model;
using CapstoneDesktop.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReactiveUI;

namespace CapstoneTest.DesktopTests.ViewModels.TestEditTransportation
{
    [TestClass]
    public class TestConstructor
    {
        [TestMethod]
        public void Constructor_PropertyCreations()
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
            EditTransportationPageViewModel editTransportationPageViewModel = new(transportation, mockScreen.Object);

            Assert.AreEqual(editTransportationPageViewModel.HostScreen, mockScreen.Object);
            Assert.AreEqual(editTransportationPageViewModel.StartDate, transportation.StartDate.Date);
            Assert.AreEqual(editTransportationPageViewModel.StartTime, transportation.StartDate.TimeOfDay);
            Assert.AreEqual(editTransportationPageViewModel.EndDate, transportation.EndDate.Date);
            Assert.AreEqual(editTransportationPageViewModel.EndTime, transportation.EndDate.TimeOfDay);
            Assert.AreEqual(editTransportationPageViewModel.Method, transportation.Method);
            Assert.AreEqual(editTransportationPageViewModel.Notes, transportation.Notes);
            Assert.IsNotNull(editTransportationPageViewModel.EditTransportationCommand);
            Assert.IsNotNull(editTransportationPageViewModel.CancelEditTransportationCommand);
            Assert.IsNotNull(editTransportationPageViewModel.TransportationManager);
            Assert.IsNotNull(editTransportationPageViewModel.ValidationManager);
            Assert.IsNotNull(editTransportationPageViewModel.UrlPathSegment);
        }
    }
}