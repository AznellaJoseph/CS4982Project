using CapstoneBackend.DAL;
using CapstoneBackend.Utils;

namespace CapstoneBackend.Model
{
    /// <summary>
    ///     A wrapper class for the UserDal. Manages the collection of Users and informs of server errors.
    /// </summary>
    public static class UserManager
    {
        /// <summary>
        ///     Gets a user by their credentials, i.e. username and password. If the user does not match any ones credentials then
        ///     an error is returned.
        /// </summary>
        /// <param name="username">the given username</param>
        /// <param name="password">the given password</param>
        /// <returns>the data of the found user or an error</returns>
        public static Response<User> GetUserByCredentials(string username, string password)
        {
            var user = UserDal.GetUserByUsername(username);
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

        public static Response<int> RegisterUser(string username, string password, string fname, string lname)
        {
            var user = UserDal.GetUserByUsername(username);
            if (user is not null)
            {
                return new Response<int>
                {
                    StatusCode = 400,
                    ErrorMessage = "Username is taken."
                };
            }

            var userCreated = UserDal.CreateUser(username, password, fname, lname);
            if (userCreated is not null)
            {
                return new Response<int>
                {
                    StatusCode = 200,
                    Data = (int) userCreated
                };
            }
            else
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