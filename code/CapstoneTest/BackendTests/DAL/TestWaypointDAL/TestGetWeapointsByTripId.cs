using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace CapstoneTest.BackendTests.DAL.TestWaypointDAL
{
    [TestClass]
    public class TestGetWeapointsByTripId
    {
        private MySqlConnection _connection;
        private int testUserId;
        private int testTripId;

        [TestInitialize]
        public void Setup()
        {
            _connection = new MySqlConnection(Connection.ConnectionString);
            testUserId = InsertTestUser();
            testTripId = InsertTestTrip();
        }

        [TestCleanup]
        public void TearDown()
        {
            DeleteTestTrip();
            DeleteTestUser();
        }

        [TestMethod]
        public void CallProcedure_InvalidTripId_ReturnsEmpty()
        {
            WaypointDal testDal = new(_connection);

            var result = testDal.GetWaypointsByTripId(-1);

            Assert.IsNotNull(result);
            Assert.IsTrue(result is IList<Waypoint>);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void CallProcedure_InvalidTripId_ReturnsList()
        {
            WaypointDal testDal = new(_connection);
            int newWaypoint = testDal.CreateWaypoint(testTripId, "San Jose", DateTime.Now, DateTime.Now.AddSeconds(10), "Some Notes");

            var result = testDal.GetWaypointsByTripId(testTripId);

            Assert.IsNotNull(result);
            Assert.IsTrue(result is IList<Waypoint>);
            Assert.AreEqual(1, result.Count);

            testDal.RemoveWaypoint(newWaypoint);
        }

        private int InsertTestUser()
        {
            UserDal userDal = new(_connection);
            return userDal.CreateUser("SomeUsername", "SomePassword", "First", "Last");
        }

        private void DeleteTestUser()
        {
            _connection.Open();
            string query = "delete from user where userId = " + testUserId + ";";

            using MySqlCommand cmd = new MySqlCommand(query, _connection);
            cmd.ExecuteNonQuery();
            this._connection.Close();
        }

        private int InsertTestTrip()
        {
            TripDal tripDal = new(_connection);
            return tripDal.CreateTrip(testUserId, "UnitTestTrip", "Some Notes", DateTime.Now, DateTime.Now.AddSeconds(10));
        }

        private void DeleteTestTrip()
        {
            _connection.Open();
            string query = "delete from trip where tripId = " + testTripId + ";";

            using MySqlCommand cmd = new MySqlCommand(query, _connection);
            cmd.ExecuteNonQuery();
            this._connection.Close();
        }
    }
}
