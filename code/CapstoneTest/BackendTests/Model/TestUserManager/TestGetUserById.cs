using System;
using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CapstoneTest.BackendTests.Model.TestUserManager
{
    [TestClass]
    public class TestGetUserById
    {
        [TestMethod]
        public void GetUserById_Success()
        {
            User fakeExistingUser = new() {UserId = 1};
            var mockUserDal = new Mock<UserDal>();
            mockUserDal.Setup(db => db.GetUserById(1)).Returns(fakeExistingUser);

            UserManager userManager = new(mockUserDal.Object);

            var resultResponse = userManager.GetUserById(1);

            Assert.AreEqual((uint) Ui.StatusCode.Success, resultResponse.StatusCode);
            Assert.IsInstanceOfType(resultResponse.Data, typeof(User));
            Assert.AreEqual(resultResponse.Data?.FirstName, fakeExistingUser.FirstName);
            Assert.AreEqual(resultResponse.Data?.LastName, fakeExistingUser.LastName);
            Assert.AreEqual(resultResponse.Data?.Username, fakeExistingUser.Username);
            Assert.AreEqual(resultResponse.Data?.Password, fakeExistingUser.Password);
            Assert.AreEqual(resultResponse.Data?.UserId, fakeExistingUser.UserId);
        }

        [TestMethod]
        public void GetUserById_WithUnknownId_ReturnsErrorMessage()
        {
            User? missingUser = null;
            var mockUserDal = new Mock<UserDal>();
            mockUserDal.Setup(db => db.GetUserById(1)).Returns(missingUser);

            UserManager userManager = new(mockUserDal.Object);

            var resultResponse = userManager.GetUserById(-1);

            Assert.AreEqual((uint) Ui.StatusCode.DataNotFound, resultResponse.StatusCode);
        }

        [TestMethod]
        public void Call_ServerMySqlException_ReturnsErrorMessage()
        {
            var mockUserDal = new Mock<UserDal>();
            var builder = new MySqlExceptionBuilder();
            mockUserDal.Setup(db => db.GetUserById(1))
                .Throws(builder
                    .WithError((uint) Ui.StatusCode.InternalServerError, Ui.ErrorMessages.InternalServerError).Build());
            UserManager userManager = new(mockUserDal.Object);

            var resultResponse = userManager.GetUserById(1);

            Assert.AreEqual((uint) Ui.StatusCode.InternalServerError, resultResponse.StatusCode);
            Assert.AreEqual(Ui.ErrorMessages.InternalServerError, resultResponse.ErrorMessage);
        }

        [TestMethod]
        public void GetUserById_ServerException_ReturnsErrorMessage()
        {
            var mockUserDal = new Mock<UserDal>();
            mockUserDal.Setup(db => db.GetUserById(1))
                .Throws(new Exception());
            UserManager userManager = new(mockUserDal.Object);

            var resultResponse = userManager.GetUserById(1);

            Assert.AreEqual((uint) Ui.StatusCode.InternalServerError, resultResponse.StatusCode);
            Assert.AreEqual(Ui.ErrorMessages.InternalServerError, resultResponse.ErrorMessage);
        }
    }
}