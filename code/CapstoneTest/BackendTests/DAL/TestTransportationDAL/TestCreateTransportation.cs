using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using System;

namespace CapstoneTest.BackendTests.DAL.TestTransportationDAL
{
    [TestClass]
    public class TestCreateTransportation
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
        public void CallProcedure_WithIvalidTripId_Fails()
        {
            TransportationDal testDAL = new(_connection);

            Assert.ThrowsException<MySqlException>(() => testDAL.CreateTransportation(-1, "TestMethod", DateTime.Now, DateTime.Now, "Some Notes"));
        }

        [TestMethod]
        public void CallProcedure_WithValidInput_Succeeds()
        {
            TransportationDal testDAL = new(_connection);

            int? resultID = testDAL.CreateTransportation(testTripId, "TestMethod", DateTime.Now, DateTime.Now, "Some Notes");
            testTransportationId = (int) resultID;

            Assert.IsTrue(resultID is not null);
            Assert.IsTrue(resultID is int);
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
