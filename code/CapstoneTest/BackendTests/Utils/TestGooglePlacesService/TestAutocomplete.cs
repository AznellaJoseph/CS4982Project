using System.Collections;
using CapstoneBackend.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CapstoneTest.BackendTests.Utils.TestGooglePlacesService
{
    [TestClass]
    public class TestAutocomplete
    {
        [TestMethod]
        public void Autocomplete_EmptyString_ReturnsEmptyList()
        {
            var input = string.Empty;

            IEnumerable result = GooglePlacesService.Autocomplete(input).Result;

            Assert.IsFalse(result.GetEnumerator().MoveNext());
        }

        [TestMethod]
        public void Autocomplete_ValidLocation_ReturnsFullList()
        {
            const string input = "Atlanta";
            const int minimumResults = 3;

            IEnumerable result = GooglePlacesService.Autocomplete(input).Result;

            for (var iterator = 0; iterator < minimumResults; iterator++)
                Assert.IsTrue(result.GetEnumerator().MoveNext());
        }
    }
}