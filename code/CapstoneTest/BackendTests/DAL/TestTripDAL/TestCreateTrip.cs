using System;
using CapstoneBackend.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;

namespace CapstoneTest.BackendTests.DAL.TestTripDAL
{
    [TestClass]
    public class TestCreateTrip
    {
        private readonly MySqlConnection _connection = new(Connection.ConnectionString);
        private int _testTripId = -1;

        [TestMethod]
        public void CallProcedure_WithInvalidUserId_Fails()
        {
            TripDal testDal = new(_connection);

            Assert.ThrowsException<MySqlException>(() =>
                testDal.CreateTrip(-1, "TestTrip", "Some Notes", DateTime.Now, DateTime.Now));
        }

        [TestMethod]
        public void CallProcedure_WithValidInput_Succeeds()
        {
            TripDal testDal = new(_connection);

            int? resultId = testDal.CreateTrip(1, "TestTrip", "Some Notes", DateTime.Now, DateTime.Now);
            _testTripId = (int) resultId;

            Assert.IsTrue(resultId is not null);
            Assert.IsInstanceOfType(resultId, typeof(int));
        }

        [TestCleanup]
        public void TearDown()
        {
            _connection.Open();
            var query = $"delete from trip where tripId = {_testTripId};";

            using var cmd = new MySqlCommand(query, _connection);
            cmd.ExecuteNonQuery();
            _connection.Close();
        }
    }
}