using System;
using System.Collections.Generic;
using CapstoneBackend.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CapstoneTest.BackendTests.Model.TestEventManager
{
    [TestClass]
    public class TestGetEventsOnDate
    {
        [TestMethod]
        public void Call_ReturnsExpectedList()
        {
            var currentTime = DateTime.Now;
            IList<Waypoint> fakeWaypoints = new List<Waypoint>
            {
                new()
                {
                    StartDate = currentTime,
                    EndDate = currentTime
                }
            };
            IList<Transportation> fakeTransportation = new List<Transportation>
            {
                new()
                {
                    StartDate = currentTime.AddDays(1),
                    EndDate = currentTime.AddDays(1)
                }
            };

            var mockWaypointManager = new Mock<WaypointManager>();
            var mockTransportationManager = new Mock<TransportationManager>();

            mockTransportationManager.Setup(tm => tm.GetTransportationOnDate(1, currentTime)).Returns(
                new Response<IList<Transportation>>
                {
                    Data = fakeTransportation
                });

            mockWaypointManager.Setup(tm => tm.GetWaypointsOnDate(1, currentTime)).Returns(new Response<IList<Waypoint>>
            {
                Data = fakeWaypoints
            });

            var eventManager = new EventManager
            {
                WaypointManager = mockWaypointManager.Object,
                TransportationManager = mockTransportationManager.Object
            };

            var resultResponse = eventManager.GetEventsOnDate(1, currentTime);

            Assert.AreEqual(2, resultResponse.Data?.Count);
            Assert.IsInstanceOfType(resultResponse.Data?[0], typeof(Waypoint));
            Assert.IsInstanceOfType(resultResponse.Data?[1], typeof(Transportation));
        }

        [TestMethod]
        public void Call_ManagersReturnNullList_ReturnsEmptyList()
        {
            var currentTime = DateTime.Now;
            IList<Waypoint>? fakeWaypoints = null;
            IList<Transportation>? fakeTransportation = null;

            var mockWaypointManager = new Mock<WaypointManager>();
            var mockTransportationManager = new Mock<TransportationManager>();

            mockTransportationManager.Setup(tm => tm.GetTransportationOnDate(1, currentTime)).Returns(
                new Response<IList<Transportation>>
                {
                    Data = fakeTransportation
                });

            mockWaypointManager.Setup(tm => tm.GetWaypointsOnDate(1, currentTime)).Returns(new Response<IList<Waypoint>>
            {
                Data = fakeWaypoints
            });

            var eventManager = new EventManager
            {
                WaypointManager = mockWaypointManager.Object,
                TransportationManager = mockTransportationManager.Object
            };

            var resultResponse = eventManager.GetEventsOnDate(1, currentTime);

            Assert.AreEqual(0, resultResponse.Data?.Count);
        }
    }
}