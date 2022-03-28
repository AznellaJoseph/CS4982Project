using System;
using CapstoneBackend.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;

namespace CapstoneTest.BackendTests.DAL.TestWaypointDAL
{
    [TestClass]
    public class TestCreateWaypoint
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
        public void CallProcedure_WithIvalidTripId_Fails()
        {
            WaypointDal testDal = new(_connection);

            Assert.ThrowsException<MySqlException>(() =>
                testDal.CreateWaypoint(-1, "TestLocation", DateTime.Now, DateTime.Now, "Some Notes"));
        }

        [TestMethod]
        public void CallProcedure_WithValidInput_Succeeds()
        {
            WaypointDal testDal = new(_connection);

            int? resultId =
                testDal.CreateWaypoint(_testTripId, "TestLocation", DateTime.Now, DateTime.Now, "Some Notes");
            _testWaypointId = (int) resultId;

            Assert.IsTrue(resultId is not null);
            Assert.IsInstanceOfType(resultId, typeof(int));
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