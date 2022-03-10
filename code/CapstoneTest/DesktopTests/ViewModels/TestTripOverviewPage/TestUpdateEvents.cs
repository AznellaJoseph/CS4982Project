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
    public class TestUpdateEvents
    {
        [TestMethod]
        public void UpdateEvents_ValidData_Success()
        {
            var startDate = DateTime.Now;
            var mockTrip = new Mock<Trip>();
            mockTrip.SetupGet(mt => mt.TripId).Returns(1);
            var mockScreen = new Mock<IScreen>();
            var mockEventManager = new Mock<EventManager>();
            mockEventManager.Setup(wm => wm.GetEventsOnDate(1, startDate))
                .Returns(new Response<IList<IEvent>>
                {
                    Data = new List<IEvent>
                    {
                        new Waypoint(), new Transportation()
                    }
                });


            TripOverviewPageViewModel testViewModel = new(mockTrip.Object, mockEventManager.Object, mockScreen.Object)
            {
                SelectedDate = startDate
            };

            Assert.AreEqual(2, testViewModel.EventViewModels.Count);
        }


        [TestMethod]
        public void UpdateEvents_NullData_EmptyWaypointList()
        {
            var startDate = DateTime.Now;
            var mockTrip = new Mock<Trip>();
            mockTrip.SetupGet(mt => mt.TripId).Returns(1);
            var mockScreen = new Mock<IScreen>();
            var mockEventManager = new Mock<EventManager>();
            mockEventManager.Setup(em => em.GetEventsOnDate(1, startDate))
                .Returns(new Response<IList<IEvent>>
                {
                    Data = null
                });
            TripOverviewPageViewModel testViewModel = new(mockTrip.Object, mockEventManager.Object, mockScreen.Object)
            {
                SelectedDate = startDate
            };

            Assert.AreEqual(0, testViewModel.EventViewModels.Count);
        }

        [TestMethod]
        public void UpdateEvents_NullSelectedDate_ReturnsEmptyList()
        {
            var mockTrip = new Mock<Trip>();
            mockTrip.SetupGet(mt => mt.TripId).Returns(1);
            var mockScreen = new Mock<IScreen>();
            var mockEventManager = new Mock<EventManager>();
            TripOverviewPageViewModel testViewModel = new(mockTrip.Object, mockEventManager.Object, mockScreen.Object)
            {
                SelectedDate = null
            };

            Assert.AreEqual(0, testViewModel.EventViewModels.Count);
        }
    }
}