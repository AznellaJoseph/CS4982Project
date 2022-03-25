using System;
using CapstoneBackend.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;

namespace CapstoneTest.BackendTests.DAL.TestWaypointDAL
{
    [TestClass]
    public class TestGetWaypointsByTripId
    {
        private readonly MySqlConnection _connection = new(Connection.ConnectionString);
        private int _testTripId;
        private int _testWaypointId;

        [TestInitialize]
        public void Setup()
        {
            _testTripId =
                new TripDal(_connection).CreateTrip(1, "TestTrip", "Some Notes", DateTime.Now, DateTime.Now.AddDays(7));
            _testWaypointId = new WaypointDal(_connection).CreateWaypoint(_testTripId, "TestLocation",
                DateTime.Now.AddDays(2), DateTime.Now.AddDays(7), "SomeNotes");
        }

        [TestMethod]
        public void CallProcedure_InvalidTripId_ReturnsEmpty()
        {
            WaypointDal testDal = new(_connection);

            var result = testDal.GetWaypointsByTripId(-1);

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void CallProcedure_ValidTripWithWaypoint_ReturnsList()
        {
            WaypointDal testDal = new(_connection);

            var result = testDal.GetWaypointsByTripId(_testTripId);

            Assert.AreEqual(1, result.Count);
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