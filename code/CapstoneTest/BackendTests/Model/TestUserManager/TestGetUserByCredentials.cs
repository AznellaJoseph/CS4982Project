using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CapstoneTest.BackendTests.Model.TestUserManager
{
    [TestClass]
    public class TestGetUserByCredentials
    {
        private const string TestUsername = "TestUsername";
        private const string TestPassword = "TestPassword";

        [TestMethod]
        public void Call_WithValidCredentials_Succeeds()
        {
            User fakeExistingUser = new() {Password = PasswordHasher.Hash(TestPassword)};
            var mockUserDal = new Mock<UserDal>();
            mockUserDal.Setup(db => db.GetUserByUsername(TestUsername)).Returns(fakeExistingUser);

            UserManager userManager = new(mockUserDal.Object);

            var resultResponse = userManager.GetUserByCredentials(TestUsername, TestPassword);

            Assert.AreEqual(200U, resultResponse.StatusCode);
            Assert.IsInstanceOfType(resultResponse.Data, typeof(User));
            Assert.AreEqual(resultResponse.Data?.FirstName, fakeExistingUser.FirstName);
            Assert.AreEqual(resultResponse.Data?.LastName, fakeExistingUser.LastName);
            Assert.AreEqual(resultResponse.Data?.Username, fakeExistingUser.Username);
            Assert.AreEqual(resultResponse.Data?.Password, fakeExistingUser.Password);
            Assert.AreEqual(resultResponse.Data?.Id, fakeExistingUser.Id);
        }

        [TestMethod]
        public void Call_WithUnknownUsername_Fails()
        {
            User? missingUser = null;
            var mockUserDal = new Mock<UserDal>();
            mockUserDal.Setup(db => db.GetUserByUsername(TestUsername)).Returns(missingUser);

            UserManager userManager = new(mockUserDal.Object);

            var resultResponse = userManager.GetUserByCredentials(TestUsername, TestPassword);

            Assert.AreEqual(404U, resultResponse.StatusCode);
        }

        [TestMethod]
        public void Call_WithWrongPassword_Fails()
        {
            User fakeExistingUser = new() {Password = PasswordHasher.Hash("CorrectPassword")};
            var mockUserDal = new Mock<UserDal>();
            mockUserDal.Setup(db => db.GetUserByUsername(TestUsername)).Returns(fakeExistingUser);

            UserManager userManager = new(mockUserDal.Object);

            var resultResponse = userManager.GetUserByCredentials(TestUsername, TestPassword);

            Assert.AreEqual(404U, resultResponse.StatusCode);
        }
    }
}