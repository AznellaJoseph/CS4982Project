using System;
using System.Collections.Generic;
using System.Data;
using CapstoneBackend.Model;
using MySql.Data.MySqlClient;

namespace CapstoneBackend.DAL
{
    /// <summary>
    ///     Data Access Layer (DAL) for Waypoint
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
        /// Creates a waypoint.
        /// </summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="location">The location.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="notes">The notes.</param>
        /// <returns>
        /// The waypoint id
        /// </returns>
        public virtual int CreateWaypoint(int tripId, string location, DateTime startDate, DateTime endDate, string? notes)
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

            var waypointId = Convert.ToInt32(cmd.ExecuteScalar());

            this._connection.Close();
            return waypointId;
        }


        /// <summary>
        /// Gets the waypoints by trip identifier.
        /// </summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <returns> a list of the waypoints from the trip specified by the trip id </returns>
        public virtual IList<Waypoint> GetWaypointsByTripId(int tripId)
        {
            _connection.Open();
            const string procedure = "uspGetWaypointsByTripId";
            using MySqlCommand cmd = new(procedure, _connection);
            cmd.CommandType = CommandType.StoredProcedure;
            IList<Waypoint> waypointsInTrip = new List<Waypoint>();

            cmd.Parameters.Add("@tripId", MySqlDbType.UInt32).Value = tripId;


            using var reader = cmd.ExecuteReader();

            var waypointIdOrdinal = reader.GetOrdinal("waypointId");
            var startDateOrdinal = reader.GetOrdinal("startDate");
            var endDateOrdinal = reader.GetOrdinal("endDate");
            var locationOrdinal = reader.GetOrdinal("location");
            var notesOrdinal = reader.GetOrdinal("notes");

            while (reader.Read())
                waypointsInTrip.Add(new Waypoint
                {
                    TripId = tripId,
                    WaypointId = reader.GetInt32(waypointIdOrdinal),
                    Location = reader.GetString(locationOrdinal),
                    StartDate = reader.GetDateTime(startDateOrdinal),
                    EndDate = reader.GetDateTime(endDateOrdinal),
                    Notes = reader.GetString(notesOrdinal)
                });

            this._connection.Close();
            return waypointsInTrip;
        }


        /// <summary>
        ///     Gets the waypyoints on date.
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

            cmd.Parameters.Add("@selectedDate", MySqlDbType.Date).Value = selectedDate;
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
                    Notes = reader.GetString(notesOrdinal)
                });

            this._connection.Close();
            return waypointsOnDate;
        }
    }
}