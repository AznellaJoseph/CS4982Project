using System;
using System.Collections.Generic;
using System.Data;
using CapstoneBackend.Model;
using MySql.Data.MySqlClient;

namespace CapstoneBackend.DAL
{
    /// <summary>
    ///     Data Access Layer (DAL) for accessing Trip information from the database
    /// </summary>
    public class TripDal
    {
        private readonly MySqlConnection _connection;

        /// <summary>
        ///     Creates a TripDal
        /// </summary>
        public TripDal() : this(new MySqlConnection(Connection.ConnectionString))
        {
        }

        /// <summary>
        ///     Creates a TripDal using a specific connection.
        /// </summary>
        /// <param name="connection">a MySQLConnection</param>
        public TripDal(MySqlConnection connection)
        {
            _connection = connection;
        }

        /// <summary>Gets the trip by trip identifier.</summary>
        /// <param name="tripId">The trip id.</param>
        /// <returns>A Trip object if a matching trip exists in the database, null otherwise.</returns>
        public virtual Trip? GetTripByTripId(int tripId)
        {
            _connection.Open();
            const string query = "uspGetTripByTripId";
            using MySqlCommand cmd = new(query, _connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@tripId", MySqlDbType.Int32).Value = tripId;

            using var reader = cmd.ExecuteReaderAsync().Result;
            var userIdOrdinal = reader.GetOrdinal("userId");
            var nameOrdinal = reader.GetOrdinal("name");
            var notesOrdinal = reader.GetOrdinal("notes");
            var startDateOrdinal = reader.GetOrdinal("startDate");
            var endDateOrdinal = reader.GetOrdinal("endDate");

            Trip? trip = null;

            if (reader.Read())
                trip = new Trip
                {
                    TripId = tripId,
                    UserId = reader.GetInt32(userIdOrdinal),
                    Name = reader.GetString(nameOrdinal),
                    Notes = reader.GetString(notesOrdinal),
                    StartDate = reader.GetDateTime(startDateOrdinal),
                    EndDate = reader.GetDateTime(endDateOrdinal)
                };

            _connection.Close();
            return trip;
        }


        /// <summary>
        ///     Gets trips of the user with the given id.
        /// </summary>
        /// <param name="userId">the user's id</param>
        /// <returns>A list of trips of the user with the given id.</returns>
        public virtual IList<Trip> GetTripsByUserId(int userId)
        {
            _connection.Open();
            const string query = "uspGetTripsByUserId";
            using MySqlCommand cmd = new(query, _connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@userId", MySqlDbType.Int32).Value = userId;
            using var reader = cmd.ExecuteReaderAsync().Result;
            var idOrdinal = reader.GetOrdinal("tripId");
            var nameOrdinal = reader.GetOrdinal("name");
            var notesOrdinal = reader.GetOrdinal("notes");
            var startDateOrdinal = reader.GetOrdinal("startDate");
            var endDateOrdinal = reader.GetOrdinal("endDate");

            IList<Trip> trips = new List<Trip>();

            while (reader.Read())
                trips.Add(new Trip
                {
                    TripId = reader.GetInt32(idOrdinal),
                    UserId = userId,
                    Name = reader.GetString(nameOrdinal),
                    Notes = reader.GetString(notesOrdinal),
                    StartDate = reader.GetDateTime(startDateOrdinal),
                    EndDate = reader.GetDateTime(endDateOrdinal)
                });

            _connection.Close();
            return trips;
        }

        /// <summary>
        ///     Creates a Trip in the database
        /// </summary>
        /// <param name="userId">the userId</param>
        /// <param name="name">the name of the trip</param>
        /// <param name="notes">the notes of the trip</param>
        /// <param name="startDate">the start date of the trip</param>
        /// <param name="endDate">the end date of the trip</param>
        /// <returns>the trip id of the created trip or null if it failed</returns>
        public virtual int CreateTrip(int userId, string name, string? notes, DateTime startDate, DateTime endDate)
        {
            _connection.Open();
            const string procedure = "uspCreateTrip";
            using MySqlCommand cmd = new(procedure, _connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@userId", MySqlDbType.Int32).Value = userId;
            cmd.Parameters.Add("@notes", MySqlDbType.VarChar).Value = notes ?? string.Empty;
            cmd.Parameters.Add("@name", MySqlDbType.VarChar).Value = name;
            cmd.Parameters.Add("@startDate", MySqlDbType.Date).Value = startDate;
            cmd.Parameters.Add("@endDate", MySqlDbType.Date).Value = endDate;

            try
            {
                var tripId = cmd.ExecuteScalar();
                _connection.Close();
                return Convert.ToInt32(tripId);
            }
            catch
            {
                _connection.Close();
                throw;
            }
        }
    }
}