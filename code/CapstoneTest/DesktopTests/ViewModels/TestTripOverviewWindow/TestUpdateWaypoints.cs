using System;
using System.Collections.Generic;
using CapstoneBackend.Model;
using CapstoneDesktop.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReactiveUI;

namespace CapstoneTest.DesktopTests.ViewModels.TestTripOverviewWindow
{
    [TestClass]
    public class TestUpdateWaypoints
    {
        [TestMethod]
        public void UpdateWaypoints_ValidData_Success()
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
        public void TestSet_NullData_EmptyWaypointList()
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
    }
}
