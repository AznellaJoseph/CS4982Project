using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace CapstoneTest.BackendTests.DAL.TestTripDAL
{
    [TestClass]
    public class TestGetTripsByUserId
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
        public void CallProcedure_MissingUser_Fails()
        {
            TripDal testDAL = new(_connection);

            IList<Trip> result = testDAL.GetTripsByUserId(-1);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void CallProcedure_WithExistingTrip_Suceeds()
        {
            TripDal testDAL = new(_connection);
            testDAL.CreateTrip(testUserId, "UnitTestTrip", "Some Notes", DateTime.Now, DateTime.Now.AddSeconds(10));

            IList<Trip> result = testDAL.GetTripsByUserId(testUserId);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
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
