using System;
using CapstoneBackend.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;

namespace CapstoneTest.BackendTests.DAL.TestTransportationDAL
{
    [TestClass]
    public class TestEditTransportation
    {
        private readonly MySqlConnection _connection = new(Connection.ConnectionString);
        private int _testTransportationId;

        [TestMethod]
        public void CallProcedure_ReturnsFalse()
        {
            TransportationDal testDal = new(_connection);
            _testTransportationId = testDal.CreateTransportation(1, "Car", DateTime.Now, DateTime.Now, "Some Notes");
            var testTransportation = testDal.GetTransportationById(_testTransportationId);
            Assert.IsNotNull(testTransportation);
            testDal.RemoveTransportation(_testTransportationId);

            Assert.IsFalse(testDal.EditTransportation(testTransportation));
        }

        [TestMethod]
        public void CallProcedure_ReturnsTrue()
        {
            TransportationDal testDal = new(_connection);
            _testTransportationId = testDal.CreateTransportation(1, "Car", DateTime.Now, DateTime.Now, "Some Notes");
            var testTransportation = testDal.GetTransportationById(_testTransportationId);
            Assert.IsNotNull(testTransportation);

            testTransportation.Method = "Bus";
            Assert.IsTrue(testDal.EditTransportation(testTransportation));
        }
    }
}