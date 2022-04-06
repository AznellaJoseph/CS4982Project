using CapstoneBackend.Model;
using CapstoneDesktop.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReactiveUI;

namespace CapstoneTest.DesktopTests.ViewModels.TestCreateLodging
{
    [TestClass]
    public class TestConstructor
    {
        [TestMethod]
        public void Constructor_PropertyCreations()
        {
            var mockTrip = new Mock<Trip>();
            var mockScreen = new Mock<IScreen>();
            CreateLodgingPageViewModel createLodgingViewModel = new(mockTrip.Object, mockScreen.Object);

            Assert.AreEqual(string.Empty, createLodgingViewModel.ErrorMessage);
            Assert.AreEqual(mockScreen.Object, createLodgingViewModel.HostScreen);
            Assert.AreEqual(string.Empty, createLodgingViewModel.Location);
            Assert.IsNotNull(createLodgingViewModel.UrlPathSegment);
            Assert.IsNotNull(createLodgingViewModel.CreateLodgingCommand);
            Assert.IsNotNull(createLodgingViewModel.CancelCreateLodgingCommand);
            Assert.IsNotNull(createLodgingViewModel.ValidationManager);
            Assert.IsNotNull(createLodgingViewModel.LodgingManager);
            Assert.IsNotNull(createLodgingViewModel.AutocompletePredictions);
            Assert.IsNull(createLodgingViewModel.StartTime);
            Assert.IsNull(createLodgingViewModel.EndTime);
            Assert.IsNull(
                createLodgingViewModel.StartDate);
            Assert.IsNull(createLodgingViewModel.EndDate);
            Assert.IsNull(
                createLodgingViewModel.Notes);
        }
    }
}