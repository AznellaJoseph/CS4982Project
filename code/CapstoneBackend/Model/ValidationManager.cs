using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
        /// <returns>
        ///     A response of true if the dates are valid, or a non-success status code and error message specifying the
        ///     invalidity
        /// </returns>
        public virtual Response<bool> DetermineIfValidEventDates(int tripId, DateTime startDate, DateTime endDate)
        {
            var currentTrip = TripManager.GetTripByTripId(tripId).Data;

            if (currentTrip is null)
                return new Response<bool>
                {
                    ErrorMessage = Ui.ErrorMessages.TripNotFound,
                    StatusCode = (uint) Ui.StatusCode.DataNotFound
                };

            if (startDate.CompareTo(currentTrip.StartDate) < 0)
                return new Response<bool>
                {
                    ErrorMessage = Ui.ErrorMessages.EventStartDateBeforeTripStartDate +
                                   currentTrip.StartDate.ToShortDateString(),
                    StatusCode = (uint) Ui.StatusCode.BadRequest
                };

            if (startDate.CompareTo(currentTrip.EndDate) > 0)
                return new Response<bool>
                {
                    ErrorMessage = Ui.ErrorMessages.EventStartDateAfterTripEndDate +
                                   currentTrip.EndDate.ToShortDateString(),
                    StatusCode = (uint) Ui.StatusCode.BadRequest
                };

            if (endDate.CompareTo(currentTrip.StartDate) < 0)
                return new Response<bool>
                {
                    ErrorMessage = Ui.ErrorMessages.EventEndDateBeforeTripStartDate +
                                   currentTrip.StartDate.ToShortDateString(),
                    StatusCode = (uint) Ui.StatusCode.BadRequest
                };

            if (endDate.CompareTo(currentTrip.EndDate) > 0)
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
        ///     Determines if there is a clashing trip with the chosen start and end dates.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns> A response of false if a clashing trip does not exist or a non-success code and error message specifying the clashing trip dates.</returns>
        public virtual Response<bool> DetermineIfClashingTripExists(int userId, DateTime startDate, DateTime endDate)
        {
            var tripDates = Enumerable.Range(0,
                    (endDate - startDate).Days + 1)
                .Select(day => startDate.AddDays(day)).ToList();

            var userTrips = TripManager.GetTripsByUser(userId);
            if (userTrips.Data == null)
                return new Response<bool>
                {
                    Data = false
                };

            var clashingTrip = (from tripDate in tripDates
                from userTrip in userTrips.Data
                where tripDate >= userTrip.StartDate && tripDate <= userTrip.EndDate
                select userTrip).FirstOrDefault();

            if (clashingTrip is null)
                return new Response<bool>
                {
                    Data = false
                };

            return new Response<bool>
            {
                ErrorMessage =
                    $"{Ui.ErrorMessages.ClashingTripDates} {clashingTrip.StartDate.ToShortDateString()} to {clashingTrip.EndDate.ToShortDateString()}.",
                StatusCode = (uint) Ui.StatusCode.BadRequest
            };
        }

        /// <summary>
        ///     Finds the clashing event with the chosen start and end dates.
        /// </summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>A response of false if a clashing event does not exist or a non-success code and error message specifying the clashing event dates.</returns>
        public virtual Response<bool> DetermineIfClashingEventExists(int tripId, DateTime startDate, DateTime endDate)
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
                return new Response<bool>
                {
                    Data = false
                };

            return new Response<bool>
            {
                ErrorMessage =
                    $"{Ui.ErrorMessages.ClashingEventDates} {clashingEvent.StartDate} to {clashingEvent.EndDate}.",
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
        ///     Success Response if valid, Error Response if invalid
        /// </returns>
        public virtual Response<bool> DetermineIfValidLocation(string locationInput)
        {
            var isValid = GooglePlacesService.IsLocationValid(locationInput);

            if (isValid)
            {
                return new Response<bool>
                {
                    Data = true
                };
            }
            else
            {
                return new Response<bool>
                {
                    ErrorMessage = Ui.ErrorMessages.InvalidLocation,
                    StatusCode = (uint) Ui.StatusCode.BadRequest
                };
            }
        }
    }
}
