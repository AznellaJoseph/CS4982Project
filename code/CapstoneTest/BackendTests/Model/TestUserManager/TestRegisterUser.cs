using System;
using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CapstoneTest.BackendTests.Model.TestUserManager
{
    [TestClass]
    public class TestRegisterUser
    {
        [TestMethod]
        public void Register_WithValidInput_Succeeds()
        {
            const string username = "TestUsername";
            const string password = "TestPassword";
            const string fname = "FirstName";
            const string lname = "LastName";

            User? fakeExistingUser = null;
            User fakeCreatedUser = new() { UserId = 1 };
            var mockUserDal = new Mock<UserDal>();
            mockUserDal.Setup(db => db.GetUserByUsername(username)).Returns(fakeExistingUser);
            mockUserDal.Setup(db => db.CreateUser(username, password, fname, lname)).Returns(fakeCreatedUser.UserId);

            UserManager userManager = new(mockUserDal.Object);

            var resultResponse = userManager.RegisterUser(username, password, fname, lname);

            Assert.AreEqual((uint)Ui.StatusCode.Success, resultResponse.StatusCode);
            Assert.AreEqual(1, resultResponse.Data);
        }

        [TestMethod]
        public void Register_WithDuplicateUsername_Fails()
        {
            const string username = "TestUsername";
            const string password = "TestPassword";
            const string fname = "FirstName";
            const string lname = "LastName";

            User fakeExistingUser = new() { UserId = 1 };
            var mockUserDal = new Mock<UserDal>();
            mockUserDal.Setup(db => db.GetUserByUsername(username)).Returns(fakeExistingUser);
            mockUserDal.Setup(db => db.CreateUser(username, password, fname, lname)).Returns(null);

            UserManager userManager = new(mockUserDal.Object);

            var resultResponse = userManager.RegisterUser(username, password, fname, lname);

            Assert.AreEqual((uint)Ui.StatusCode.BadRequest, resultResponse.StatusCode);
        }

        [TestMethod]
        public void Register_InternalServerErrorConfiguration_Fails()
        {
            const string username = "TestUsername";
            const string password = "TestPassword";
            const string fname = "FirstName";
            const string lname = "LastName";

            User? fakeExistingUser = null;
            var mockUserDal = new Mock<UserDal>();
            var builder = new MySqlExceptionBuilder();
            mockUserDal.Setup(db => db.GetUserByUsername(username)).Returns(fakeExistingUser);
            mockUserDal.Setup(db => db.CreateUser(username, password, fname, lname))
                .Throws(builder.WithError((uint)Ui.StatusCode.InternalServerError, Ui.ErrorMessages.InternalServerError).Build());

            UserManager userManager = new(mockUserDal.Object);

            var resultResponse = userManager.RegisterUser(username, password, fname, lname);

            Assert.AreEqual((uint)Ui.StatusCode.InternalServerError, resultResponse.StatusCode);
        }

        [TestMethod]
        public void Register_ThrowsException_Fails()
        {
            const string username = "TestUsername";
            const string password = "TestPassword";
            const string fname = "FirstName";
            const string lname = "LastName";

            User? fakeExistingUser = null;
            var mockUserDal = new Mock<UserDal>();
            mockUserDal.Setup(db => db.GetUserByUsername(username)).Returns(fakeExistingUser);
            mockUserDal.Setup(db => db.CreateUser(username, password, fname, lname))
                .Throws(new Exception(Ui.ErrorMessages.InternalServerError));

            UserManager userManager = new(mockUserDal.Object);

            var resultResponse = userManager.RegisterUser(username, password, fname, lname);

            Assert.AreEqual((uint)Ui.StatusCode.InternalServerError, resultResponse.StatusCode);
            Assert.AreEqual(Ui.ErrorMessages.InternalServerError, resultResponse.ErrorMessage);
        }
    }
}