using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using System;

namespace CapstoneTest.BackendTests.DAL.TestTripDAL
{
    [TestClass]
    public class TestGetTripByTripId
    {
        private MySqlConnection _connection;
        private int testTripId;

        [TestInitialize]
        public void Setup()
        {
            _connection = new MySqlConnection(Connection.ConnectionString);
        }

        [TestMethod]
        public void CallProcedure_WithInvalidTripId_ReturnsNull()
        {
            TripDal testDAL = new(_connection);

            Trip? result = testDAL.GetTripByTripId(-1);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void CallProcedure_WithValidTripId_Succeeds()
        {
            TripDal testDAL = new(_connection);
            testTripId = testDAL.CreateTrip(1, "TestTrip", "Some Notes", DateTime.Now, DateTime.Now);

            Trip? result = testDAL.GetTripByTripId(1);

            Assert.IsNotNull(result);
            Assert.IsTrue(result is Trip);
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
