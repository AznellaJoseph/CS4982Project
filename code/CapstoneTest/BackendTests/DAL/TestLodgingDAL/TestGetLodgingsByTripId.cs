﻿using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace CapstoneTest.BackendTests.DAL.TestLodgingDAL
{
    [TestClass]
    public class TestGetLodgingsByTripId
    {
        private MySqlConnection _connection;
        private int testTripId;
        private int testLodgingId;

        [TestInitialize]
        public void Setup()
        {
            _connection = new MySqlConnection(Connection.ConnectionString);
            testTripId = new TripDal(_connection).CreateTrip(1, "TestTrip", "Some Notes", DateTime.Now, DateTime.Now.AddDays(7));
            testLodgingId = new LodgingDal(_connection).CreateLodging(testTripId, "TestLocation", DateTime.Now.AddDays(2), DateTime.Now.AddDays(7), "SomeNotes");
        }

        [TestMethod]
        public void CallProcedure_InvalidTripId_ReturnsEmpty()
        {
            LodgingDal testDAL = new(_connection);

            IList<Lodging> result = testDAL.GetLodgingsByTripId(-1);

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void CallProcedure_ValidTripWithLodging_ReturnsList()
        {
            LodgingDal testDAL = new(_connection);

            IList<Lodging> result = testDAL.GetLodgingsByTripId(testTripId);

            Assert.AreEqual(1, result.Count);
        }

        [TestCleanup]
        public void TearDown()
        {
            _connection.Open();
            string removeTrip = $"delete from trip where tripId = {testTripId};";
            string removeLodging = $"delete from lodging where lodgingId = {testLodgingId};";

            using MySqlCommand tripCmd = new MySqlCommand(removeTrip, _connection);
            tripCmd.ExecuteNonQuery();

            using MySqlCommand lodgingCmd = new MySqlCommand(removeLodging, _connection);
            lodgingCmd.ExecuteNonQuery();

            this._connection.Close();
        }
    }
}