using System;
using CapstoneBackend.Model;
using CapstoneDesktop.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CapstoneTest.DesktopTests.ViewModels.TestCreateTripWindow
{
    [TestClass]
    public class TestConstructor
    {
        [TestMethod]
        public void Constructor_OneParameter_PropertyCreations()
        {
            var mockTripManager = new Mock<TripManager>();
            CreateTripWindowViewModel createTripWindowViewModel = new(mockTripManager.Object);

            Assert.IsNotNull(createTripWindowViewModel.CancelCreateTripCommand);
            Assert.IsNotNull(createTripWindowViewModel.CreateTripCommand);
            Assert.AreEqual(string.Empty, createTripWindowViewModel.ErrorMessage);
            Assert.AreEqual(DateTime.Today, createTripWindowViewModel.StartDate);
            Assert.AreEqual(DateTime.Today.AddDays(1), createTripWindowViewModel.EndDate);
            Assert.IsNull(createTripWindowViewModel.TripName);
            Assert.IsNull(createTripWindowViewModel.Notes);
        }

        [TestMethod]
        public void Constructor_NoParameters_PropertyCreations()
        {
            CreateTripWindowViewModel createTripWindowViewModel = new();

            Assert.IsNotNull(createTripWindowViewModel.CancelCreateTripCommand);
            Assert.IsNotNull(createTripWindowViewModel.CreateTripCommand);
            Assert.AreEqual(string.Empty, createTripWindowViewModel.ErrorMessage);
            Assert.AreEqual(DateTime.Today, createTripWindowViewModel.StartDate);
            Assert.AreEqual(DateTime.Today.AddDays(1), createTripWindowViewModel.EndDate);
            Assert.IsNull(createTripWindowViewModel.TripName);
            Assert.IsNull(createTripWindowViewModel.Notes);
        }
    }
}