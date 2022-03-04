using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using System;

namespace CapstoneTest.BackendTests.DAL.TestTripDAL
{
    [TestClass]
    public class TestCreateTrip
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

            Assert.ThrowsException<MySqlException>(() => testDAL.CreateTrip(-1, "Some Trip", "Some Notes", DateTime.Now, DateTime.Now.AddSeconds(10)));
        }

        [TestMethod]
        public void CallProcedure_WithValidInput_Suceeds()
        {
            TripDal testDAL = new(_connection);

            int? resultId = testDAL.CreateTrip(testUserId, "UnitTestTrip", "Some Notes", DateTime.Now, DateTime.Now.AddSeconds(10));

            Assert.IsTrue(resultId is not null);
            Assert.IsTrue(resultId is int);
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
