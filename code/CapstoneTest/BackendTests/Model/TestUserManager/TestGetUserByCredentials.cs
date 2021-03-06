using System;
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
        public void GetUserByCredentials_Success()
        {
            User fakeExistingUser = new() {Password = PasswordHasher.Hash(TestPassword)};
            var mockUserDal = new Mock<UserDal>();
            mockUserDal.Setup(db => db.GetUserByUsername(TestUsername)).Returns(fakeExistingUser);

            UserManager userManager = new(mockUserDal.Object);

            var resultResponse = userManager.GetUserByCredentials(TestUsername, TestPassword);

            Assert.AreEqual((uint) Ui.StatusCode.Success, resultResponse.StatusCode);
            Assert.IsInstanceOfType(resultResponse.Data, typeof(User));
            Assert.AreEqual(resultResponse.Data?.FirstName, fakeExistingUser.FirstName);
            Assert.AreEqual(resultResponse.Data?.LastName, fakeExistingUser.LastName);
            Assert.AreEqual(resultResponse.Data?.Username, fakeExistingUser.Username);
            Assert.AreEqual(resultResponse.Data?.Password, fakeExistingUser.Password);
            Assert.AreEqual(resultResponse.Data?.UserId, fakeExistingUser.UserId);
        }

        [TestMethod]
        public void GetUserByCredentials_WithUnknownUsername_ReturnsErrorMessage()
        {
            User? missingUser = null;
            var mockUserDal = new Mock<UserDal>();
            mockUserDal.Setup(db => db.GetUserByUsername(TestUsername)).Returns(missingUser);

            UserManager userManager = new(mockUserDal.Object);

            var resultResponse = userManager.GetUserByCredentials(TestUsername, TestPassword);

            Assert.AreEqual((uint) Ui.StatusCode.DataNotFound, resultResponse.StatusCode);
        }

        [TestMethod]
        public void GetUserByCredentials_WithWrongPassword_ReturnsErrorMessage()
        {
            User fakeExistingUser = new() {Password = PasswordHasher.Hash("CorrectPassword")};
            var mockUserDal = new Mock<UserDal>();
            mockUserDal.Setup(db => db.GetUserByUsername(TestUsername)).Returns(fakeExistingUser);

            UserManager userManager = new(mockUserDal.Object);

            var resultResponse = userManager.GetUserByCredentials(TestUsername, TestPassword);

            Assert.AreEqual((uint) Ui.StatusCode.DataNotFound, resultResponse.StatusCode);
        }

        [TestMethod]
        public void Call_ServerMySqlException_ReturnsErrorMessage()
        {
            var mockUserDal = new Mock<UserDal>();
            var builder = new MySqlExceptionBuilder();
            mockUserDal.Setup(db => db.GetUserByUsername(TestUsername))
                .Throws(builder
                    .WithError((uint) Ui.StatusCode.InternalServerError, Ui.ErrorMessages.InternalServerError).Build());
            UserManager userManager = new(mockUserDal.Object);

            var resultResponse = userManager.GetUserByCredentials(TestUsername, TestPassword);

            Assert.AreEqual((uint) Ui.StatusCode.InternalServerError, resultResponse.StatusCode);
            Assert.AreEqual(Ui.ErrorMessages.InternalServerError, resultResponse.ErrorMessage);
        }

        [TestMethod]
        public void GetUserByCredentials_ServerException_ReturnsErrorMessage()
        {
            var mockUserDal = new Mock<UserDal>();
            mockUserDal.Setup(db => db.GetUserByUsername(TestUsername))
                .Throws(new Exception());
            UserManager userManager = new(mockUserDal.Object);

            var resultResponse = userManager.GetUserByCredentials(TestUsername, TestPassword);

            Assert.AreEqual((uint) Ui.StatusCode.InternalServerError, resultResponse.StatusCode);
            Assert.AreEqual(Ui.ErrorMessages.InternalServerError, resultResponse.ErrorMessage);
        }
    }
}