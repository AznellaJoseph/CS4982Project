using CapstoneBackend.Model;
using CapstoneDesktop.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReactiveUI;

namespace CapstoneTest.DesktopTests.ViewModels.TestTransportation
{
    [TestClass]
    public class TestConstructor
    {
        [TestMethod]
        public void TestConstructor_Success()
        {
            var mockTransportation = new Mock<Transportation>();
            var mockScreen = new Mock<IScreen>();
            var result = new TransportationViewModel(mockTransportation.Object, mockScreen.Object);
            Assert.AreEqual(mockTransportation.Object, result.Transportation);
            Assert.AreEqual(mockTransportation.Object, result.Event);
        }
    }
}