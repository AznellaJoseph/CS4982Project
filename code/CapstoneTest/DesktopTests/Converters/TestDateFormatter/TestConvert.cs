using System;
using System.Globalization;
using CapstoneDesktop.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CapstoneTest.DesktopTests.Converters.TestDateFormatter
{
    [TestClass]
    public class TestConvert
    {
        [TestMethod]
        public void Convert_NullValue_ReturnsNull()
        {
            var testFormatter = new DateFormatter();

            var result = testFormatter.Convert(null, typeof(DateTime), "Both", CultureInfo.CurrentCulture);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Convert_EnumCannotParseParameter_ReturnsDateToString()
        {
            var testFormatter = new DateFormatter();

            var result = testFormatter.Convert(DateTime.Today, typeof(DateTime), "Test", CultureInfo.CurrentCulture);

            Assert.AreEqual(result, DateTime.Today.ToString(CultureInfo.CurrentCulture));
        }

        [TestMethod]
        public void Convert_DateShownBoth_ReturnsDateWithTimeString()
        {
            var testFormatter = new DateFormatter();
            var testDate = DateTime.Parse("11/12/2013 12:12:12");

            var result = testFormatter.Convert(testDate, typeof(DateTime), "Both", new CultureInfo("en-US"));

            Assert.AreEqual(result, testDate.ToString("MM/dd/yyyy hh:mm tt", new CultureInfo("en-US")));
        }

        [TestMethod]
        public void Convert_DateShownDateOnly_ReturnsDateString()
        {
            var testFormatter = new DateFormatter();

            var result = testFormatter.Convert(DateTime.Today, typeof(DateTime), "Date", CultureInfo.CurrentCulture);

            Assert.AreEqual(result, DateTime.Today.ToString("MM/dd/yyyy", CultureInfo.CurrentCulture));
        }

        [TestMethod]
        public void Convert_DateShownTimeOnly_ReturnsTimeString()
        {
            var testFormatter = new DateFormatter();
            var testDate = DateTime.Parse("11/12/2013 12:12:12");

            var result = testFormatter.Convert(testDate, typeof(DateTime), "Time", CultureInfo.CurrentCulture);

            Assert.AreEqual(result, testDate.ToString("hh:mm tt", CultureInfo.CurrentCulture));
        }
    }
}