using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;

namespace CapstoneTest.BackendTests.DAL.TestUserDAL
{

    [TestClass]
    public class TestCreateUser
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
        public void Call_WithDuplicateUsername_Fails()
        {
            InsertTestUser();

            UserDal testDAL = new(_connection);

            var result = testDAL.CreateUser("TestUsername", "SomePassword", "SomeName", "SomeName");

            Assert.IsTrue(result is null);
        }

        [TestMethod]
        public void Call_WithValidInput_Succeeds()
        {
            UserDal testDAL = new(_connection);

            var resultID = testDAL.CreateUser("TestUsername", "SomePassword", "SomeName", "SomeName");
            var resultUser = testDAL.GetUserByUsername("TestUsername");

            Assert.IsTrue(resultID is int);
            Assert.IsTrue(resultUser is not null);
            Assert.IsTrue(resultUser is User);
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
