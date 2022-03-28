using System;
using CapstoneBackend.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;

namespace CapstoneTest.BackendTests.DAL.TestTripDAL
{
    [TestClass]
    public class TestGetTripsByUserId
    {
        private readonly MySqlConnection _connection = new(Connection.ConnectionString);
        private int _testTripId;

        [TestInitialize]
        public void Setup()
        {
            _testTripId = new TripDal(_connection).CreateTrip(1, "TestTrip", "Some Notes", DateTime.Now, DateTime.Now);
        }

        [TestMethod]
        public void CallProcedure_WithInvalidUserId_ReturnsEmpty()
        {
            TripDal testDal = new(_connection);

            var resultList = testDal.GetTripsByUserId(-1);

            Assert.AreEqual(0, resultList.Count);
        }

        [TestMethod]
        public void CallProcedure_WithValidUserId_Succeeds()
        {
            TripDal testDal = new(_connection);

            var resultList = testDal.GetTripsByUserId(1);

            Assert.IsTrue(resultList.Count > 0);
        }

        [TestCleanup]
        public void TearDown()
        {
            _connection.Open();
            var query = $"delete from trip where tripId = {_testTripId};";

            using var cmd = new MySqlCommand(query, _connection);
            cmd.ExecuteNonQuery();
            _connection.Close();
        }
    }
}