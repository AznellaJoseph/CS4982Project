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
    public class TestDetermineIfEventDatesClash
    {

        [TestMethod]
        public void DetermineIfEventDatesClash_ReturnsTrue()
        {
            var mockEventManager = new Mock<EventManager>();
            mockEventManager.Setup(em => em.DetermineIfEventDatesClash(1, DateTime.Today, DateTime.Today))
                .Returns(new Response<bool> { Data = true });


            Assert.IsTrue(mockEventManager.Object.DetermineIfEventDatesClash(1, DateTime.Today, DateTime.Today).Data);
        }

        [TestMethod]
        public void DetermineIfEventsDatesClash_ReturnsFalse()
        {
            var mockEventManager = new Mock<EventManager>();
            mockEventManager.Setup(em => em.DetermineIfEventDatesClash(1, DateTime.Today, DateTime.Today))
                .Returns(new Response<bool> { Data = false });


            Assert.IsFalse(mockEventManager.Object.DetermineIfEventDatesClash(1, DateTime.Today, DateTime.Today).Data);
        }
    }
}
