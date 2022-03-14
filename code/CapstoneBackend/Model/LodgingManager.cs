﻿using System;
using System.Collections.Generic;
using CapstoneBackend.DAL;
using CapstoneBackend.Utils;
using MySql.Data.MySqlClient;

namespace CapstoneBackend.Model
{
    /// <summary>
    ///     A wrapper class for the LodgingDal. Manages the collection of Lodgings and informs of server errors.
    /// </summary>
    public class LodgingManager
    {
        private readonly LodgingDal _dal;

        /// <summary>
        ///     Initializes a new instance of the <see cref="LodgingManager" /> class.
        /// </summary>
        public LodgingManager() : this(new LodgingDal())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="LodgingManager" /> class.
        /// </summary>
        /// <param name="dal">The dal.</param>
        public LodgingManager(LodgingDal dal)
        {
            _dal = dal;
        }

        /// <summary>
        ///     Creates the Lodging.
        /// </summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="location">The location.</param>
        /// <param name="notes">The notes.</param>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>
        ///     A response of if the Lodging was created in the database or a non-success status code and error message
        /// </returns>
        public virtual Response<int> CreateLodging(int tripId, string location, DateTime startTime, DateTime endTime,
            string? notes)
        {
            if (startTime.CompareTo(endTime) > 0)
                return new Response<int>
                {
                    StatusCode = (uint) Ui.StatusCode.BadRequest,
                    ErrorMessage = Ui.ErrorMessages.InvalidStartDate
                };

            try
            {
                var lodgingId = _dal.CreateLodging(tripId, location, startTime, endTime, notes);
                return new Response<int>
                {
                    Data = lodgingId
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
        ///     Gets the Lodgings on date.
        /// </summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="selectedDate">The selected date.</param>
        /// <returns> A response of the Lodgings on that date or a non-success status code and error message. </returns>
        public virtual Response<IList<Lodging>> GetLodgingsOnDate(int tripId, DateTime selectedDate)
        {
            try
            {
                var lodgingsOnDate = _dal.GetLodgingsOnDate(tripId, selectedDate);

                return new Response<IList<Lodging>>
                {
                    Data = lodgingsOnDate
                };
            }
            catch (MySqlException e)
            {
                return new Response<IList<Lodging>>
                {
                    StatusCode = e.Code,
                    ErrorMessage = e.Message
                };
            }
            catch (Exception)
            {
                return new Response<IList<Lodging>>
                {
                    StatusCode = (uint) Ui.StatusCode.InternalServerError,
                    ErrorMessage = Ui.ErrorMessages.InternalServerError
                };
            }
        }

        /// <summary>
        ///     Gets the Lodgings by trip identifier.
        /// </summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <returns> A response of the Lodgings with the entered trip LodgingId or a non-success status code and error message.</returns>
        public virtual Response<IList<Lodging>> GetLodgingsByTripId(int tripId)
        {
            try
            {
                var lodgingsInTrip = _dal.GetLodgingsByTripId(tripId);

                return new Response<IList<Lodging>>
                {
                    Data = lodgingsInTrip
                };
            }
            catch (MySqlException e)
            {
                return new Response<IList<Lodging>>
                {
                    StatusCode = e.Code,
                    ErrorMessage = e.Message
                };
            }
            catch (Exception)
            {
                return new Response<IList<Lodging>>
                {
                    StatusCode = (uint) Ui.StatusCode.InternalServerError,
                    ErrorMessage = Ui.ErrorMessages.InternalServerError
                };
            }
        }

        /// <summary>
        ///     Removes the Lodging.
        /// </summary>
        /// <param name="lodgingId">The identifier.</param>
        /// <returns> A response specifying whether or not the Lodging was removed or a non-success status code and error message. </returns>
        public virtual Response<bool> RemoveLodging(int lodgingId)
        {
            try
            {
                var removed = _dal.RemoveLodging(lodgingId);

                if (!removed)
                    return new Response<bool>
                    {
                        ErrorMessage = Ui.ErrorMessages.LodgingNotFound,
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
    }
}