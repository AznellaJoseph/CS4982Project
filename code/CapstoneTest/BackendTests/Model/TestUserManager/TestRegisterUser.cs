using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace CapstoneTest.BackendTests.Model.TestUserManager
{

    [TestClass]
    public class TestRegisterUser
    {
        [TestMethod]
        public void Register_WithValidInput_Succeeds()
        {
            string username = "TestUsername";
            string password = "TestPassword";
            string fname = "FirstName";
            string lname = "LastName";

            User? fakeExistingUser = null;
            User fakeCreatedUser = new() { Id = 1};
            var mockUserDAL = new Mock<UserDal>();
            mockUserDAL.Setup(db => db.GetUserByUsername(username)).Returns(fakeExistingUser);
            mockUserDAL.Setup(db => db.CreateUser(username, password, fname, lname)).Returns(fakeCreatedUser.Id);

            UserManager userManager = new(mockUserDAL.Object);

            var resultResponse = userManager.RegisterUser(username, password, fname, lname);

            Assert.AreEqual(200, resultResponse.StatusCode);
            Assert.AreEqual(1, resultResponse.Data);
        }

        [TestMethod]
        public void Register_WithDuplicateUsername_Fails()
        {
            string username = "TestUsername";
            string password = "TestPassword";
            string fname = "FirstName";
            string lname = "LastName";

            int? dbResult = null;
            User fakeExistingUser = new() { Id = 1 };
            var mockUserDAL = new Mock<UserDal>();
            mockUserDAL.Setup(db => db.GetUserByUsername(username)).Returns(fakeExistingUser);
            mockUserDAL.Setup(db => db.CreateUser(username, password, fname, lname)).Returns(dbResult);

            UserManager userManager = new(mockUserDAL.Object);

            var resultResponse = userManager.RegisterUser(username, password, fname, lname);

            Assert.AreEqual(400, resultResponse.StatusCode);
        }
    }
}