using System;
using CapstoneBackend.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;

namespace CapstoneTest.BackendTests.DAL.TestLodgingDAL
{
    [TestClass]
    public class TestRemoveLodging
    {
        private readonly MySqlConnection _connection = new(Connection.ConnectionString);
        private int _testLodgingId;
        private int _testTripId;

        [TestInitialize]
        public void Setup()
        {
            _testTripId = new TripDal(_connection).CreateTrip(1, "TestTrip", "Some Notes", DateTime.Now, DateTime.Now);
        }

        [TestMethod]
        public void CallProcedure_WithInvalidLodgingId_ReturnsFalse()
        {
            LodgingDal testDal = new(_connection);

            var result = testDal.RemoveLodging(-1);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CallProcedure_WithValidLodgingId_ReturnsTrue()
        {
            LodgingDal testDal = new(_connection);
            _testLodgingId = testDal.CreateLodging(1, "TestLocation", DateTime.Now, DateTime.Now, "Some Notes");

            var result = testDal.RemoveLodging(_testLodgingId);

            Assert.IsTrue(result);
        }

        [TestCleanup]
        public void TearDown()
        {
            _connection.Close();
            _connection.Open();
            var removeTrip = $"delete from trip where tripId = {_testTripId};";
            var removeLodging = $"delete from lodging where lodgingId = {_testLodgingId};";

            using var tripCmd = new MySqlCommand(removeTrip, _connection);
            tripCmd.ExecuteNonQuery();

            using var lodgingCmd = new MySqlCommand(removeLodging, _connection);
            lodgingCmd.ExecuteNonQuery();

            _connection.Close();
        }
    }
}