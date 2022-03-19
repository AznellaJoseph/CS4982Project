using System;
using System.Globalization;
using CapstoneDesktop.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CapstoneTest.DesktopTests.Converters.TestDateFormatter
{
    [TestClass]
    public class TestConvertBack
    {
        [TestMethod]
        public void ConvertBack_ThrowsException()
        {
            var testFormatter = new DateFormatter();

            Assert.ThrowsException<NotSupportedException>(() => testFormatter.ConvertBack("convert", typeof(DateTime),
                "String", CultureInfo.CurrentCulture));
        }
    }
}