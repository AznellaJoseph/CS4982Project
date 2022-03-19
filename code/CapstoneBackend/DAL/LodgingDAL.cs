using System;
using System.Collections.Generic;
using System.Data;
using CapstoneBackend.Model;
using MySql.Data.MySqlClient;

namespace CapstoneBackend.DAL
{
    /// <summary>
    ///     Data Access Layer (DAL) for accessing Lodging information from the database
    /// </summary>
    public class LodgingDal
    {
        private readonly MySqlConnection _connection;

        /// <summary>
        ///     Initializes a new instance of the <see cref="LodgingDal" /> class.
        /// </summary>
        public LodgingDal() : this(new MySqlConnection(Connection.ConnectionString))
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="LodgingDal" /> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        public LodgingDal(MySqlConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        ///     Creates a Lodging.
        /// </summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="location">The location.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="notes">The notes.</param>
        /// <returns>
        ///     The Lodging id
        /// </returns>
        public virtual int CreateLodging(int tripId, string location, DateTime startDate, DateTime endDate,
            string? notes)
        {
            _connection.Open();
            const string procedure = "uspCreateLodging";
            using MySqlCommand cmd = new(procedure, _connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@tripId", MySqlDbType.Int32).Value = tripId;
            cmd.Parameters.Add("@location", MySqlDbType.VarChar).Value = location;
            cmd.Parameters.Add("@startDate", MySqlDbType.DateTime).Value = startDate;
            cmd.Parameters.Add("@endDate", MySqlDbType.DateTime).Value = endDate;
            cmd.Parameters.Add("@notes", MySqlDbType.VarChar).Value = notes;

            var lodgingId = Convert.ToInt32(cmd.ExecuteScalar());

            _connection.Close();
            return lodgingId;
        }

        /// <summary>
        ///     Gets the Lodgings by trip identifier.
        /// </summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <returns> a list of the Lodgings from the trip specified by the trip id </returns>
        public virtual IList<Lodging> GetLodgingsByTripId(int tripId)
        {
            _connection.Open();
            const string procedure = "uspGetLodgingsByTripId";
            using MySqlCommand cmd = new(procedure, _connection);
            cmd.CommandType = CommandType.StoredProcedure;
            IList<Lodging> lodgingsInTrip = new List<Lodging>();

            cmd.Parameters.Add("@tripId", MySqlDbType.UInt32).Value = tripId;


            using var reader = cmd.ExecuteReader();

            var lodgingIdOrdinal = reader.GetOrdinal("lodgingId");
            var startDateOrdinal = reader.GetOrdinal("startDate");
            var endDateOrdinal = reader.GetOrdinal("endDate");
            var locationOrdinal = reader.GetOrdinal("location");
            var notesOrdinal = reader.GetOrdinal("notes");

            while (reader.Read())
                lodgingsInTrip.Add(new Lodging
                {
                    TripId = tripId,
                    LodgingId = reader.GetInt32(lodgingIdOrdinal),
                    Location = reader.GetString(locationOrdinal),
                    StartDate = reader.GetDateTime(startDateOrdinal),
                    EndDate = reader.GetDateTime(endDateOrdinal),
                    Notes = reader.IsDBNull(notesOrdinal) ? "" : reader.GetString(notesOrdinal)
                });

            _connection.Close();
            return lodgingsInTrip;
        }

        /// <summary>
        ///     Gets the lodgings on the specified date.
        /// </summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="selectedDate">The selected date.</param>
        /// <returns> A list of the lodgings of the trip on the specified date </returns>
        public virtual IList<Lodging> GetLodgingsOnDate(int tripId, DateTime selectedDate)
        {
            _connection.Open();
            const string procedure = "uspGetLodgingsOnDate";
            using MySqlCommand cmd = new(procedure, _connection);
            cmd.CommandType = CommandType.StoredProcedure;
            IList<Lodging> lodgingsOnDate = new List<Lodging>();

            cmd.Parameters.Add("@selectedDate", MySqlDbType.DateTime).Value = selectedDate;
            cmd.Parameters.Add("@tripId", MySqlDbType.Int32).Value = tripId;

            using var reader = cmd.ExecuteReaderAsync().Result;
            var lodgingIdOrdinal = reader.GetOrdinal("lodgingId");
            var startDateOrdinal = reader.GetOrdinal("startDate");
            var endDateOrdinal = reader.GetOrdinal("endDate");
            var locationOrdinal = reader.GetOrdinal("location");
            var notesOrdinal = reader.GetOrdinal("notes");

            while (reader.Read())
                lodgingsOnDate.Add(new Lodging
                {
                    TripId = tripId,
                    LodgingId = reader.GetInt32(lodgingIdOrdinal),
                    Location = reader.GetString(locationOrdinal),
                    StartDate = reader.GetDateTime(startDateOrdinal),
                    EndDate = reader.GetDateTime(endDateOrdinal),
                    Notes = reader.IsDBNull(notesOrdinal) ? string.Empty : reader.GetString(notesOrdinal)
                });

            _connection.Close();
            return lodgingsOnDate;
        }

        /// <summary>
        ///     Removes the Lodging.
        /// </summary>
        /// <param name="lodgingId">The Lodging identifier.</param>
        /// <returns>
        ///     True if the Lodging was removed, false otherwise
        /// </returns>
        public virtual bool RemoveLodging(int lodgingId)
        {
            _connection.Open();
            const string procedure = "uspRemoveLodging";
            using MySqlCommand cmd = new(procedure, _connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@lodgingId", MySqlDbType.UInt32).Value = lodgingId;

            return cmd.ExecuteNonQuery() == 1;
        }
    }
}