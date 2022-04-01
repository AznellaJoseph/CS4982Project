using System;
using System.Collections.Generic;
using System.Data;
using CapstoneBackend.Model;
using MySql.Data.MySqlClient;

namespace CapstoneBackend.DAL
{
    /// <summary>
    ///     Data Access Layer (DAL) for accessing Transportation information from the database
    /// </summary>
    public class TransportationDal
    {
        private readonly MySqlConnection _connection;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TransportationDal" /> class.
        /// </summary>
        public TransportationDal() : this(new MySqlConnection(Connection.ConnectionString))
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="TransportationDal" /> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        public TransportationDal(MySqlConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        ///     Creates a transportation.
        /// </summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="method">The method of transportation.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="notes">The notes.</param>
        /// <returns>
        ///     The transportation id or throws an exception if there was an error
        /// </returns>
        public virtual int CreateTransportation(int tripId, string method, DateTime startDate, DateTime endDate,
            string? notes)
        {
            _connection.Open();
            const string procedure = "uspCreateTransportation";
            using MySqlCommand cmd = new(procedure, _connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@tripId", MySqlDbType.Int32).Value = tripId;
            cmd.Parameters.Add("@method", MySqlDbType.VarChar).Value = method;
            cmd.Parameters.Add("@startDate", MySqlDbType.DateTime).Value = startDate;
            cmd.Parameters.Add("@endDate", MySqlDbType.DateTime).Value = endDate;
            cmd.Parameters.Add("@notes", MySqlDbType.VarChar).Value = notes;

            try
            {
                var transportationId = Convert.ToInt32(cmd.ExecuteScalar());
                _connection.Close();
                return transportationId;
            }
            catch
            {
                _connection.Close();
                throw;
            }
        }

        /// <summary>
        ///     Gets the transportation on date.
        /// </summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="selectedDate">The selected date.</param>
        /// <returns> A list of the transportation of the trip on the specified date </returns>
        public virtual IList<Transportation> GetTransportationOnDate(int tripId, DateTime selectedDate)
        {
            _connection.Open();
            const string procedure = "uspGetTransportationOnDate";
            using MySqlCommand cmd = new(procedure, _connection);
            cmd.CommandType = CommandType.StoredProcedure;
            IList<Transportation> transportationOnDate = new List<Transportation>();

            cmd.Parameters.Add("@selectedDate", MySqlDbType.DateTime).Value = selectedDate;
            cmd.Parameters.Add("@tripId", MySqlDbType.Int32).Value = tripId;

            using var reader = cmd.ExecuteReaderAsync().Result;
            var transportationIdOrdinal = reader.GetOrdinal("transportationId");
            var startDateOrdinal = reader.GetOrdinal("startDate");
            var endDateOrdinal = reader.GetOrdinal("endDate");
            var methodOrdinal = reader.GetOrdinal("method");
            var notesOrdinal = reader.GetOrdinal("notes");

            while (reader.Read())
                transportationOnDate.Add(new Transportation
                {
                    TripId = tripId,
                    TransportationId = reader.GetInt32(transportationIdOrdinal),
                    Method = reader.GetString(methodOrdinal),
                    StartDate = reader.GetDateTime(startDateOrdinal),
                    EndDate = reader.GetDateTime(endDateOrdinal),
                    Notes = reader.IsDBNull(notesOrdinal) ? string.Empty : reader.GetString(notesOrdinal)
                });

            _connection.Close();
            return transportationOnDate;
        }

        /// <summary>
        ///     Removes the transportation.
        /// </summary>
        /// <param name="transportationId">The transportation identifier.</param>
        /// <returns>
        ///     True if the transportation was removed, false otherwise or throws an exception if there was an error
        /// </returns>
        public virtual bool RemoveTransportation(int transportationId)
        {
            _connection.Open();
            const string procedure = "uspRemoveTransportation";
            using MySqlCommand cmd = new(procedure, _connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@transportationId", MySqlDbType.Int32).Value = transportationId;

            try
            {
                var result = cmd.ExecuteNonQuery();
                _connection.Close();
                return result == 1;
            }
            catch
            {
                _connection.Close();
                throw;
            }
        }

        /// <summary>
        ///     Gets the transportation by its id.
        /// </summary>
        /// <param name="transportationId">The transportation identifier.</param>
        /// <returns>The transportation with the given id, null if no matching transportation found</returns>
        public virtual Transportation? GetTransportationById(int transportationId)
        {
            _connection.Open();
            const string procedure = "uspGetTransportationById";
            using MySqlCommand cmd = new(procedure, _connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@transportationId", MySqlDbType.Int32).Value = transportationId;

            using var reader = cmd.ExecuteReaderAsync().Result;
            var tripIdOrdinal = reader.GetOrdinal("tripId");
            var startDateOrdinal = reader.GetOrdinal("startDate");
            var endDateOrdinal = reader.GetOrdinal("endDate");
            var methodOrdinal = reader.GetOrdinal("method");
            var notesOrdinal = reader.GetOrdinal("notes");

            Transportation? transportation = null;

            if (reader.Read())
                transportation = new Transportation
                {
                    TripId = reader.GetInt32(tripIdOrdinal),
                    TransportationId = transportationId,
                    Method = reader.GetString(methodOrdinal),
                    StartDate = reader.GetDateTime(startDateOrdinal),
                    EndDate = reader.GetDateTime(endDateOrdinal),
                    Notes = reader.IsDBNull(notesOrdinal) ? string.Empty : reader.GetString(notesOrdinal)
                };

            _connection.Close();
            return transportation;
        }
    }
}