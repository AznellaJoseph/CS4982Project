using System;
using System.Collections.Generic;
using CapstoneBackend.DAL;
using CapstoneBackend.Utils;
using MySql.Data.MySqlClient;

namespace CapstoneBackend.Model
{
    /// <summary>
    ///     A wrapper class for the TripDal. Manages the collection of Trips and informs of server errors.
    /// </summary>
    public class TripManager
    {
        private readonly TripDal _dal;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TripManager" /> class.
        /// </summary>
        public TripManager() : this(new TripDal())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="TripManager" /> class.
        /// </summary>
        /// <param name="dal">The dal.</param>
        public TripManager(TripDal dal)
        {
            _dal = dal;
        }

        /// <summary>Gets the trip specified by the given trip ID.</summary>
        /// <param name="tripId">The trip ID.</param>
        /// <returns>
        ///     A response with the Trip object or a non-success status code and error message
        /// </returns>
        public virtual Response<Trip> GetTripByTripId(int tripId)
        {
            Trip? trip;
            try
            {
                trip = _dal.GetTripByTripId(tripId);
            }
            catch (MySqlException e)
            {
                return new Response<Trip>
                {
                    StatusCode = e.Code,
                    ErrorMessage = e.Message
                };
            }
            catch (Exception)
            {
                return new Response<Trip>
                {
                    StatusCode = (uint) Ui.StatusCode.InternalServerError,
                    ErrorMessage = Ui.ErrorMessages.InternalServerError
                };
            }

            if (trip is null)
                return new Response<Trip>
                {
                    StatusCode = (uint) Ui.StatusCode.DataNotFound,
                    ErrorMessage = Ui.ErrorMessages.TripNotFound
                };
            return new Response<Trip>
            {
                Data = trip
            };
        }

        /// <summary>
        ///     Gets the trips of the user specified by the given id.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns> A response of a list of the user's trips or a non-success status code and error message </returns>
        public virtual Response<IList<Trip>> GetTripsByUser(int userId)
        {
            try
            {
                var trips = _dal.GetTripsByUserId(userId);
                return new Response<IList<Trip>>
                {
                    Data = trips
                };
            }
            catch (MySqlException e)
            {
                return new Response<IList<Trip>>
                {
                    StatusCode = e.Code,
                    ErrorMessage = e.Message
                };
            }
            catch (Exception e)
            {
                return new Response<IList<Trip>>
                {
                    StatusCode = (uint) Ui.StatusCode.InternalServerError,
                    ErrorMessage = e.Message
                };
            }
        }

        /// <summary>
        ///     Creates a trip with the entered values.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="notes">The notes.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns> A response of the id of the new trip or a non-success status code and error message </returns>
        public virtual Response<int> CreateTrip(int userId, string name, string? notes, DateTime startDate,
            DateTime endDate)
        {
            if (startDate.CompareTo(endDate) > 0)
                return new Response<int>
                {
                    ErrorMessage = Ui.ErrorMessages.InvalidStartDate,
                    StatusCode = (uint) Ui.StatusCode.BadRequest
                };

            try
            {
                var tripId = _dal.CreateTrip(userId, name, notes, startDate, endDate);
                return new Response<int>
                {
                    Data = tripId
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
            catch (Exception e)
            {
                return new Response<int>
                {
                    StatusCode = (uint) Ui.StatusCode.InternalServerError,
                    ErrorMessage = e.Message
                };
            }
        }
    }
}