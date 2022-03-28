using System;
using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;

namespace CapstoneTest.BackendTests.DAL.TestTransportationDAL
{
    [TestClass]
    public class TestGetTransportationById
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
        public void CallProcedure_WithInvalidTransportationId_ReturnsNull()
        {
            TransportationDal testDal = new(_connection);

            var result = testDal.GetTransportationById(-1);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void CallProcedure_WithValidTransportationId_ReturnsTransportation()
        {
            TransportationDal testDal = new(_connection);
            _testTransportationId =
                testDal.CreateTransportation(1, "TestMethod", DateTime.Now, DateTime.Now, "Some Notes");

            var result = testDal.GetTransportationById(1);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Transportation));
        }

        [TestCleanup]
        public void TearDown()
        {
            _connection.Open();
            var removeTrip = $"delete from trip where tripId = {_testTripId};";
            var removeWaypoint = $"delete from waypoint where waypointId = {_testTransportationId};";

            using var tripCmd = new MySqlCommand(removeTrip, _connection);
            tripCmd.ExecuteNonQuery();

            using var waypointCmd = new MySqlCommand(removeWaypoint, _connection);
            waypointCmd.ExecuteNonQuery();

            _connection.Close();
        }
    }
}