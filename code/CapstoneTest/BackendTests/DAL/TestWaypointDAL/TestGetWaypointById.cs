using System;
using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;

namespace CapstoneTest.BackendTests.DAL.TestWaypointDAL
{
    [TestClass]
    public class TestGetWaypointById
    {
        private readonly MySqlConnection _connection = new(Connection.ConnectionString);
        private int _testTripId;
        private int _testWaypointId;

        [TestInitialize]
        public void Setup()
        {
            _testTripId = new TripDal(_connection).CreateTrip(1, "TestTrip", "Some Notes", DateTime.Now, DateTime.Now);
        }

        [TestMethod]
        public void CallProcedure_WithInvalidWaypointId_ReturnsNull()
        {
            WaypointDal testDal = new(_connection);

            var result = testDal.GetWaypointById(-1);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void CallProcedure_WithValidWaypointId_ReturnsWaypoint()
        {
            WaypointDal testDal = new(_connection);
            _testWaypointId = testDal.CreateWaypoint(1, "TestLocation", DateTime.Now, DateTime.Now, "Some Notes");

            var result = testDal.GetWaypointById(1);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Waypoint));
        }

        [TestCleanup]
        public void TearDown()
        {
            _connection.Open();
            var removeTrip = $"delete from trip where tripId = {_testTripId};";
            var removeWaypoint = $"delete from waypoint where waypointId = {_testWaypointId};";

            using var tripCmd = new MySqlCommand(removeTrip, _connection);
            tripCmd.ExecuteNonQuery();

            using var waypointCmd = new MySqlCommand(removeWaypoint, _connection);
            waypointCmd.ExecuteNonQuery();

            _connection.Close();
        }
    }
}