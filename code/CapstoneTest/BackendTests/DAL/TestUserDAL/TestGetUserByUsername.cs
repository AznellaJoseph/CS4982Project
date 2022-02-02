using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;

namespace CapstoneTest.BackendTests.DAL.TestUserDal
{

    [TestClass]
    public class TestGetUserByUsername
    {
        private MySqlConnection _connection;

        [TestInitialize]
        public void Setup()
        {
            this._connection = new MySqlConnection(Connection.ConnectionString);
        }


        [TestCleanup]
        public void TearDown()
        {
            DeleteTestUser();
        }

        [TestMethod]
        public void Call_WithMissingUser_Fails()
        {
            UserDal testDAL = new(_connection);

            var result = testDAL.GetUserByUsername("TestUsername");

            Assert.IsTrue(result is null);
        }

        [TestMethod]
        public void Call_WithExistingUser_Succeeds()
        {
            InsertTestUser();

            UserDal testDAL = new(_connection);

            var result = testDAL.GetUserByUsername("TestUsername");

            Assert.IsTrue(result is User);
            Assert.IsTrue(result is not null);
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
            this._connection.Open();
            string query = "delete from user where username = 'TestUsername';";

            using MySqlCommand cmd = new MySqlCommand(query, _connection);
            cmd.ExecuteNonQuery();
            this._connection.Close();
        }
    }
}