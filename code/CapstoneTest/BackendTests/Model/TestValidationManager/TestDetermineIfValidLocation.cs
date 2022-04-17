using System;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CapstoneTest.BackendTests.Model.TestValidationManager
{
    [TestClass]
    public class TestDetermineIfValidLocation
    {
        [TestMethod]
        public void DetermineIfValidLocation_InvalidLocation_ReturnsErrorResponse()
        {
            var validationManager = new ValidationManager();
            var input = string.Empty;

            var response = validationManager.DetermineIfValidLocation(input);

            Assert.AreEqual((uint)Ui.StatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual(Ui.ErrorMessages.InvalidLocation, response.ErrorMessage);
        }

        [TestMethod]
        public void DetermineIfValidLocation_ValidLocation_ReturnsSuccessResponse()
        {
            var validationManager = new ValidationManager();
            var input = "Atlanta";

            var response = validationManager.DetermineIfValidLocation(input);

            Assert.AreEqual((uint)Ui.StatusCode.Success, response.StatusCode);
            Assert.IsTrue(response.Data);
        }
    }
}
