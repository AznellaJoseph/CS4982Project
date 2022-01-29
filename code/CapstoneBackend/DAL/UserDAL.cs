using System;
using System.Data;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using MySql.Data.MySqlClient;

namespace CapstoneBackend.DAL
{
    /// <summary>
    ///     Data Access Layer (DAL) for User
    /// </summary>
    public class UserDal
    {
        /// <summary>
        ///     Gets the user with the specified username
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns> The user with the given username</returns>
        public static User? GetUserByUsername(string username)
        {
            using MySqlConnection connection = new(Connection.ConnectionString);
            connection.Open();

            const string query = "uspGetUserByUsername";
            using MySqlCommand cmd = new(query, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@username", MySqlDbType.VarChar).Value = username;

            using var reader = cmd.ExecuteReaderAsync().Result;
            var idOrdinal = reader.GetOrdinal("id");
            var fnameOrdinal = reader.GetOrdinal("fname");
            var lnameOrdinal = reader.GetOrdinal("lname");
            var passwordOrdinal = reader.GetOrdinal("password");

            if (reader.Read())
                return new User
                {
                    Id = reader.GetInt32(idOrdinal),
                    Username = username,
                    FirstName = reader.GetString(fnameOrdinal),
                    LastName = reader.GetString(lnameOrdinal),
                    Password = reader.GetString(passwordOrdinal)
                };

            return null;
        }

        /// <summary>Inserts a User into the DB.</summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="fname">The first name.</param>
        /// <param name="lname">The last name.</param>
        /// <returns>True, if a new user was inserted to the DB. False, otherwise.</returns>
        public static bool CreateUser(string username, string password, string fname, string lname)
        {
            using MySqlConnection connection = new(Connection.ConnectionString);

            connection.Open();

            const string procedure = "uspCreateUser";
            using MySqlCommand cmd = new(procedure, connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@username", MySqlDbType.VarChar).Value = username;
            cmd.Parameters.Add("@password", MySqlDbType.VarChar).Value = PasswordHasher.Hash(password);
            cmd.Parameters.Add("@fname", MySqlDbType.VarChar).Value = fname;
            cmd.Parameters.Add("@lname", MySqlDbType.VarChar).Value = lname;

            return Convert.ToBoolean(cmd.ExecuteNonQueryAsync());
        }
    }
}