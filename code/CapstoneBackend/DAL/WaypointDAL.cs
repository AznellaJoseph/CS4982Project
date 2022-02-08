using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace CapstoneBackend.DAL
{

    /// <summary>
    /// Data Access Layer (DAL) for Waypoint
    /// </summary>
    class WaypointDal
    {
        private readonly MySqlConnection _connection;


        /// <summary>
        /// Initializes a new instance of the <see cref="WaypointDal"/> class.
        /// </summary>
        public WaypointDal() : this(new MySqlConnection(Connection.ConnectionString))
        {

        }


        /// <summary>
        /// Initializes a new instance of the <see cref="WaypointDal"/> class.
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
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns> The waypoint number </returns>
        public int CreateWaypoint(int tripId, string location, DateTime startTime, DateTime endTime)
        {
            this._connection.Open();
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
    }
}
