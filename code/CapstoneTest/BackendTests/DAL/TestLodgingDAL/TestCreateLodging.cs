using System;
using CapstoneBackend.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;

namespace CapstoneTest.BackendTests.DAL.TestLodgingDAL
{
    [TestClass]
    public class TestCreateLodging
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
        public void CallProcedure_WithInvalidTripId_Fails()
        {
            LodgingDal testDal = new(_connection);
            Assert.ThrowsException<MySqlException>(() =>
                testDal.CreateLodging(-1, "TestLocation", DateTime.Now, DateTime.Now, "Some Notes"));
        }

        [TestMethod]
        public void CallProcedure_WithValidInput_Succeeds()
        {
            LodgingDal testDal = new(_connection);

            int? resultId =
                testDal.CreateLodging(_testTripId, "TestLocation", DateTime.Now, DateTime.Now, "Some Notes");
            _testLodgingId = (int) resultId;

            Assert.IsTrue(resultId.HasValue);
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