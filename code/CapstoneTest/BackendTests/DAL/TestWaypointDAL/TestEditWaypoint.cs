using System;
using CapstoneBackend.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;

namespace CapstoneTest.BackendTests.DAL.TestWaypointDAL
{
    [TestClass]
    public class TestEditWaypoint
    {
        private readonly MySqlConnection _connection = new(Connection.ConnectionString);
        private int _testWaypointId;

        [TestMethod]
        public void CallProcedure_ReturnsFalse()
        {
            WaypointDal testDal = new(_connection);
            _testWaypointId = testDal.CreateWaypoint(1, "TestLocation", DateTime.Now, DateTime.Now, "Some Notes");
            var testWaypoint = testDal.GetWaypointById(_testWaypointId);
            Assert.IsNotNull(testWaypoint);
            testDal.RemoveWaypoint(_testWaypointId);

            Assert.IsFalse(testDal.EditWaypoint(testWaypoint));
        }

        [TestMethod]
        public void CallProcedure_ReturnsTrue()
        {
            WaypointDal testDal = new(_connection);
            _testWaypointId = testDal.CreateWaypoint(1, "TestLocation", DateTime.Now, DateTime.Now, "Some Notes");
            var testWaypoint = testDal.GetWaypointById(_testWaypointId);
            Assert.IsNotNull(testWaypoint);

            testWaypoint.Location = "Hilton Atlanta";
            Assert.IsTrue(testDal.EditWaypoint(testWaypoint));
        }
    }
}