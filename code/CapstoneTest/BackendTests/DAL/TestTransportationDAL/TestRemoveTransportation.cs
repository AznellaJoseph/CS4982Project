using System;
using CapstoneBackend.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;

namespace CapstoneTest.BackendTests.DAL.TestTransportationDAL
{
    [TestClass]
    public class TestRemoveTransportation
    {
        private readonly MySqlConnection _connection = new(Connection.ConnectionString);
        private int _testTransportationId;
        private int _testTripId;

        [TestInitialize]
        public void Setup()
        {
            _testTripId = new TripDal(_connection).CreateTrip(1, "TestTrip", "Some Notes", DateTime.Now, DateTime.Now);
        }

        [TestMethod]
        public void CallProcedure_WithInvalidTransportationId_ReturnsFalse()
        {
            TransportationDal testDal = new(_connection);

            var result = testDal.RemoveTransportation(-1);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CallProcedure_WithValidTransportationId_ReturnsTrue()
        {
            TransportationDal testDal = new(_connection);
            _testTransportationId =
                testDal.CreateTransportation(_testTripId, "TestMethod", DateTime.Now, DateTime.Now, "Some Notes");

            var result = testDal.RemoveTransportation(_testTransportationId);

            Assert.IsTrue(result);
        }

        [TestCleanup]
        public void TearDown()
        {
            _connection.Open();
            var removeTrip = $"delete from trip where tripId = {_testTripId};";
            var removeTransportation = $"delete from transportation where transportationId = {_testTransportationId};";

            using var tripCmd = new MySqlCommand(removeTrip, _connection);
            tripCmd.ExecuteNonQuery();

            using var transportationCmd = new MySqlCommand(removeTransportation, _connection);
            transportationCmd.ExecuteNonQuery();

            _connection.Close();
        }
    }
}