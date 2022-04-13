using System;
using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;

namespace CapstoneTest.BackendTests.DAL.TestTripDAL
{
    [TestClass]
    public class TestGetTripByTripId
    {
        private readonly MySqlConnection _connection = new(Connection.ConnectionString);
        private int _testTripId;

        [TestMethod]
        public void CallProcedure_WithInvalidTripId_ReturnsNull()
        {
            TripDal testDal = new(_connection);

            var result = testDal.GetTripByTripId(-1);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void CallProcedure_WithValidTripId_Succeeds()
        {
            TripDal testDal = new(_connection);
            _testTripId = testDal.CreateTrip(1, "TestTrip", "Some Notes", DateTime.Now, DateTime.Now);

            var result = testDal.GetTripByTripId(1);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Trip));
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