using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using System;

namespace CapstoneTest.BackendTests.DAL.TestTransportationDAL
{
    [TestClass]
    public class TestRemoveTransportation
    {
        private MySqlConnection _connection;
        private int testTripId;
        private int testTransportationId;

        [TestInitialize]
        public void Setup()
        {
            _connection = new MySqlConnection(Connection.ConnectionString);
            testTripId = new TripDal(_connection).CreateTrip(1, "TestTrip", "Some Notes", DateTime.Now, DateTime.Now);
        }

        [TestMethod]
        public void CallProcedure_WithInvalidWaypointId_ReturnsFalse()
        {
            TransportationDal testDAL = new(_connection);

            bool result = testDAL.RemoveTransportation(-1);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CallProcedure_WithValidWaypointId_ReturnsTrue()
        {
            TransportationDal testDAL = new(_connection);
            testTransportationId = testDAL.CreateTransportation(testTripId, "TestMethod", DateTime.Now, DateTime.Now, "Some Notes");

            bool result = testDAL.RemoveTransportation(testTransportationId);

            Assert.IsTrue(result);
        }

        [TestCleanup]
        public void TearDown()
        {
            _connection.Open();
            string removeTrip = $"delete from trip where tripId = {testTripId};";
            string removeTransportation = $"delete from transportation where transportationId = {testTransportationId};";

            using MySqlCommand tripCmd = new MySqlCommand(removeTrip, _connection);
            tripCmd.ExecuteNonQuery();

            using MySqlCommand transportationCmd = new MySqlCommand(removeTransportation, _connection);
            transportationCmd.ExecuteNonQuery();

            this._connection.Close();
        }
    }
}
