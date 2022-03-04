using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace CapstoneTest.BackendTests.DAL.TestTripDAL
{
    [TestClass]
    public class TestGetTripByTripId
    {
        private MySqlConnection _connection;
        private int testUserId;

        [TestInitialize]
        public void Setup()
        {
            _connection = new MySqlConnection(Connection.ConnectionString);
            testUserId = InsertTestUser();
        }

        [TestCleanup]
        public void TearDown()
        {
            DeleteTestTrip();
            DeleteTestUser();
        }

        [TestMethod]
        public void CallProcedure_NoTripExists_ReturnsNull()
        {
            TripDal testDAL = new(_connection);

            Trip? result = testDAL.GetTripByTripId(-1);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void CallProcedure_TripExists_ReturnsTrip()
        {
            TripDal testDAL = new(_connection);
            int tripId = testDAL.CreateTrip(testUserId, "UnitTestTrip", "Some Notes", DateTime.Now, DateTime.Now.AddSeconds(10));

            var result = testDAL.GetTripByTripId(tripId);

            Assert.IsNotNull(result);
            Assert.IsTrue(result is Trip);
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

        private void DeleteTestTrip()
        {
            _connection.Open();
            string query = "delete from trip where name = 'UnitTestTrip';";

            using MySqlCommand cmd = new MySqlCommand(query, _connection);
            cmd.ExecuteNonQuery();
            this._connection.Close();
        }
    }
}
