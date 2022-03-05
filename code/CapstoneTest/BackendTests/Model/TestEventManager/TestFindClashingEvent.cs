using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapstoneBackend.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CapstoneTest.BackendTests.Model.TestEventManager
{
    [TestClass]
    public class TestFindClashingEvent
    {

        [TestMethod]
        public void DetermineIfEventDatesClash_ReturnsNull()
        {
            var mockEventManager = new Mock<EventManager>();
            mockEventManager.Setup(em => em.FindClashingEvent(1, DateTime.Today, DateTime.Today))
                .Returns(new Response<IEvent> { Data = null });


            Assert.IsNull(mockEventManager.Object.FindClashingEvent(1, DateTime.Today, DateTime.Today).Data);
        }

        [TestMethod]
        public void DetermineIfEventsDatesClash_ReturnsNotNull()
        {
            var mockEventManager = new Mock<EventManager>();
            mockEventManager.Setup(em => em.FindClashingEvent(1, DateTime.Today, DateTime.Today))
                .Returns(new Response<IEvent> { Data = new Transportation() });


            Assert.IsNotNull(mockEventManager.Object.FindClashingEvent(1, DateTime.Today, DateTime.Today).Data);
        }
    }
}
