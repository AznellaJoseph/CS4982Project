using System;
using System.Collections.Generic;
using CapstoneBackend.DAL;
using CapstoneBackend.Utils;
using MySql.Data.MySqlClient;

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
        ///     A response of the id of the new waypoint or a non-success status code and error message
        /// </returns>
        public virtual Response<int> CreateWaypoint(int tripId, string location, DateTime startTime, DateTime endTime,
            string? notes)
        {
            try
            {
                var waypointId = _dal.CreateWaypoint(tripId, location, startTime, endTime, notes);
                return new Response<int>
                {
                    Data = waypointId
                };
            }
            catch (MySqlException e)
            {
                return new Response<int>
                {
                    StatusCode = e.Code,
                    ErrorMessage = e.Message
                };
            }
            catch (Exception)
            {
                return new Response<int>
                {
                    StatusCode = (uint) Ui.StatusCode.InternalServerError,
                    ErrorMessage = Ui.ErrorMessages.InternalServerError
                };
            }
        }

        /// <summary>
        ///     Gets the waypoints on date.
        /// </summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="selectedDate">The selected date.</param>
        /// <returns> A response of the waypoints on that date or a non-success status code and error message. </returns>
        public virtual Response<IList<Waypoint>> GetWaypointsOnDate(int tripId, DateTime selectedDate)
        {
            try
            {
                var waypointsOnDate = _dal.GetWaypointsOnDate(tripId, selectedDate);

                return new Response<IList<Waypoint>>
                {
                    Data = waypointsOnDate
                };
            }
            catch (MySqlException e)
            {
                return new Response<IList<Waypoint>>
                {
                    StatusCode = e.Code,
                    ErrorMessage = e.Message
                };
            }
            catch (Exception)
            {
                return new Response<IList<Waypoint>>
                {
                    StatusCode = (uint) Ui.StatusCode.InternalServerError,
                    ErrorMessage = Ui.ErrorMessages.InternalServerError
                };
            }
        }


        /// <summary>
        ///     Removes the waypoint.
        /// </summary>
        /// <param name="waypointId">The identifier.</param>
        /// <returns> A response specifying the waypoint was removed or a non-success status code and error message. </returns>
        public virtual Response<bool> RemoveWaypoint(int waypointId)
        {
            try
            {
                var removed = _dal.RemoveWaypoint(waypointId);

                if (!removed)
                    return new Response<bool>
                    {
                        ErrorMessage = Ui.ErrorMessages.WaypointNotFound,
                        StatusCode = (uint) Ui.StatusCode.BadRequest
                    };

                return new Response<bool>
                {
                    Data = removed
                };
            }
            catch (MySqlException e)
            {
                return new Response<bool>
                {
                    StatusCode = e.Code,
                    ErrorMessage = e.Message
                };
            }
            catch (Exception)
            {
                return new Response<bool>
                {
                    StatusCode = (uint) Ui.StatusCode.InternalServerError,
                    ErrorMessage = Ui.ErrorMessages.InternalServerError
                };
            }
        }

        /// <summary>
        ///     Gets the waypoint by identifier.
        /// </summary>
        /// <param name="waypointId">The waypoint identifier.</param>
        /// <returns>A response of the waypoint with the given id or a non-success status code and error message</returns>
        public virtual Response<Waypoint> GetWaypointById(int waypointId)
        {
            try
            {
                var waypoint = _dal.GetWaypointById(waypointId);

                if (waypoint is null)
                    return new Response<Waypoint>
                    {
                        ErrorMessage = Ui.ErrorMessages.WaypointNotFound,
                        StatusCode = (uint) Ui.StatusCode.DataNotFound
                    };

                return new Response<Waypoint> {Data = waypoint};
            }
            catch (MySqlException e)
            {
                return new Response<Waypoint>
                {
                    StatusCode = e.Code,
                    ErrorMessage = e.Message
                };
            }
            catch (Exception)
            {
                return new Response<Waypoint>
                {
                    StatusCode = (uint) Ui.StatusCode.InternalServerError,
                    ErrorMessage = Ui.ErrorMessages.InternalServerError
                };
            }
        }

        /// <summary>
        ///     Edits the waypoint.
        /// </summary>
        /// <param name="waypoint">The waypoint.</param>
        /// <returns>
        ///     A response specifying the waypoint was updated or a non-success code and error message
        /// </returns>
        public virtual Response<bool> EditWaypoint(Waypoint waypoint)
        {
            try
            {
                var updated = _dal.EditWaypoint(waypoint);

                if (!updated)
                    return new Response<bool>
                    {
                        StatusCode = (uint) Ui.StatusCode.DataNotFound, ErrorMessage = Ui.ErrorMessages.WaypointNotFound
                    };
                return new Response<bool> {Data = updated};
            }
            catch (MySqlException e)
            {
                return new Response<bool>
                {
                    StatusCode = e.Code,
                    ErrorMessage = e.Message
                };
            }
            catch (Exception)
            {
                return new Response<bool>
                {
                    StatusCode = (uint) Ui.StatusCode.InternalServerError,
                    ErrorMessage = Ui.ErrorMessages.InternalServerError
                };
            }
        }
    }
}