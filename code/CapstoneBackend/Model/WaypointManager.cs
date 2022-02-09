using System;
using System.Collections.Generic;
using CapstoneBackend.DAL;

namespace CapstoneBackend.Model
{
    /// <summary>
    ///     A wrapper class for the WaypointDal. Manages the collection of Waypoints and informs of server errors.
    /// </summary>
    public class WaypointManager
    {
        private readonly WaypointDal _dal;


        /// <summary>
        ///     Initializes a new instance of the <see cref="WaypointManager" /> class.
        /// </summary>
        public WaypointManager() : this(new WaypointDal())
        {
        }


        /// <summary>
        ///     Initializes a new instance of the <see cref="WaypointManager" /> class.
        /// </summary>
        /// <param name="dal">The dal.</param>
        public WaypointManager(WaypointDal dal)
        {
            _dal = dal;
        }


        /// <summary>
        ///     Creates the waypoint.
        /// </summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="location">The location.</param>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns> A response of if the waypoint was created in the database </returns>
        public Response<int> CreateWaypoint(int tripId, string location, DateTime startTime, DateTime endTime)
        {
            if (startTime.CompareTo(endTime) > 0)
                return new Response<int>
                {
                    StatusCode = 400,
                    ErrorMessage = "The start time cannot be before the end time."
                };
            return new Response<int>
            {
                StatusCode = 200,
                Data = _dal.CreateWaypoint(tripId, location, startTime, endTime)
            };
        }


        /// <summary>
        ///     Gets the waypoints on date.
        /// </summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="selectedDate">The selected date.</param>
        /// <returns> A response of the waypoints on that date or an error message if there are none </returns>
        public Response<IList<Waypoint>> GetWaypointsOnDate(int tripId, DateTime selectedDate)
        {
            var waypointsOnDate = _dal.GetWaypyointsOnDate(tripId, selectedDate);

            return new Response<IList<Waypoint>>
            {
                Data = waypointsOnDate
            };
        }
    }
}