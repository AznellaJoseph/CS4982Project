using CapstoneBackend.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReactiveUI;
using System.Collections;

namespace CapstoneTest.BackendTests.Utils.TestGooglePlacesService
{
    [TestClass]
    public class TestAutocomplete
    {
        [TestMethod]
        public void Autocomplete_EmptyString_ReturnsEmptyList()
        {
            string input = string.Empty;

            IEnumerable result = GooglePlacesService.Autocomplete(input).Result;

            Assert.IsFalse(result.GetEnumerator().MoveNext());
        }

        [TestMethod]
        public void Autocomplete_ValidLocation_ReturnsFullList()
        {
            string input = "Atlanta";
            int minimumResults = 3;

            IEnumerable result = GooglePlacesService.Autocomplete(input).Result;

            for (int iterator = 0; iterator < minimumResults; iterator++) {
                Assert.IsTrue(result.GetEnumerator().MoveNext());
            }
        }

    }
}
