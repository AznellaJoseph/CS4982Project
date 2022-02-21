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
        /// <param name="notes">The notes.</param>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>
        ///     A response of if the waypoint was created in the database
        /// </returns>
        public virtual Response<int> CreateWaypoint(int tripId, string location, DateTime startTime, DateTime? endTime,
            string? notes)
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
                Data = _dal.CreateWaypoint(tripId, location, startTime, endTime, notes ?? string.Empty)
            };
        }

        /// <summary>
        ///     Gets the waypoints on date.
        /// </summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="selectedDate">The selected date.</param>
        /// <returns> A response of the waypoints on that date </returns>
        public virtual Response<IList<Waypoint>> GetWaypointsOnDate(int tripId, DateTime selectedDate)
        {
            var waypointsOnDate = _dal.GetWaypointsOnDate(tripId, selectedDate);

            return new Response<IList<Waypoint>>
            {
                Data = waypointsOnDate
            };
        }

        /// <summary>
        ///     Gets the waypoints by trip identifier.
        /// </summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <returns> A response of the waypoints with the entered trip id</returns>
        public virtual Response<IList<Waypoint>> GetWaypointsByTripId(int tripId)
        {
            var waypointsInTrip = _dal.GetWaypointsByTripId(tripId);

            return new Response<IList<Waypoint>>
            {
                Data = waypointsInTrip
            };
        }

        /// <summary>
        ///     Removes the waypoint.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns> A response specifying whether or not the waypoint was removed </returns>
        public virtual Response<bool> RemoveWaypoint(int id)
        {
            var removed = _dal.RemoveWaypoint(id);

            if (!removed)
                return new Response<bool>
                {
                    ErrorMessage = "The waypoint could not be removed.",
                    StatusCode = 400
                };

            return new Response<bool>
            {
                Data = removed
            };
        }
    }
}