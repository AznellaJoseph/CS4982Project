using System;
using CapstoneBackend.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;

namespace CapstoneTest.BackendTests.DAL.TestTransportationDAL
{
    [TestClass]
    public class TestGetTransportationOnDate
    {
        private readonly MySqlConnection _connection = new(Connection.ConnectionString);
        private int _testTransportationId;
        private int _testTripId;

        [TestInitialize]
        public void Setup()
        {
            _testTripId =
                new TripDal(_connection).CreateTrip(1, "TestTrip", "Some Notes", DateTime.Now, DateTime.Now.AddDays(7));
            _testTransportationId = new TransportationDal(_connection).CreateTransportation(_testTripId, "TestMethod",
                DateTime.Now.AddDays(2), DateTime.Now.AddDays(7), null);
        }

        [TestMethod]
        public void CallProcedure_WithNoTransportationOnDate_ReturnsEmpty()
        {
            TransportationDal testDal = new(_connection);

            var result = testDal.GetTransportationOnDate(_testTripId, DateTime.Now);

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void CallProcedure_WithTransportationOnDate_ReturnsList()
        {
            TransportationDal testDal = new(_connection);

            var result = testDal.GetTransportationOnDate(_testTripId, DateTime.Now.AddDays(3));

            Assert.AreEqual(1, result.Count);
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