using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapstoneBackend.DAL;

namespace CapstoneBackend.Model
{

    /// <summary>
    ///     A wrapper class for the WaypointDal. Manages the collection of Waypoints and informs of server errors.
    /// </summary>
    class WaypointManager
    {
        private readonly WaypointDal _dal;


        /// <summary>
        /// Initializes a new instance of the <see cref="WaypointManager"/> class.
        /// </summary>
        public WaypointManager() : this(new WaypointDal())
        {

        }


        /// <summary>
        /// Initializes a new instance of the <see cref="WaypointManager"/> class.
        /// </summary>
        /// <param name="dal">The dal.</param>
        public WaypointManager(WaypointDal dal)
        {
            _dal = dal;
        }

        public Response<int> CreateWaypoint(int tripId, string location, DateTime startTime, DateTime endTime)
        {
            if (startTime.CompareTo(endTime) > 0)
            {
                return new Response<int>
                {
                    StatusCode = 400,
                    ErrorMessage = "The start time cannot be before the end time."
                };
            }
            return new Response<int>
            {
                StatusCode = 200,
                Data = _dal.CreateWaypoint(tripId, location, startTime, endTime)
            };
        }

    }
}
