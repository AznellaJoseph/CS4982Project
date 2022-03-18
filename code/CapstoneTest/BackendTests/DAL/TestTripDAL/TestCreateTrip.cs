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
        private int testTripId = -1;

        [TestInitialize]
        public void Setup()
        {
            _connection = new MySqlConnection(Connection.ConnectionString);
        }

        [TestMethod]
        public void CallProcedure_WithIvalidUserId_Fails()
        {
            TripDal testDAL = new(_connection);

            Assert.ThrowsException<MySqlException>(() => testDAL.CreateTrip(-1, "TestTrip", "Some Notes", DateTime.Now, DateTime.Now));
        }

        [TestMethod]
        public void CallProcedure_WithValidInput_Succeeds()
        {
            TripDal testDAL = new(_connection);

            int? resultID = testDAL.CreateTrip(1, "TestTrip", "Some Notes", DateTime.Now, DateTime.Now);
            testTripId = (int) resultID;

            Assert.IsTrue(resultID is not null);
            Assert.IsTrue(resultID is int);
        }

        [TestCleanup]
        public void TearDown()
        {
            _connection.Open();
            string query = $"delete from trip where tripId = {testTripId};";

            using MySqlCommand cmd = new MySqlCommand(query, _connection);
            cmd.ExecuteNonQuery();
            this._connection.Close();
        }
    }
}
