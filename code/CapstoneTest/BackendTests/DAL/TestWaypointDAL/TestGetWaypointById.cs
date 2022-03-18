using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using System;

namespace CapstoneTest.BackendTests.DAL.TestWaypointDAL
{
    [TestClass]
    public class TestGetWaypointById
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
        public void CallProcedure_WithInvalidWaypointId_ReturnsNull()
        {
            WaypointDal testDAL = new(_connection);

            Waypoint? result = testDAL.GetWaypointById(-1);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void CallProcedure_WithValidWaypointId_ReturnsWaypoint()
        {
            WaypointDal testDAL = new(_connection);
            testWaypointId = testDAL.CreateWaypoint(1, "TestLocation", DateTime.Now, DateTime.Now, "Some Notes");

            Waypoint? result = testDAL.GetWaypointById(1);

            Assert.IsNotNull(result);
            Assert.IsTrue(result is Waypoint);
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
