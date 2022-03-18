using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace CapstoneTest.BackendTests.DAL.TestTransportationDAL
{
    [TestClass]
    public class TestGetTransportationOnDate
    {
        private MySqlConnection _connection;
        private int testTripId;
        private int testTransportationId;

        [TestInitialize]
        public void Setup()
        {
            _connection = new MySqlConnection(Connection.ConnectionString);
            testTripId = new TripDal(_connection).CreateTrip(1, "TestTrip", "Some Notes", DateTime.Now, DateTime.Now.AddDays(7));
            testTransportationId = new TransportationDal(_connection).CreateTransportation(testTripId, "TestMethod", DateTime.Now.AddDays(2), DateTime.Now.AddDays(7), null);
        }

        [TestMethod]
        public void CallProcedure_WithNoTransportationOnDate_ReturnsEmpty()
        {
            TransportationDal testDAL = new(_connection);

            IList<Transportation> result = testDAL.GetTransportationOnDate(testTripId, DateTime.Now);

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void CallProcedure_WithTransportationOnDate_ReturnsList()
        {
            TransportationDal testDAL = new(_connection);

            IList<Transportation> result = testDAL.GetTransportationOnDate(testTripId, DateTime.Now.AddDays(3));

            Assert.AreEqual(1, result.Count);
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
