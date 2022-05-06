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
        ///     Gets a user by the given credentials
        /// </summary>
        /// <param name="username">the given username</param>
        /// <param name="password">the given password</param>
        /// <returns>A response of the user with the given credentials or a non-success status code and error message</returns>
        public virtual Response<User> GetUserByCredentials(string username, string password)
        {
            User? user;
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
            catch (Exception)
            {
                return new Response<User>
                {
                    StatusCode = (uint) Ui.StatusCode.InternalServerError,
                    ErrorMessage = Ui.ErrorMessages.InternalServerError
                };
            }

            if (user is null)
                return new Response<User>
                {
                    StatusCode = (uint) Ui.StatusCode.DataNotFound,
                    ErrorMessage = Ui.ErrorMessages.IncorrectUsername
                };

            if (!PasswordHasher.ValidatePassword(password, user.Password))
                return new Response<User>
                {
                    StatusCode = (uint) Ui.StatusCode.DataNotFound,
                    ErrorMessage = Ui.ErrorMessages.IncorrectPassword
                };

            return new Response<User>
            {
                Data = user
            };
        }

        /// <summary>Registers the user with the given credentials.</summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="fname">The first name.</param>
        /// <param name="lname">The last name.</param>
        /// <returns>A response of the id of the new user or a non-success status code and error message.</returns>
        public virtual Response<int> RegisterUser(string username, string password, string fname, string lname)
        {
            var user = _dal.GetUserByUsername(username);
            if (user is not null)
                return new Response<int>
                {
                    StatusCode = (uint) Ui.StatusCode.BadRequest,
                    ErrorMessage = Ui.ErrorMessages.UsernameTaken
                };

            try
            {
                var userCreated = _dal.CreateUser(username, password, fname, lname);
                return new Response<int>
                {
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
                    StatusCode = (uint) Ui.StatusCode.InternalServerError,
                    ErrorMessage = Ui.ErrorMessages.InternalServerError
                };
            }
        }

        /// <summary>
        ///     Gets a user by the given credentials
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>
        ///     A response of the user with the given credentials or a non-success status code and error message
        /// </returns>
        public virtual Response<User> GetUserById(int userId)
        {
            User? user;
            try
            {
                user = _dal.GetUserById(userId);
            }
            catch (MySqlException e)
            {
                return new Response<User>
                {
                    StatusCode = e.Code,
                    ErrorMessage = e.Message
                };
            }
            catch (Exception)
            {
                return new Response<User>
                {
                    StatusCode = (uint) Ui.StatusCode.InternalServerError,
                    ErrorMessage = Ui.ErrorMessages.InternalServerError
                };
            }

            if (user is null)
                return new Response<User>
                {
                    StatusCode = (uint) Ui.StatusCode.DataNotFound,
                    ErrorMessage = Ui.ErrorMessages.IncorrectUsername
                };

            return new Response<User>
            {
                Data = user
            };
        }
    }
}