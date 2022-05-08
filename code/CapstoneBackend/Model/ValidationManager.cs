using System;
using System.Collections.Generic;
using System.Linq;
using CapstoneBackend.Utils;

namespace CapstoneBackend.Model
{
    /// <summary>
    ///     ValidationManager to validate trip and event dates
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
        /// <returns>
        ///     A response of true if the dates are valid, or a non-success status code and error message specifying the
        ///     invalidity
        /// </returns>
        public virtual Response<bool> DetermineIfValidEventDates(int tripId, DateTime startDate, DateTime endDate)
        {
            if (startDate.CompareTo(endDate) > 0)
                return new Response<bool>
                {
                    StatusCode = (uint) Ui.StatusCode.BadRequest,
                    ErrorMessage = Ui.ErrorMessages.InvalidStartDate
                };

            var currentTrip = TripManager.GetTripByTripId(tripId).Data;

            if (currentTrip is null)
                return new Response<bool>
                {
                    ErrorMessage = Ui.ErrorMessages.TripNotFound,
                    StatusCode = (uint) Ui.StatusCode.DataNotFound
                };

            if (startDate.Date.CompareTo(currentTrip.StartDate.Date) < 0)
                return new Response<bool>
                {
                    ErrorMessage = Ui.ErrorMessages.EventStartDateBeforeTripStartDate +
                                   currentTrip.StartDate.ToShortDateString(),
                    StatusCode = (uint) Ui.StatusCode.BadRequest
                };

            if (startDate.Date.CompareTo(currentTrip.EndDate.Date) > 0)
                return new Response<bool>
                {
                    ErrorMessage = Ui.ErrorMessages.EventStartDateAfterTripEndDate +
                                   currentTrip.EndDate.ToShortDateString(),
                    StatusCode = (uint) Ui.StatusCode.BadRequest
                };

            if (endDate.Date.CompareTo(currentTrip.StartDate.Date) < 0)
                return new Response<bool>
                {
                    ErrorMessage = Ui.ErrorMessages.EventEndDateBeforeTripStartDate +
                                   currentTrip.StartDate.ToShortDateString(),
                    StatusCode = (uint) Ui.StatusCode.BadRequest
                };

            if (endDate.Date.CompareTo(currentTrip.EndDate.Date) > 0)
                return new Response<bool>
                {
                    ErrorMessage = Ui.ErrorMessages.EventEndDateAfterTripEndDate +
                                   currentTrip.EndDate.ToShortDateString(),
                    StatusCode = (uint) Ui.StatusCode.BadRequest
                };

            return new Response<bool>
            {
                Data = true
            };
        }

        /// <summary>
        ///     Finds the clashing event with the chosen start and end dates.
        /// </summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="excludingEvent">The excluding event.</param>
        /// <returns>
        ///     A response of the clashing event and a non-success code and error message specifying the clashing event dates
        ///     or an empty response.
        /// </returns>
        public virtual Response<IList<IEvent>> FindClashingEvents(int tripId, DateTime startDate,
            DateTime endDate, IEvent? excludingEvent)
        {
            var eventDates = Enumerable.Range(0,
                    (endDate - startDate).Days + 1)
                .Select(day => startDate.AddDays(day)).ToList();

            var clashingEvents = from eventDate in eventDates
                select EventManager.GetEventsOnDate(tripId, eventDate).Data
                into eventsOnDate
                where eventsOnDate is not null
                from eventOnDate in eventsOnDate
                where
                    startDate >= eventOnDate.StartDate && startDate <= eventOnDate.EndDate ||
                    endDate >= eventOnDate.StartDate && endDate <= eventOnDate.EndDate
                select eventOnDate;

            var clashingEventList = clashingEvents.ToList();

            if (excludingEvent is not null)
                clashingEventList.RemoveAll(clashingEvent => clashingEvent.Equals(excludingEvent));

            if (clashingEventList.Count == 0)
                return new Response<IList<IEvent>>
                {
                    Data = null
                };

            return new Response<IList<IEvent>>
            {
                Data = clashingEventList,
                ErrorMessage =
                    $"There {(clashingEventList.Count == 1 ? "is" : "are")} {clashingEventList.Count} clashing event{(clashingEventList.Count > 1 ? "s" : string.Empty)} from {clashingEventList.Min(clashingEvent => clashingEvent.StartDate)} to {clashingEventList.Max(clashingEvent => clashingEvent.EndDate)}",
                StatusCode = (uint) Ui.StatusCode.BadRequest
            };
        }

        /// <summary>
        ///     Determines if the input is a valid location based on the Google Place Search.
        /// </summary>
        /// <param name="locationInput">
        ///     The location input to validate.
        /// </param>
        /// <returns>
        ///     A response specifying the entered location is valid, a non-success status code and error method if otherwise
        /// </returns>
        public virtual Response<bool> DetermineIfValidLocation(string locationInput)
        {
            var isValid = GooglePlacesService.IsLocationValid(locationInput);

            if (isValid)
                return new Response<bool>
                {
                    Data = true
                };
            return new Response<bool>
            {
                ErrorMessage = Ui.ErrorMessages.InvalidLocation,
                StatusCode = (uint) Ui.StatusCode.BadRequest
            };
        }
    }
}