using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using System;

namespace CapstoneTest.BackendTests.DAL.TestWaypointDAL
{
    [TestClass]
    public class TestGetTransportationById
    {
        private MySqlConnection _connection;
        private int testTripId;
        private int testTransportationId;

        [TestInitialize]
        public void Setup()
        {
            _connection = new MySqlConnection(Connection.ConnectionString);
            testTripId = new TripDal(_connection).CreateTrip(1, "TestTrip", "Some Notes", DateTime.Now, DateTime.Now);
        }

        [TestMethod]
        public void CallProcedure_WithInvalidTransportationId_ReturnsNull()
        {
            TransportationDal testDAL = new(_connection);

            Transportation? result = testDAL.GetTransportationById(-1);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void CallProcedure_WithValidTransportationId_ReturnsTransportation()
        {
            TransportationDal testDAL = new(_connection);
            testTransportationId = testDAL.CreateTransportation(1, "TestMethod", DateTime.Now, DateTime.Now, "Some Notes");

            Transportation? result = testDAL.GetTransportationById(1);

            Assert.IsNotNull(result);
            Assert.IsTrue(result is Transportation);
        }

        [TestCleanup]
        public void TearDown()
        {
            _connection.Open();
            string removeTrip = $"delete from trip where tripId = {testTripId};";
            string removeWaypoint = $"delete from waypoint where waypointId = {testTransportationId};";

            using MySqlCommand tripCmd = new MySqlCommand(removeTrip, _connection);
            tripCmd.ExecuteNonQuery();

            using MySqlCommand waypointCmd = new MySqlCommand(removeWaypoint, _connection);
            waypointCmd.ExecuteNonQuery();

            this._connection.Close();
        }
    }
}
