using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using System;

namespace CapstoneTest.BackendTests.DAL.TestLodgingDAL
{
    [TestClass]
    public class TestRemoveLodging
    {
        private MySqlConnection _connection;
        private int testTripId;
        private int testLodgingId;

        [TestInitialize]
        public void Setup()
        {
            _connection = new MySqlConnection(Connection.ConnectionString);
            testTripId = new TripDal(_connection).CreateTrip(1, "TestTrip", "Some Notes", DateTime.Now, DateTime.Now);
        }

        [TestMethod]
        public void CallProcedure_WithInvalidLodgingId_ReturnsFalse()
        {
            LodgingDal testDAL = new(_connection);

            bool result = testDAL.RemoveLodging(-1);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CallProcedure_WithValidLodgingId_ReturnsTrue()
        {
            LodgingDal testDAL = new(_connection);
            testLodgingId = testDAL.CreateLodging(1, "TestLocation", DateTime.Now, DateTime.Now, "Some Notes");

            bool result = testDAL.RemoveLodging(testLodgingId);

            Assert.IsTrue(result);
        }

        [TestCleanup]
        public void TearDown()
        {
            _connection.Open();
            string removeTrip = $"delete from trip where tripId = {testTripId};";
            string removeLodging = $"delete from lodging where lodgingId = {testLodgingId};";

            using MySqlCommand tripCmd = new MySqlCommand(removeTrip, _connection);
            tripCmd.ExecuteNonQuery();

            using MySqlCommand lodgingCmd = new MySqlCommand(removeLodging, _connection);
            lodgingCmd.ExecuteNonQuery();

            this._connection.Close();
        }
    }
}
