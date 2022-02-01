using System;
using CapstoneBackend.DAL;
using CapstoneBackend.Utils;

namespace CapstoneBackend.Model
{
    /// <summary>
    ///     A wrapper class for the UserDal. Manages the collection of Users and informs of server errors.
    /// </summary>
    public class UserManager : IDisposable
    {
        private readonly UserDal _dal;

        public UserManager() : this(new UserDal())
        {
        }

        public UserManager(UserDal dal)
        {
            _dal = dal;
        }

        public void Dispose()
        {
            _dal.Dispose();
        }


        ~UserManager()
        {
            _dal.Dispose();
        }


        /// <summary>
        ///     Gets a user by their credentials, i.e. username and password. If the user does not match any ones credentials then
        ///     an error is returned.
        /// </summary>
        /// <param name="username">the given username</param>
        /// <param name="password">the given password</param>
        /// <returns>the data of the found user or an error</returns>
        public Response<User> GetUserByCredentials(string username, string password)
        {
            var user = _dal.GetUserByUsername(username);
            if (user is null)
                return new Response<User>
                {
                    StatusCode = 404,
                    ErrorMessage = "Username is incorrect."
                };

            if (!PasswordHasher.ValidatePassword(password, user.Password))
                return new Response<User>
                {
                    StatusCode = 404,
                    ErrorMessage = "Password is incorrect."
                };

            return new Response<User>
            {
                Data = user
            };
        }

        /// <summary>Registers the user.</summary>
        /// <param name="username">The username input.</param>
        /// <param name="password">The password input.</param>
        /// <param name="fname">The fname input.</param>
        /// <param name="lname">The lname input.</param>
        /// <returns>Response status 200 for success; 400 for bad username; 500 otherwise.</returns>
        public Response<int> RegisterUser(string username, string password, string fname, string lname)
        {
            var user = _dal.GetUserByUsername(username);
            if (user is not null)
                return new Response<int>
                {
                    StatusCode = 400,
                    ErrorMessage = "Username is taken."
                };

            var userCreated = _dal.CreateUser(username, password, fname, lname);
            if (userCreated is not null)
                return new Response<int>
                {
                    StatusCode = 200,
                    Data = (int) userCreated
                };
            return new Response<int>
            {
                StatusCode = 500,
                ErrorMessage = "Internal Server Error."
            };
        }
    }
}