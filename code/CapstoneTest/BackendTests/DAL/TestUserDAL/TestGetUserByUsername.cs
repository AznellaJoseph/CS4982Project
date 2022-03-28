using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;

namespace CapstoneTest.BackendTests.DAL.TestUserDAL
{
    [TestClass]
    public class TestGetUserByUsername
    {
        private readonly MySqlConnection _connection = new(Connection.ConnectionString);

        [TestCleanup]
        public void TearDown()
        {
            DeleteTestUser();
        }

        [TestMethod]
        public void CallProcedure_NoExistingUsername_ReturnsNull()
        {
            UserDal testDal = new(_connection);

            var result = testDal.GetUserByUsername("TestUsername");

            Assert.IsTrue(result is null);
        }

        [TestMethod]
        public void CallProcedure_ExistingUsername_ReturnsUser()
        {
            InsertTestUser();
            UserDal testDal = new(_connection);

            var result = testDal.GetUserByUsername("TestUsername");

            Assert.IsTrue(result is not null);
            Assert.IsInstanceOfType(result, typeof(User));
        }

        private void InsertTestUser()
        {
            _connection.Open();
            const string query = "INSERT user (username, password, fname, lname) " +
                                 "VALUES ('TestUsername', 'TestPassword', 'TestFirstName', 'TestLastName');";

            using var cmd = new MySqlCommand(query, _connection);
            cmd.ExecuteNonQuery();
            _connection.Close();
        }

        private void DeleteTestUser()
        {
            _connection.Open();
            const string query = "delete from user where username = 'TestUsername';";

            using var cmd = new MySqlCommand(query, _connection);
            cmd.ExecuteNonQuery();
            _connection.Close();
        }
    }
}