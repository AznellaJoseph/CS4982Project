using CapstoneBackend.Model;
using MySql.Data.MySqlClient;

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
        public User? GetUser(string username, string password)
        {
            using (MySqlConnection connection = new(Connection.connectionString))
            {
                connection.Open();
                var query = "CALL uspGetUser(@username, @password)";
                using MySqlCommand cmd = new(query, connection);

                cmd.Parameters.Add("@username", MySqlDbType.VarChar).Value = username;
                cmd.Parameters.Add("@password", MySqlDbType.VarChar).Value = password;

                using var reader = cmd.ExecuteReader();
                var idOrdinal = reader.GetOrdinal("id");
                var usernameOrdinal = reader.GetOrdinal("username");
                var fnameOrdinal = reader.GetOrdinal("fname");
                var lnameOrdinal = reader.GetOrdinal("lname");

                if (reader.Read())
                    return new User
                    {
                        Id = reader.GetInt32(idOrdinal),
                        Username = reader.GetString(usernameOrdinal),
                        FirstName = reader.GetString(fnameOrdinal),
                        LastName = reader.GetString(lnameOrdinal)
                    };
            }

            return null;
        }
    }
}