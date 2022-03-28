using System;
using CapstoneBackend.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;

namespace CapstoneTest.BackendTests.DAL.TestTransportationDAL
{
    [TestClass]
    public class TestCreateTransportation
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
        public void CallProcedure_WithIvalidTripId_Fails()
        {
            TransportationDal testDal = new(_connection);

            Assert.ThrowsException<MySqlException>(() =>
                testDal.CreateTransportation(-1, "TestMethod", DateTime.Now, DateTime.Now, "Some Notes"));
        }

        [TestMethod]
        public void CallProcedure_WithValidInput_Succeeds()
        {
            TransportationDal testDal = new(_connection);

            int? resultId =
                testDal.CreateTransportation(_testTripId, "TestMethod", DateTime.Now, DateTime.Now, "Some Notes");
            _testTransportationId = (int) resultId;

            Assert.IsTrue(resultId is not null);
            Assert.IsInstanceOfType(resultId, typeof(int));
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