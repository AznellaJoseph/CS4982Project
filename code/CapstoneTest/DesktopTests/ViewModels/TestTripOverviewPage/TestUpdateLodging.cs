using System;
using System.Collections.Generic;
using CapstoneBackend.Model;
using CapstoneDesktop.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReactiveUI;

namespace CapstoneTest.DesktopTests.ViewModels.TestTripOverviewPage
{
    [TestClass]
    public class TestUpdateLodging
    {
        [TestMethod]
        public void UpdateLodgings_ValidData_Success()
        {
            var startDate = DateTime.Now;
            var mockTrip = new Mock<Trip>();
            mockTrip.SetupGet(mt => mt.TripId).Returns(1);
            var mockScreen = new Mock<IScreen>();
            var mockLodgingManager = new Mock<LodgingManager>();
            mockLodgingManager.Setup(wm => wm.GetLodgingsOnDate(1, startDate))
                .Returns(new Response<IList<Lodging>>
                {
                    Data = new List<Lodging>
                    {
                        new(), new()
                    }
                });


            TripOverviewPageViewModel testViewModel = new(mockTrip.Object, mockScreen.Object)
            {
                LodgingManager = mockLodgingManager.Object,
                SelectedDate = startDate
            };

            Assert.AreEqual(2, testViewModel.LodgingViewModels.Count);
        }


        [TestMethod]
        public void UpdateEvents_NullData_EmptyWaypointList()
        {
            var startDate = DateTime.Now;
            var mockTrip = new Mock<Trip>();
            mockTrip.SetupGet(mt => mt.TripId).Returns(1);
            var mockScreen = new Mock<IScreen>();
            var mockLodgingManager = new Mock<LodgingManager>();
            mockLodgingManager.Setup(em => em.GetLodgingsOnDate(1, startDate))
                .Returns(new Response<IList<Lodging>>
                {
                    Data = null
                });
            TripOverviewPageViewModel testViewModel = new(mockTrip.Object, mockScreen.Object)
            {
                LodgingManager = mockLodgingManager.Object,
                SelectedDate = startDate
            };

            Assert.AreEqual(0, testViewModel.LodgingViewModels.Count);
        }

        [TestMethod]
        public void UpdateEvents_NullSelectedDate_ReturnsEmptyList()
        {
            var mockTrip = new Mock<Trip>();
            mockTrip.SetupGet(mt => mt.TripId).Returns(1);
            var mockScreen = new Mock<IScreen>();
            var mockLodgingManager = new Mock<LodgingManager>();
            TripOverviewPageViewModel testViewModel = new(mockTrip.Object, mockScreen.Object)
            {
                LodgingManager = mockLodgingManager.Object,
                SelectedDate = null
            };

            Assert.AreEqual(0, testViewModel.LodgingViewModels.Count);
        }
    }
}