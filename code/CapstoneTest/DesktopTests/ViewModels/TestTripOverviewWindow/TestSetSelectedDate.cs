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
    public class TestSetSelectedDate
    {
        [TestMethod]
        public void TestSet_Success()
        {
            var startDate = DateTime.Now;
            var mockTrip = new Mock<Trip>();
            mockTrip.SetupGet(mt => mt.TripId).Returns(1);
            var mockScreen = new Mock<IScreen>();
            var mockWaypointManager = new Mock<WaypointManager>();
            mockWaypointManager.Setup(wm => wm.GetWaypointsOnDate(1, startDate))
                .Returns(new Response<IList<Waypoint>>
                {
                    Data = new List<Waypoint>
                    {
                        new()
                    }
                });
            TripOverviewPageViewModel testViewModel = new(mockTrip.Object, mockWaypointManager.Object, mockScreen.Object);
            
            testViewModel.SelectedDate = startDate;
            
            Assert.AreEqual(1, testViewModel.WaypointViewModels.Count);
            
        }
    }
}