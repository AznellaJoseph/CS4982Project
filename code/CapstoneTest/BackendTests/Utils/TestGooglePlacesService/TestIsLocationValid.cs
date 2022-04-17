using CapstoneBackend.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReactiveUI;
using System.Collections;

namespace CapstoneTest.BackendTests.Utils.TestGooglePlacesService
{
    [TestClass]
    public class TestIsLocationValid
    {
        [TestMethod]
        public void Location_EmptyString_ReturnsFalse()
        {
            string input = string.Empty;

            bool result = GooglePlacesService.IsLocationValid(input).Result;

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Location_ValidLocation_ReturnsTrue()
        {
            string input = "Atlanta";

            bool result = GooglePlacesService.IsLocationValid(input).Result;

            Assert.IsTrue(result);
        }
    }
}
