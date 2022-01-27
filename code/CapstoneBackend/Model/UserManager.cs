using System.Threading.Tasks;
using CapstoneBackend.DAL;
using CapstoneBackend.Utils;

namespace CapstoneBackend.Model
{
    public static class UserManager
    {
        public static async Task<Response<User>> LoginUser(string username, string password)
        {
            var hashedPassword = PasswordHasher.Hash(password);
            User? user = await UserDAL.GetUser(username, hashedPassword);
            if (user is null)
            {
                return new Response<User>
                {
                    StatusCode = 404,
                    ErrorMessage = "Incorrect Password or Username."
                };
            }

            return new Response<User>
            {
                Data = user
            };
        }
    }
}