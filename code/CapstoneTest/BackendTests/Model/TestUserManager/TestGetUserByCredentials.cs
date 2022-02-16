using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace CapstoneTest.BackendTests.Model.TestUserManager
{

    [TestClass]
    public class TestGetUserByCredentials
    {
        private const string TEST_USERNAME = "TestUsername";
        private const string TEST_PASSWORD = "TestPassword";

        [TestMethod]
        public void Call_WithValidCredentials_Succeeds()
        {
            User fakeExistingUser = new() { Password = PasswordHasher.Hash(TEST_PASSWORD) };
            var mockUserDAL = new Mock<UserDal>();
            mockUserDAL.Setup(db => db.GetUserByUsername(TEST_USERNAME)).Returns(fakeExistingUser);

            UserManager userManager = new(mockUserDAL.Object);

            var resultResponse = userManager.GetUserByCredentials(TEST_USERNAME, TEST_PASSWORD);

            Assert.AreEqual(200U, resultResponse.StatusCode);
            Assert.IsInstanceOfType(resultResponse.Data, typeof(User));
            Assert.AreEqual(resultResponse?.Data?.FirstName, fakeExistingUser.FirstName);
            Assert.AreEqual(resultResponse?.Data?.LastName, fakeExistingUser.LastName);
            Assert.AreEqual(resultResponse?.Data?.Username, fakeExistingUser.Username);
            Assert.AreEqual(resultResponse?.Data?.Password, fakeExistingUser.Password);
            Assert.AreEqual(resultResponse?.Data?.UserId, fakeExistingUser.UserId);
        }

        [TestMethod]
        public void Call_WithUnknownUsername_Fails()
        {
            User? missingUser = null;
            var mockUserDAL = new Mock<UserDal>();
            mockUserDAL.Setup(db => db.GetUserByUsername(TEST_USERNAME)).Returns(missingUser);

            UserManager userManager = new(mockUserDAL.Object);

            var resultResponse = userManager.GetUserByCredentials(TEST_USERNAME, TEST_PASSWORD);

            Assert.AreEqual(404U, resultResponse.StatusCode);
        }

        [TestMethod]
        public void Call_WithWrongPassword_Fails()
        {
            User fakeExistingUser = new() { Password = PasswordHasher.Hash("CorrectPassword") };
            var mockUserDAL = new Mock<UserDal>();
            mockUserDAL.Setup(db => db.GetUserByUsername(TEST_USERNAME)).Returns(fakeExistingUser);

            UserManager userManager = new(mockUserDAL.Object);

            var resultResponse = userManager.GetUserByCredentials(TEST_USERNAME, TEST_PASSWORD);

            Assert.AreEqual(404U, resultResponse.StatusCode);
        }
    }
}