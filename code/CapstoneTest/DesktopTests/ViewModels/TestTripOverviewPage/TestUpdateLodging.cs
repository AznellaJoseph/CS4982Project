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
            mockLodgingManager.Setup(wm => wm.GetLodgingsByTripId(1))
                .Returns(new Response<IList<Lodging>>
                {
                    Data = new List<Lodging>
                    {
                        new(), new()
                    }
                });


            TripOverviewPageViewModel testViewModel =
                new(mockTrip.Object, mockScreen.Object, mockLodgingManager.Object);

            Assert.AreEqual(2, testViewModel.LodgingViewModels.Count);
        }


        [TestMethod]
        public void UpdateLodgings_NullData_EmptyLodgingList()
        {
            var startDate = DateTime.Now;
            var mockTrip = new Mock<Trip>();
            mockTrip.SetupGet(mt => mt.TripId).Returns(1);
            var mockScreen = new Mock<IScreen>();
            var mockLodgingManager = new Mock<LodgingManager>();
            mockLodgingManager.Setup(em => em.GetLodgingsByTripId(1))
                .Returns(new Response<IList<Lodging>>
                {
                    Data = null
                });
            TripOverviewPageViewModel testViewModel =
                new(mockTrip.Object, mockScreen.Object, mockLodgingManager.Object);

            Assert.AreEqual(0, testViewModel.LodgingViewModels.Count);
        }

    }
}