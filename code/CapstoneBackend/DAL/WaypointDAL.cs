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
        ///     Creates a waypoint.
        /// </summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="location">The location.</param>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns> The waypoint number </returns>
        public virtual int CreateWaypoint(int tripId, string location, DateTime startTime, DateTime endTime)
        {
            _connection.Open();
            const string procedure = "uspCreateWaypoint";
            using MySqlCommand cmd = new(procedure, _connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@tripId", MySqlDbType.Int32).Value = tripId;
            cmd.Parameters.Add("@location", MySqlDbType.VarChar).Value = location;
            cmd.Parameters.Add("startTime", MySqlDbType.DateTime).Value = startTime;
            cmd.Parameters.Add("@endTime", MySqlDbType.DateTime).Value = endTime;

            var waypointNum = Convert.ToInt32(cmd.ExecuteScalar());

            return waypointNum;
        }


        /// <summary>
        ///     Gets the waypyoints on date.
        /// </summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="selectedDate">The selected date.</param>
        /// <returns> A list of the waypoints of the trip on the specified date </returns>
        public virtual IList<Waypoint> GetWaypyointsOnDate(int tripId, DateTime selectedDate)
        {
            _connection.Open();
            const string procedure = "uspGetWaypointsOnDate";
            using MySqlCommand cmd = new(procedure, _connection);
            cmd.CommandType = CommandType.StoredProcedure;
            IList<Waypoint> waypointsOnDate = new List<Waypoint>();

            cmd.Parameters.Add("@tripId", MySqlDbType.Int32).Value = tripId;
            cmd.Parameters.Add("@selectedDate", MySqlDbType.Date).Value = selectedDate;

            using var reader = cmd.ExecuteReader();

            var waypointNumOrdinal = reader.GetOrdinal("waypointNum");
            var startTimeOrdinal = reader.GetOrdinal("startTime");
            var endTimeOrdinal = reader.GetOrdinal("endTime");
            var locationOrdinal = reader.GetOrdinal("location");

            while (reader.Read())
                waypointsOnDate.Add(new Waypoint
                {
                    TripId = tripId,
                    WaypointNum = reader.GetInt32(waypointNumOrdinal),
                    Location = reader.GetString(locationOrdinal),
                    StartTime = reader.GetDateTime(startTimeOrdinal),
                    EndTime = reader.GetDateTime(endTimeOrdinal)
                });

            return waypointsOnDate;
        }
    }
}