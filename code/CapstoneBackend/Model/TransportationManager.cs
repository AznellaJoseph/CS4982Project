using System;
using System.Collections.Generic;
using CapstoneBackend.DAL;
using CapstoneBackend.Utils;

namespace CapstoneBackend.Model
{
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
        /// <returns>
        ///     A response of if the transportation was created in the database
        /// </returns>
        public virtual Response<int> CreateTransportation(int tripId, string method, DateTime startTime, DateTime? endTime)
        {
            if (startTime.CompareTo(endTime) > 0)
                return new Response<int>
                {
                    StatusCode = 400,
                    ErrorMessage = Ui.ErrorMessages.InvalidStartDate
                };
            return new Response<int>
            {
                StatusCode = 200,
                Data = _dal.CreateTransportation(tripId, method, startTime, endTime)
            };
        }

        /// <summary>
        ///     Gets the transportations on date.
        /// </summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="selectedDate">The selected date.</param>
        /// <returns> A response of the transportation on that date </returns>
        public virtual Response<IList<Transportation>> GetTransportationOnDate(int tripId, DateTime selectedDate)
        {
            var transportationsOnDate = _dal.GetTransportationOnDate(tripId, selectedDate);

            return new Response<IList<Transportation>>
            {
                Data = transportationsOnDate
            };
        }

        /// <summary>
        ///     Removes the transportation.
        /// </summary>
        /// <param name="transportationId">The identifier.</param>
        /// <returns> A response specifying whether or not the transportation was removed </returns>
        public virtual Response<bool> RemoveTransportation(int transportationId)
        {
            var removed = _dal.RemoveTransportation(transportationId);

            if (!removed)
                return new Response<bool>
                {
                    ErrorMessage = Ui.ErrorMessages.TransportationNotFound,
                    StatusCode = 400
                };

            return new Response<bool>
            {
                Data = removed
            };
        }
    }
}