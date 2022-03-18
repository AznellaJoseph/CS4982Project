using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace CapstoneTest.BackendTests.DAL.TestWaypointDAL
{
    [TestClass]
    public class TestGetWaypointsOnDate
    {
        private MySqlConnection _connection;
        private int testTripId;
        private int testWaypointId;

        [TestInitialize]
        public void Setup()
        {
            _connection = new MySqlConnection(Connection.ConnectionString);
            testTripId = new TripDal(_connection).CreateTrip(1, "TestTrip", "Some Notes", DateTime.Now, DateTime.Now.AddDays(7));
            testWaypointId = new WaypointDal(_connection).CreateWaypoint(testTripId, "TestLocation", DateTime.Now.AddDays(2), DateTime.Now.AddDays(7), null);
        }

        [TestMethod]
        public void CallProcedure_WithNoWaypointsOnDate_ReturnsEmpty()
        {
            WaypointDal testDAL = new(_connection);

            IList<Waypoint> result = testDAL.GetWaypointsOnDate(testTripId, DateTime.Now);

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void CallProcedure_WithWaypointOnDate_ReturnsList()
        {
            WaypointDal testDAL = new(_connection);

            IList<Waypoint> result = testDAL.GetWaypointsOnDate(testTripId, DateTime.Now.AddDays(3));

            Assert.AreEqual(1, result.Count);
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
