using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;

namespace CapstoneTest.BackendTests.DAL.TestUserDAL
{
    [TestClass]
    public class TestCreateUser
    {
        private readonly MySqlConnection _connection = new(Connection.ConnectionString);

        [TestCleanup]
        public void TearDown()
        {
            DeleteTestUser();
        }

        [TestMethod]
        public void CallProcedure_WithDuplicateUsername_Fails()
        {
            InsertTestUser();
            UserDal testDal = new(_connection);

            Assert.ThrowsException<MySqlException>(() =>
                testDal.CreateUser("TestUsername", "SomePassword", "SomeName", "SomeName"));
        }

        [TestMethod]
        public void CallProcedure_WithValidInput_Succeeds()
        {
            UserDal testDal = new(_connection);

            int? resultId = testDal.CreateUser("TestUsername", "SomePassword", "SomeName", "SomeName");
            var resultUser = testDal.GetUserByUsername("TestUsername");

            Assert.IsTrue(resultId is not null);
            Assert.IsInstanceOfType(resultId, typeof(int));

            Assert.IsTrue(resultUser is not null);
            Assert.IsInstanceOfType(resultUser, typeof(User));
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