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
        private int testTripId;

        [TestInitialize]
        public void Setup()
        {
            _connection = new MySqlConnection(Connection.ConnectionString);
            new TripDal(_connection).CreateTrip(1, "TestTrip", "Some Notes", DateTime.Now, DateTime.Now);
        }

        [TestMethod]
        public void CallProcedure_WithInvalidUserId_ReturnsEmpty()
        {
            TripDal testDAL = new(_connection);

            IList<Trip> resultList = testDAL.GetTripsByUserId(-1);

            Assert.AreEqual(0, resultList.Count);
        }

        [TestMethod]
        public void CallProcedure_WithValidUserId_Succeeds()
        {
            TripDal testDAL = new(_connection);

            IList<Trip> resultList = testDAL.GetTripsByUserId(1);

            Assert.IsTrue(resultList.Count > 0);
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
