using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;

namespace CapstoneTest.BackendTests.DAL.TestUserDAL
{
    [TestClass]
    public class TestGetUserByUsername
    {
        private MySqlConnection _connection;

        [TestInitialize]
        public void Setup()
        {
            _connection = new MySqlConnection(Connection.ConnectionString);
        }

        [TestCleanup]
        public void TearDown()
        {
            DeleteTestUser();
        }

        [TestMethod]
        public void CallProcedure_NoExistingUsername_ReturnsNull()
        {
            UserDal testDAL = new(_connection);

            var result = testDAL.GetUserByUsername("TestUsername");

            Assert.IsTrue(result is null);
        }

        [TestMethod]
        public void CallProcedure_ExistingUsername_ReturnsUser()
        {
            InsertTestUser();
            UserDal testDAL = new(_connection);

            var result = testDAL.GetUserByUsername("TestUsername");

            Assert.IsTrue(result is not null);
            Assert.IsTrue(result is User);
        }

        private void InsertTestUser()
        {
            this._connection.Open();
            string query = "INSERT user (username, password, fname, lname) " +
                           "VALUES ('TestUsername', 'TestPassword', 'TestFirstName', 'TestLastName');";

            using MySqlCommand cmd = new MySqlCommand(query, _connection);
            cmd.ExecuteNonQuery();
            this._connection.Close();
        }

        private void DeleteTestUser()
        {
            _connection.Open();
            string query = "delete from user where username = 'TestUsername';";

            using MySqlCommand cmd = new MySqlCommand(query, _connection);
            cmd.ExecuteNonQuery();
            this._connection.Close();
        }
    }
}
