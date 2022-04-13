using System;
using System.Collections.Generic;
using System.Data;
using CapstoneBackend.Model;
using MySql.Data.MySqlClient;

namespace CapstoneBackend.DAL
{
    /// <summary>
    ///     Data Access Layer (DAL) for accessing Waypoint information from the database
    /// </summary>
    public class WaypointDal
    {
        private readonly MySqlConnection _connection;

        /// <summary>
        ///     Initializes a new instance of the <see cref="WaypointDal" /> class.
        /// </summary>
        public WaypointDal() : this(new MySqlConnection(Connection.ConnectionString))
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WaypointDal" /> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        public WaypointDal(MySqlConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        ///     Creates a waypoint.
        /// </summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="location">The location.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="notes">The notes.</param>
        /// <returns>
        ///     The waypoint id or throws an exception if there was an error
        /// </returns>
        public virtual int CreateWaypoint(int tripId, string location, DateTime startDate, DateTime endDate,
            string? notes)
        {
            _connection.Open();
            const string procedure = "uspCreateWaypoint";
            using MySqlCommand cmd = new(procedure, _connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@tripId", MySqlDbType.Int32).Value = tripId;
            cmd.Parameters.Add("@location", MySqlDbType.VarChar).Value = location;
            cmd.Parameters.Add("@startDate", MySqlDbType.DateTime).Value = startDate;
            cmd.Parameters.Add("@endDate", MySqlDbType.DateTime).Value = endDate;
            cmd.Parameters.Add("@notes", MySqlDbType.VarChar).Value = notes;

            try
            {
                var waypointId = Convert.ToInt32(cmd.ExecuteScalar());
                _connection.Close();
                return waypointId;
            }
            catch
            {
                _connection.Close();
                throw;
            }
        }

        /// <summary>
        ///     Gets the waypyoints on the specified date.
        /// </summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="selectedDate">The selected date.</param>
        /// <returns> A list of the waypoints of the trip on the specified date </returns>
        public virtual IList<Waypoint> GetWaypointsOnDate(int tripId, DateTime selectedDate)
        {
            _connection.Open();
            const string procedure = "uspGetWaypointsOnDate";
            using MySqlCommand cmd = new(procedure, _connection);
            cmd.CommandType = CommandType.StoredProcedure;
            IList<Waypoint> waypointsOnDate = new List<Waypoint>();

            cmd.Parameters.Add("@selectedDate", MySqlDbType.DateTime).Value = selectedDate;
            cmd.Parameters.Add("@tripId", MySqlDbType.Int32).Value = tripId;

            using var reader = cmd.ExecuteReaderAsync().Result;
            var waypointIdOrdinal = reader.GetOrdinal("waypointId");
            var startDateOrdinal = reader.GetOrdinal("startDate");
            var endDateOrdinal = reader.GetOrdinal("endDate");
            var locationOrdinal = reader.GetOrdinal("location");
            var notesOrdinal = reader.GetOrdinal("notes");

            while (reader.Read())
                waypointsOnDate.Add(new Waypoint
                {
                    TripId = tripId,
                    WaypointId = reader.GetInt32(waypointIdOrdinal),
                    Location = reader.GetString(locationOrdinal),
                    StartDate = reader.GetDateTime(startDateOrdinal),
                    EndDate = reader.GetDateTime(endDateOrdinal),
                    Notes = reader.IsDBNull(notesOrdinal) ? string.Empty : reader.GetString(notesOrdinal)
                });

            _connection.Close();
            return waypointsOnDate;
        }

        /// <summary>
        ///     Removes the waypoint.
        /// </summary>
        /// <param name="waypointId">The waypoint identifier.</param>
        /// <returns>
        ///     True if the waypoint was removed, false otherwise or throws an exception if there was an error
        /// </returns>
        public virtual bool RemoveWaypoint(int waypointId)
        {
            _connection.Open();
            const string procedure = "uspRemoveWaypoint";
            using MySqlCommand cmd = new(procedure, _connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@waypointId", MySqlDbType.Int32).Value = waypointId;

            var result = cmd.ExecuteNonQuery();
            _connection.Close();
            return result == 1;
        }

        /// <summary>
        ///     Gets the waypoint by identifier.
        /// </summary>
        /// <param name="waypointId">The waypoint identifier.</param>
        /// <returns>The waypoint with the given id, null if no matching waypoint found.</returns>
        public virtual Waypoint? GetWaypointById(int waypointId)
        {
            _connection.Open();
            const string procedure = "uspGetWaypointById";
            using MySqlCommand cmd = new(procedure, _connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@waypointId", MySqlDbType.Int32).Value = waypointId;

            using var reader = cmd.ExecuteReaderAsync().Result;
            var tripIdOrdinal = reader.GetOrdinal("tripId");
            var startDateOrdinal = reader.GetOrdinal("startDate");
            var endDateOrdinal = reader.GetOrdinal("endDate");
            var locationOrdinal = reader.GetOrdinal("location");
            var notesOrdinal = reader.GetOrdinal("notes");

            Waypoint? waypoint = null;

            if (reader.Read())
                waypoint = new Waypoint
                {
                    TripId = reader.GetInt32(tripIdOrdinal),
                    WaypointId = waypointId,
                    Location = reader.GetString(locationOrdinal),
                    StartDate = reader.GetDateTime(startDateOrdinal),
                    EndDate = reader.GetDateTime(endDateOrdinal),
                    Notes = reader.IsDBNull(notesOrdinal) ? string.Empty : reader.GetString(notesOrdinal)
                };

            _connection.Close();
            return waypoint;
        }
    }
}