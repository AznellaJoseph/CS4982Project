using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using System;

namespace CapstoneTest.BackendTests.DAL.TestLodgingDAL
{
    [TestClass]
    public class TestCreateLodging
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
        public void CallProcedure_WithInvalidTripId_Fails()
        {
            LodgingDal testDAL = new(_connection);

            Assert.ThrowsException<MySqlException>(() => testDAL.CreateLodging(-1, "TestLocation", DateTime.Now, DateTime.Now, "Some Notes"));
        }

        [TestMethod]
        public void CallProcedure_WithValidInput_Succeeds()
        {
            LodgingDal testDAL = new(_connection);

            int? resultID = testDAL.CreateLodging(testTripId, "TestLocation", DateTime.Now, DateTime.Now, "Some Notes");
            testLodgingId = (int) resultID;

            Assert.IsTrue(resultID.HasValue);
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
