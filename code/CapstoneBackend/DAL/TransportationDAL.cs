using System;
using System.Collections.Generic;
using System.Data;
using CapstoneBackend.Model;
using MySql.Data.MySqlClient;

namespace CapstoneBackend.DAL
{
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
        /// <returns>
        ///     The transportation id
        /// </returns>
        public virtual int CreateTransportation(int tripId, string method, DateTime startDate, DateTime? endDate)
        {
            _connection.Open();
            const string procedure = "uspCreateTransportation";
            using MySqlCommand cmd = new(procedure, _connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@tripId", MySqlDbType.Int32).Value = tripId;
            cmd.Parameters.Add("@method", MySqlDbType.VarChar).Value = method;
            cmd.Parameters.Add("@startDate", MySqlDbType.DateTime).Value = startDate;
            cmd.Parameters.Add("@endDate", MySqlDbType.DateTime).Value = endDate;

            var transportationId = Convert.ToInt32(cmd.ExecuteScalar());

            _connection.Close();
            return transportationId;
        }
        
        /// <summary>
        ///     Gets the transportations on date.
        /// </summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="selectedDate">The selected date.</param>
        /// <returns> A list of the transportations of the trip on the specified date </returns>
        public virtual IList<Transportation> GetTransportationOnDate(int tripId, DateTime selectedDate)
        {
            _connection.Open();
            const string procedure = "uspGetTransportationOnDate";
            using MySqlCommand cmd = new(procedure, _connection);
            cmd.CommandType = CommandType.StoredProcedure;
            IList<Transportation> transportationsOnDate = new List<Transportation>();

            cmd.Parameters.Add("@selectedDate", MySqlDbType.DateTime).Value = selectedDate;
            cmd.Parameters.Add("@tripId", MySqlDbType.Int32).Value = tripId;

            using var reader = cmd.ExecuteReaderAsync().Result;
            var transportationIdOrdinal = reader.GetOrdinal("transportationId");
            var startDateOrdinal = reader.GetOrdinal("startDate");
            var endDateOrdinal = reader.GetOrdinal("endDate");
            var methodOrdinal = reader.GetOrdinal("method");

            while (reader.Read())
                transportationsOnDate.Add(new Transportation
                {
                    TripId = tripId,
                    TransportationId = reader.GetInt32(transportationIdOrdinal),
                    Method = reader.GetString(methodOrdinal),
                    StartDate = reader.GetDateTime(startDateOrdinal),
                    EndDate = reader.GetDateTime(endDateOrdinal)
                });

            _connection.Close();
            return transportationsOnDate;
        }

        /// <summary>
        /// Removes the transportation.
        /// </summary>
        /// <param name="transportationId">The transportation identifier.</param>
        /// <returns>
        /// True if the transport was removed, false otherwise
        /// </returns>
        public virtual bool RemoveTransportation(int transportationId)
        {
            _connection.Open();
            const string procedure = "uspRemoveTransportation";
            using MySqlCommand cmd = new(procedure, _connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@transportationId", MySqlDbType.Int32).Value = transportationId;

            return cmd.ExecuteNonQuery() == 1;
        }
    }
}