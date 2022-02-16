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
        private readonly MySqlConnection _connection;

        public UserDal() : this(new MySqlConnection(Connection.ConnectionString))
        {
        }

        public UserDal(MySqlConnection connection)
        {
            _connection = connection;

        }

        /// <summary>
        ///     Gets the user with the specified username
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns> The user with the given username</returns>
        public virtual User? GetUserByUsername(string username)
        {
            this._connection.Open();
            const string query = "uspGetUserByUsername";
            using MySqlCommand cmd = new(query, _connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@username", MySqlDbType.VarChar).Value = username;

            using var reader = cmd.ExecuteReaderAsync().Result;
            var idOrdinal = reader.GetOrdinal("userId");
            var fnameOrdinal = reader.GetOrdinal("fname");
            var lnameOrdinal = reader.GetOrdinal("lname");
            var passwordOrdinal = reader.GetOrdinal("password");

            User? user = null;
            
            if (reader.Read())
            {
                user = new User
                  {
                      Id = reader.GetInt32(idOrdinal),
                      Username = username, 
                      FirstName = reader.GetString(fnameOrdinal),
                      LastName = reader.GetString(lnameOrdinal),
                      Password = reader.GetString(passwordOrdinal)
                  };
            }
              
            this._connection.Close();
            return user;
        }

        /// <summary>Inserts a User into the DB.</summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="fname">The first name.</param>
        /// <param name="lname">The last name.</param>
        /// <returns>ID of new user if successful. null, otherwise.</returns>
        public virtual int CreateUser(string username, string password, string fname, string lname)
        {
            this._connection.Open();
            const string procedure = "uspCreateUser";
            using MySqlCommand cmd = new(procedure, _connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@username", MySqlDbType.VarChar).Value = username;
            cmd.Parameters.Add("@password", MySqlDbType.VarChar).Value = PasswordHasher.Hash(password);
            cmd.Parameters.Add("@fname", MySqlDbType.VarChar).Value = fname;
            cmd.Parameters.Add("@lname", MySqlDbType.VarChar).Value = lname;

            var userId = cmd.ExecuteScalar();
            this._connection.Close();
            return Convert.ToInt32(userId);
        }
    }
}