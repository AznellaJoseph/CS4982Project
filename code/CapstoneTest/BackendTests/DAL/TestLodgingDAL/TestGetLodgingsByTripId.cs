using System;
using CapstoneBackend.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;

namespace CapstoneTest.BackendTests.DAL.TestLodgingDAL
{
    [TestClass]
    public class TestGetLodgingsByTripId
    {
        private readonly MySqlConnection _connection = new(Connection.ConnectionString);
        private int _testLodgingId;
        private int _testTripId;

        [TestInitialize]
        public void Setup()
        {
            _testTripId =
                new TripDal(_connection).CreateTrip(1, "TestTrip", "Some Notes", DateTime.Now, DateTime.Now.AddDays(7));
            _testLodgingId = new LodgingDal(_connection).CreateLodging(_testTripId, "TestLocation",
                DateTime.Now.AddDays(2), DateTime.Now.AddDays(7), "SomeNotes");
        }

        [TestMethod]
        public void CallProcedure_InvalidTripId_ReturnsEmpty()
        {
            LodgingDal testDal = new(_connection);

            var result = testDal.GetLodgingsByTripId(-1);

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void CallProcedure_ValidTripWithLodging_ReturnsList()
        {
            LodgingDal testDal = new(_connection);

            var result = testDal.GetLodgingsByTripId(_testTripId);

            Assert.AreEqual(1, result.Count);
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