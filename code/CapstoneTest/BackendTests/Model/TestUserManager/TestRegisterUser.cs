using CapstoneBackend.DAL;
using CapstoneBackend.Model;
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
    }
}