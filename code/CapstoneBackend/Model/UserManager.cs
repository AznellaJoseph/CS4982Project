using System;
using CapstoneBackend.DAL;
using CapstoneBackend.Utils;
using MySql.Data.MySqlClient;

namespace CapstoneBackend.Model
{
    /// <summary>
    ///     A wrapper class for the UserDal. Manages the collection of Users and informs of server errors.
    /// </summary>
    public class UserManager
    {
        private readonly UserDal _dal;

        /// <summary>
        ///     Initializes a new instance of the <see cref="UserManager" /> class.
        /// </summary>
        public UserManager() : this(new UserDal())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="UserManager" /> class.
        /// </summary>
        /// <param name="dal">The dal.</param>
        public UserManager(UserDal dal)
        {
            _dal = dal;
        }

        /// <summary>
        ///     Gets a user by their credentials, i.e. username and password. If the user does not match any ones credentials then
        ///     an error is returned.
        /// </summary>
        /// <param name="username">the given username</param>
        /// <param name="password">the given password</param>
        /// <returns>the data of the found user or an error</returns>
        public virtual Response<User> GetUserByCredentials(string username, string password)
        {
            User? user = null;
            try
            {
                user = _dal.GetUserByUsername(username);
            }
            catch (MySqlException e)
            {
                return new Response<User>
                {
                    StatusCode = e.Code,
                    ErrorMessage = e.Message
                };
            }

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
                StatusCode = 200,
                Data = user
            };
        }

        /// <summary>Registers the user.</summary>
        /// <param name="username">The username input.</param>
        /// <param name="password">The password input.</param>
        /// <param name="fname">The fname input.</param>
        /// <param name="lname">The lname input.</param>
        /// <returns>Response status 200 for success; 400 for bad username; 500 otherwise.</returns>
        public virtual Response<int> RegisterUser(string username, string password, string fname, string lname)
        {
            var user = _dal.GetUserByUsername(username);
            if (user is not null)
                return new Response<int>
                {
                    StatusCode = 400,
                    ErrorMessage = "Username is taken."
                };

            try
            {
                var userCreated = _dal.CreateUser(username, password, fname, lname);
                return new Response<int>
                {
                    StatusCode = 200,
                    Data = userCreated
                };
            }
            catch (MySqlException e)
            {
                return new Response<int>
                {
                    StatusCode = e.Code,
                    ErrorMessage = e.Message
                };
            }
            catch (Exception)
            {
                return new Response<int>
                {
                    StatusCode = 500,
                    ErrorMessage = "Internal Server Error."
                };
            }
        }
    }
}