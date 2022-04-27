using System;
using CapstoneBackend.Model;
using CapstoneDesktop.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReactiveUI;

namespace CapstoneTest.DesktopTests.ViewModels.TestEditLodging
{
    [TestClass]
    public class TestConstructor
    {
        [TestMethod]
        public void Constructor_PropertyCreations()
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
            EditLodgingPageViewModel editLodgingPageViewModel = new(lodging, mockScreen.Object);

            Assert.AreEqual(editLodgingPageViewModel.HostScreen, mockScreen.Object);
            Assert.AreEqual(editLodgingPageViewModel.StartDate, lodging.StartDate.Date);
            Assert.AreEqual(editLodgingPageViewModel.StartTime, lodging.StartDate.TimeOfDay);
            Assert.AreEqual(editLodgingPageViewModel.EndDate, lodging.EndDate.Date);
            Assert.AreEqual(editLodgingPageViewModel.EndTime, lodging.EndDate.TimeOfDay);
            Assert.AreEqual(editLodgingPageViewModel.Location, lodging.Location);
            Assert.AreEqual(editLodgingPageViewModel.Notes, lodging.Notes);
            Assert.IsNotNull(editLodgingPageViewModel.EditLodgingCommand);
            Assert.IsNotNull(editLodgingPageViewModel.CancelEditLodgingCommand);
            Assert.IsNotNull(editLodgingPageViewModel.LodgingManager);
            Assert.IsNotNull(editLodgingPageViewModel.ValidationManager);
            Assert.IsNotNull(editLodgingPageViewModel.UrlPathSegment);
        }
    }
}