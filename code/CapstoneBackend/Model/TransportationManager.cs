using System;
using System.Collections.Generic;
using CapstoneBackend.DAL;
using CapstoneBackend.Utils;
using MySql.Data.MySqlClient;

namespace CapstoneBackend.Model
{
    /// <summary>
    ///     A wrapper class for the TransportationDal. Manages the collection of Transportation and informs of server errors.
    /// </summary>
    public class TransportationManager
    {
        private readonly TransportationDal _dal;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TransportationManager" /> class.
        /// </summary>
        public TransportationManager() : this(new TransportationDal())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="TransportationManager" /> class.
        /// </summary>
        /// <param name="dal">The dal.</param>
        public TransportationManager(TransportationDal dal)
        {
            _dal = dal;
        }

        /// <summary>
        ///     Creates the transportation.
        /// </summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="method">The method.</param>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <param name="notes">The notes.</param>
        /// <returns>
        ///     A response of the id of the new transportation or a non-success status code and error message
        /// </returns>
        public virtual Response<int> CreateTransportation(int tripId, string method, DateTime startTime,
            DateTime endTime, string? notes)
        {
            try
            {
                return new Response<int>
                {
                    Data = _dal.CreateTransportation(tripId, method, startTime, endTime, notes)
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
        ///     Gets the transportation on date.
        /// </summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="selectedDate">The selected date.</param>
        /// <returns> A response of the transportation on that date or a non-success status code and error message</returns>
        public virtual Response<IList<Transportation>> GetTransportationOnDate(int tripId, DateTime selectedDate)
        {
            try
            {
                var transportationOnDate = _dal.GetTransportationOnDate(tripId, selectedDate);

                return new Response<IList<Transportation>>
                {
                    Data = transportationOnDate
                };
            }
            catch (MySqlException e)
            {
                return new Response<IList<Transportation>>
                {
                    StatusCode = e.Code,
                    ErrorMessage = e.Message
                };
            }
            catch (Exception)
            {
                return new Response<IList<Transportation>>
                {
                    StatusCode = (uint) Ui.StatusCode.InternalServerError,
                    ErrorMessage = Ui.ErrorMessages.InternalServerError
                };
            }
        }

        /// <summary>
        ///     Removes the transportation.
        /// </summary>
        /// <param name="transportationId">The identifier.</param>
        /// <returns> A response specifying the transportation was removed or a non-success status code and error message </returns>
        public virtual Response<bool> RemoveTransportation(int transportationId)
        {
            try
            {
                var removed = _dal.RemoveTransportation(transportationId);

                if (!removed)
                    return new Response<bool>
                    {
                        ErrorMessage = Ui.ErrorMessages.TransportationNotFound,
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
        ///     Gets the transportation by identifier.
        /// </summary>
        /// <param name="transportationId">The transportation identifier.</param>
        /// <returns>A response of the transportation with the given id or a non-success code and error message</returns>
        public virtual Response<Transportation> GetTransportationById(int transportationId)
        {
            try
            {
                var transportation = _dal.GetTransportationById(transportationId);

                if (transportation is null)
                    return new Response<Transportation>
                    {
                        ErrorMessage = Ui.ErrorMessages.TransportationNotFound,
                        StatusCode = (uint) Ui.StatusCode.DataNotFound
                    };

                return new Response<Transportation> {Data = transportation};
            }
            catch (MySqlException e)
            {
                return new Response<Transportation>
                {
                    StatusCode = e.Code,
                    ErrorMessage = e.Message
                };
            }
            catch (Exception)
            {
                return new Response<Transportation>
                {
                    StatusCode = (uint) Ui.StatusCode.InternalServerError,
                    ErrorMessage = Ui.ErrorMessages.InternalServerError
                };
            }
        }

        /// <summary>
        ///     Edits the transportation.
        /// </summary>
        /// <param name="transportation">The transportation.</param>
        /// <returns>A response specifying the transportation was updated or a non-success code and error message</returns>
        public virtual Response<bool> EditTransportation(Transportation transportation)
        {
            try
            {
                var updated = _dal.EditTransportation(transportation);

                if (!updated)
                    return new Response<bool>
                    {
                        StatusCode = (uint) Ui.StatusCode.DataNotFound,
                        ErrorMessage = Ui.ErrorMessages.TransportationNotFound
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