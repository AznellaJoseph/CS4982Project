using System;
using System.Collections.Generic;
using System.Data;
using CapstoneBackend.Model;
using MySql.Data.MySqlClient;

namespace CapstoneBackend.DAL
{
    /// <summary>
    /// The DAL for accessing trip information
    /// </summary>
    public class TripDal
    {
        private readonly MySqlConnection _connection;

        /// <summary>
        /// Creates a TripDal
        /// </summary>
        public TripDal() : this(new MySqlConnection(Connection.ConnectionString))
        {
        }

        /// <summary>
        /// Creates a TripDal using a specific connection.
        /// </summary>
        /// <param name="connection">a MySQLConnection</param>
        private TripDal(MySqlConnection connection)
        {
            _connection = connection;
        }
        
        /// <summary>
        /// Gets trips of the user with the given id.
        /// </summary>
        /// <param name="userId">the user's id</param>
        /// <returns>A list of trips of the user with the given id.</returns>
        public virtual IList<Trip> GetTripsByUserId(int userId)
        {
            this._connection.Open();
            const string query = "uspGetTripsByUserId";
            using MySqlCommand cmd = new(query, _connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@userId", MySqlDbType.Int32).Value = userId;
            using var reader = cmd.ExecuteReaderAsync().Result;
            var idOrdinal = reader.GetOrdinal("id");
            var nameOrdinal = reader.GetOrdinal("name");
            var startDateOrdinal = reader.GetOrdinal("startDate");
            var endDateOrdinal = reader.GetOrdinal("endDate");

            IList<Trip> trips = new List<Trip>();
            
            if (reader.Read())
            {
                trips.Add(new Trip
                {
                    Id = reader.GetInt32(idOrdinal),
                    UserId = userId,
                    Name = reader.GetString(nameOrdinal),
                    StartDate = reader.GetDateTime(startDateOrdinal),
                    EndDate = reader.GetDateTime(endDateOrdinal)
                });
            }
              
            this._connection.Close();
            return trips;
        }
        
        /// <summary>
        /// Creates a Trip in the database
        /// </summary>
        /// <param name="userId">the userId</param>
        /// <param name="name">the name of the trip</param>
        /// <param name="startDate">the start date of the trip</param>
        /// <param name="endDate">the end date of the trip</param>
        /// <returns>the trip id of the created trip or null if it failed</returns>
        public virtual int? CreateTrip(int userId, string name, DateTime startDate, DateTime endDate)
        {
            this._connection.Open();
            const string procedure = "uspCreateTrip";
            using MySqlCommand cmd = new(procedure, _connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@userId", MySqlDbType.Int32).Value = userId;
            cmd.Parameters.Add("@name", MySqlDbType.VarChar).Value = name;
            cmd.Parameters.Add("@startDate", MySqlDbType.Date).Value = startDate;
            cmd.Parameters.Add("@endDate", MySqlDbType.Date).Value = endDate;

            try
            {
                var tripId = cmd.ExecuteScalar();
                this._connection.Close();
                return Convert.ToInt32(tripId);
            }
            catch
            {
                this._connection.Close();
                return null;
            }
        }
    }
}