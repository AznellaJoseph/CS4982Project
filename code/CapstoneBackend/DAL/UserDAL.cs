using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using CapstoneBackend.Model;

namespace CapstoneBackend.DAL
{
    /// <summary>
    ///     Data Access Layer (DAL) for User
    /// </summary>
    public class UserDAL
    {
        /// <summary>
        ///     Gets the user with the specified username and password
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns> The user with the entered username and password </returns>
        public static async Task<User?> GetUser(string username, string password)
        {
            using SqlConnection connection = new(Connection.connectionString);


            connection.Open();
            var query = "uspGetUser";

            using SqlCommand cmd = new(query, connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = username;
            cmd.Parameters.Add("@password", SqlDbType.VarChar).Value = password;

            using var reader = await cmd.ExecuteReaderAsync();
            var idOrdinal = reader.GetOrdinal("id");
            var fnameOrdinal = reader.GetOrdinal("fname");
            var lnameOrdinal = reader.GetOrdinal("lname");

            if (reader.Read())
                return new User
                {
                    Id = reader.GetInt32(idOrdinal),
                    Username = username,
                    FirstName = reader.GetString(fnameOrdinal),
                    LastName = reader.GetString(lnameOrdinal)
                };

            return null;
        }
    }
}