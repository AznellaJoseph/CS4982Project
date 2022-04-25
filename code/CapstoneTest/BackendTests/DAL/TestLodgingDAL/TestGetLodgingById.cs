using System;
using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;

namespace CapstoneTest.BackendTests.DAL.TestLodgingDAL
{
    [TestClass]
    public class TestGetLodgingById
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
        public void CallProcedure_WithInvalidLodgingId_ReturnsNull()
        {
            LodgingDal testDal = new(_connection);

            var result = testDal.GetLodgingById(-1);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void CallProcedure_WithValidLodgingId_ReturnsLodging()
        {
            LodgingDal testDal = new(_connection);
            _testLodgingId =
                testDal.CreateLodging(1, "TestMethod", DateTime.Now, DateTime.Now, "Some Notes");

            var result = testDal.GetLodgingById(1);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Lodging));
        }

        [TestCleanup]
        public void TearDown()
        {
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