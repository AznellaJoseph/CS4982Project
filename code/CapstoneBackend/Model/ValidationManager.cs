using System;
using System.Linq;
using CapstoneBackend.Utils;

namespace CapstoneBackend.Model
{
    /// <summary>
    ///     ValidationManager to validate trips and event dates
    /// </summary>
    public class ValidationManager
    {
        /// <summary>
        ///     The trip manager.
        /// </summary>
        public TripManager TripManager { get; set; } = new();

        /// <summary>
        ///     The event manager.
        /// </summary>
        public EventManager EventManager { get; set; } = new();


        /// <summary>
        ///     Determines if the entered event dates are valid in relation to the trip.
        /// </summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>A response of true if the dates are valid, false otherwise. </returns>
        public virtual Response<bool> DetermineIfValidEventDates(int tripId, DateTime startDate, DateTime endDate)
        {
            var currentTrip = TripManager.GetTripByTripId(tripId).Data;

            if (currentTrip is null)
                return new Response<bool>
                {
                    ErrorMessage = Ui.ErrorMessages.TripNotFound,
                    StatusCode = (uint)Ui.StatusCode.DataNotFound
                };

            if (startDate.CompareTo(currentTrip.StartDate) < 0)
                return new Response<bool>
                {
                    ErrorMessage = Ui.ErrorMessages.EventStartDateBeforeTripStartDate +
                                   currentTrip.StartDate.ToShortDateString(),
                    StatusCode = (uint)Ui.StatusCode.BadRequest
                };

            if (startDate.CompareTo(currentTrip.EndDate) > 0)
                return new Response<bool>
                {
                    ErrorMessage = Ui.ErrorMessages.EventStartDateAfterTripEndDate +
                                   currentTrip.EndDate.ToShortDateString(),
                    StatusCode = (uint)Ui.StatusCode.BadRequest
                };

            if (endDate.CompareTo(currentTrip.StartDate) < 0)
                return new Response<bool>
                {
                    ErrorMessage = Ui.ErrorMessages.EventEndDateBeforeTripStartDate +
                                   currentTrip.StartDate.ToShortDateString(),
                    StatusCode = (uint)Ui.StatusCode.BadRequest
                };

            if (endDate.CompareTo(currentTrip.EndDate) > 0)
                return new Response<bool>
                {
                    ErrorMessage = Ui.ErrorMessages.EventEndDateAfterTripEndDate +
                                   currentTrip.EndDate.ToShortDateString(),
                    StatusCode = (uint)Ui.StatusCode.BadRequest
                };

            return new Response<bool>
            {
                Data = true
            };
        }

        /// <summary>
        ///     Finds the clashing trip with the chosen start and end dates.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns> A response of the clashing trip or a null trip if there is none.</returns>
        public virtual Response<Trip> FindClashingTrip(int userId, DateTime startDate, DateTime endDate)
        {
            var tripDates = Enumerable.Range(0,
                    (endDate - startDate).Days + 1)
                .Select(day => startDate.AddDays(day)).ToList();

            var userTrips = TripManager.GetTripsByUser(userId);
            if (userTrips.Data == null)
                return new Response<Trip>
                {
                    Data = null
                };

            var clashingTrip = (from tripDate in tripDates
                                from userTrip in userTrips.Data
                                where tripDate >= userTrip.StartDate && tripDate <= userTrip.EndDate
                                select userTrip).FirstOrDefault();

            if (clashingTrip is null)
            {
                return new Response<Trip>
                {
                    Data = clashingTrip
                };
            }

            return new Response<Trip>
            {
                ErrorMessage =
                    $"{Ui.ErrorMessages.ClashingTripDates} {clashingTrip.StartDate.ToShortDateString()} to {clashingTrip.EndDate.ToShortDateString()}.",
                StatusCode = (uint)Ui.StatusCode.BadRequest
            };



        }

        /// <summary>
        ///     Finds the clashing event with the chosen start and end dates.
        /// </summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>A response of the clashing event or a null event if there is none.</returns>
        public virtual Response<IEvent> FindClashingEvent(int tripId, DateTime startDate, DateTime endDate)
        {
            var eventDates = Enumerable.Range(0,
                    (endDate - startDate).Days + 1)
                .Select(day => startDate.AddDays(day)).ToList();

            var clashingEvent = (from eventDate in eventDates
                                 select EventManager.GetEventsOnDate(tripId, eventDate).Data
                into eventsOnDate
                                 where eventsOnDate is not null
                                 from eventOnDate in eventsOnDate
                                 where startDate >= eventOnDate.StartDate && startDate <= eventOnDate.EndDate ||
                                       endDate >= eventOnDate.StartDate && endDate <= eventOnDate.EndDate
                                 select eventOnDate).FirstOrDefault();

            if (clashingEvent is null)
            {
                return new Response<IEvent>
                {
                    Data = clashingEvent
                };
            }

            return new Response<IEvent>
            {
                ErrorMessage =
                    $"{Ui.ErrorMessages.ClashingEventDates} {clashingEvent.StartDate} to {clashingEvent.EndDate}.",
                StatusCode = (uint)Ui.StatusCode.BadRequest
            };
        }
    }
}