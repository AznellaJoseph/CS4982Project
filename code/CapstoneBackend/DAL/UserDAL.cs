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

        /// <summary>Inserts a User into the DB.</summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="fname">The first name.</param>
        /// <param name="lname">The last name.</param>
        /// <returns>True, if a new user was inserted to the DB. False, otherwise.</returns>
        public static async Task<bool> CreateUser(string username, string password, string fname, string lname)
        {
            using SqlConnection connection = new(Connection.connectionString);

            connection.Open();

            var procedure = "uspInsertUser";
            using SqlCommand cmd = new(procedure, connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = username;
            //TODO : Store password hash.
            cmd.Parameters.Add("@password", SqlDbType.VarChar).Value = password;
            cmd.Parameters.Add("@fname", SqlDbType.VarChar).Value = fname;
            cmd.Parameters.Add("@lname", SqlDbType.VarChar).Value = lname;

            return System.Convert.ToBoolean(await cmd.ExecuteNonQueryAsync());
        }
    }
}