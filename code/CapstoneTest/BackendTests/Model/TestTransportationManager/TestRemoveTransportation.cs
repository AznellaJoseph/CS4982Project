using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CapstoneTest.BackendTests.Model.TestTransportationManager
{
    [TestClass]
    public class TestRemoveTransportation
    {
        [TestMethod]
        public void RemoveWaypoint_WaypointNotExists_ReturnsErrorMessage()
        {
            var mock = new Mock<TransportationDal>();
            mock.Setup(db => db.RemoveTransportation(2)).Returns(false);

            TransportationManager manager = new(mock.Object);
            var resultResponse = manager.RemoveTransportation(2);

            Assert.AreEqual((uint)Ui.StatusCode.BadRequest, resultResponse.StatusCode);
            Assert.AreEqual(Ui.ErrorMessages.TransportationNotFound, resultResponse.ErrorMessage);
        }

        [TestMethod]
        public void RemoveWaypoint_ValidWaypointId_ReturnsTrue()
        {
            var mock = new Mock<TransportationDal>();
            mock.Setup(db => db.RemoveTransportation(1)).Returns(true);

            TransportationManager manager = new(mock.Object);
            var resultResponse = manager.RemoveTransportation(1);

            Assert.AreEqual((uint)Ui.StatusCode.Success, resultResponse.StatusCode);
            Assert.AreEqual(true, resultResponse.Data);
        }
    }
}