using System;
using CapstoneBackend.Model;
using CapstoneDesktop.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CapstoneTest.DesktopTests.ViewModels.TestCreateWaypointWindow
{
    [TestClass]
    public class TestConstructor
    {
        [TestMethod]
        public void Constructor_OneParameter_PropertyCreations()
        {
            var mockWaypointManager = new Mock<WaypointManager>();
            CreateWaypointWindowViewModel createWaypointWindowViewModel = new(mockWaypointManager.Object);

            Assert.AreEqual(string.Empty, createWaypointWindowViewModel.ErrorMessage);
            Assert.AreEqual(DateTime.MinValue, createWaypointWindowViewModel.StartDate);
            Assert.AreEqual(DateTime.MinValue, createWaypointWindowViewModel.EndDate);
            Assert.IsNotNull(createWaypointWindowViewModel.CreateWaypointCommand);
            Assert.IsNotNull(createWaypointWindowViewModel.CancelCreateWaypointCommand);
            Assert.IsNotNull(createWaypointWindowViewModel.StartTime);
            Assert.IsNotNull(createWaypointWindowViewModel.EndTime);
            Assert.IsNull(createWaypointWindowViewModel.WaypointLocation);
            Assert.IsNull(createWaypointWindowViewModel.Notes);
        }

        [TestMethod]
        public void Constructor_NoParameters_PropertyCreations()
        {
            CreateWaypointWindowViewModel createWaypointWindowViewModel = new();

            Assert.AreEqual(string.Empty, createWaypointWindowViewModel.ErrorMessage);
            Assert.AreEqual(DateTime.MinValue, createWaypointWindowViewModel.StartDate);
            Assert.AreEqual(DateTime.MinValue, createWaypointWindowViewModel.EndDate);
            Assert.IsNotNull(createWaypointWindowViewModel.CreateWaypointCommand);
            Assert.IsNotNull(createWaypointWindowViewModel.CancelCreateWaypointCommand);
            Assert.IsNotNull(createWaypointWindowViewModel.StartTime);
            Assert.IsNotNull(createWaypointWindowViewModel.EndTime);
            Assert.IsNull(createWaypointWindowViewModel.WaypointLocation);
            Assert.IsNull(createWaypointWindowViewModel.Notes);
        }
    }
}