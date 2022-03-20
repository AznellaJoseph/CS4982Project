using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using System;

namespace CapstoneTest.BackendTests.DAL.TestWaypointDAL
{
    [TestClass]
    public class TestRemoveWaypoint
    {
        private MySqlConnection _connection;
        private int testTripId;
        private int testWaypointId;

        [TestInitialize]
        public void Setup()
        {
            _connection = new MySqlConnection(Connection.ConnectionString);
            testTripId = new TripDal(_connection).CreateTrip(1, "TestTrip", "Some Notes", DateTime.Now, DateTime.Now);
        }

        [TestMethod]
        public void CallProcedure_WithInvalidWaypointId_ReturnsFalse()
        {
            WaypointDal testDAL = new(_connection);

            bool result = testDAL.RemoveWaypoint(-1);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CallProcedure_WithValidWaypointId_ReturnsTrue()
        {
            WaypointDal testDAL = new(_connection);
            testWaypointId = testDAL.CreateWaypoint(1, "TestLocation", DateTime.Now, DateTime.Now, "Some Notes");

            bool result = testDAL.RemoveWaypoint(testWaypointId);

            Assert.IsTrue(result);
        }

        [TestCleanup]
        public void TearDown()
        {
            _connection.Open();
            string removeTrip = $"delete from trip where tripId = {testTripId};";
            string removeWaypoint = $"delete from waypoint where waypointId = {testWaypointId};";

            using MySqlCommand tripCmd = new MySqlCommand(removeTrip, _connection);
            tripCmd.ExecuteNonQuery();

            using MySqlCommand waypointCmd = new MySqlCommand(removeWaypoint, _connection);
            waypointCmd.ExecuteNonQuery();

            this._connection.Close();
        }
    }
}
