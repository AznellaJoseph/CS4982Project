using CapstoneBackend.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CapstoneTest.BackendTests.Utils.TestGoogleGeocodeService
{
    [TestClass]
    public class TestGetLocationByAddress
    {
        [TestMethod]
        public void GetLocationByAddress_ValidAddress_ReturnsValidLocation()
        {
            var locationResult =
                GoogleGeocodeService.GetLocationByAddress("Starbucks, South Park Street, Carrollton, GA 30117");

            Assert.AreEqual(33.5591996, locationResult.Latitude);
            Assert.AreEqual(-85.0764036, locationResult.Longitude);
        }
    }
}