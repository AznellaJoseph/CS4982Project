using System;
using System.Collections.Generic;
using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CapstoneTest.BackendTests.Model.TestTransportationManager
{
    [TestClass]
    public class TestGetTransportationOnDate
    {
        [TestMethod]
        public void Call_EmptySet_ReturnsEmptyList()
        {
            var currentTime = DateTime.Now;
            IList<Transportation> transportations = new List<Transportation>();

            var mockTransportationDal = new Mock<TransportationDal>();
            mockTransportationDal.Setup(db => db.GetTransportationsOnDate(1, currentTime)).Returns(transportations);

            TransportationManager transportationManager = new(mockTransportationDal.Object);

            var resultResponse =
                transportationManager.GetTransportationsOnDate(1, currentTime);

            Assert.AreEqual(0, resultResponse.Data?.Count);
        }

        [TestMethod]
        public void Call_YieldsSetWithOneValue_ReturnsExpectedList()
        {
            var currentTime = DateTime.Now;
            IList<Transportation> transportations = new List<Transportation>
            {
                new()
                {
                    TripId = 1,
                    TransportationId = 1,
                    Method = "Car",
                    StartDate = currentTime,
                    EndDate = currentTime
                }
            };

            var mockTransportationDal = new Mock<TransportationDal>();
            mockTransportationDal.Setup(db => db.GetTransportationsOnDate(1, currentTime)).Returns(transportations);

            TransportationManager transportationManager = new(mockTransportationDal.Object);

            var resultResponse =
                transportationManager.GetTransportationsOnDate(1, currentTime);

            Assert.AreEqual(1, resultResponse.Data?.Count);
            Assert.AreEqual(1, resultResponse.Data?[0].TripId);
            Assert.AreEqual(1, resultResponse.Data?[0].TransportationId);
            Assert.AreEqual("Car", resultResponse.Data?[0].Method);
            Assert.AreEqual(currentTime, resultResponse.Data?[0].StartDate);
            Assert.AreEqual(currentTime, resultResponse.Data?[0].EndDate);
        }
    }
}