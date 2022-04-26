using System;
using CapstoneBackend.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;

namespace CapstoneTest.BackendTests.DAL.TestLodgingDAL
{
    [TestClass]
    public class TestEditLodging
    {
        private readonly MySqlConnection _connection = new(Connection.ConnectionString);
        private int _testLodgingId;

        [TestMethod]
        public void CallProcedure_ReturnsFalse()
        {
            LodgingDal testDal = new(_connection);
            _testLodgingId = testDal.CreateLodging(1, "TestLocation", DateTime.Now, DateTime.Now, "Some Notes");
            var testLodging = testDal.GetLodgingById(_testLodgingId);
            Assert.IsNotNull(testLodging);
            testDal.RemoveLodging(_testLodgingId);

            Assert.IsFalse(testDal.EditLodging(testLodging));
        }

        [TestMethod]
        public void CallProcedure_ReturnsTrue()
        {
            LodgingDal testDal = new(_connection);
            _testLodgingId = testDal.CreateLodging(1, "TestLocation", DateTime.Now, DateTime.Now, "Some Notes");
            var testLodging = testDal.GetLodgingById(_testLodgingId);
            Assert.IsNotNull(testLodging);

            testLodging.Location = "Hilton Atlanta";
            Assert.IsTrue(testDal.EditLodging(testLodging));
        }
    }
}