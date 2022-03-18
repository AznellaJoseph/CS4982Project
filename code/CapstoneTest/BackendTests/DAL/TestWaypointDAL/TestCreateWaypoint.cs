using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using System;

namespace CapstoneTest.BackendTests.DAL.TestWaypointDAL
{
    [TestClass]
    public class TestCreateWaypoint
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
        public void CallProcedure_WithIvalidTripId_Fails()
        {
            WaypointDal testDAL = new(_connection);

            Assert.ThrowsException<MySqlException>(() => testDAL.CreateWaypoint(-1, "TestLocation", DateTime.Now, DateTime.Now, "Some Notes"));
        }

        [TestMethod]
        public void CallProcedure_WithValidInput_Succeeds()
        {
            WaypointDal testDAL = new(_connection);

            int? resultID = testDAL.CreateWaypoint(testTripId, "TestLocation", DateTime.Now, DateTime.Now, "Some Notes");
            testWaypointId = (int) resultID;

            Assert.IsTrue(resultID is not null);
            Assert.IsTrue(resultID is int);
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
