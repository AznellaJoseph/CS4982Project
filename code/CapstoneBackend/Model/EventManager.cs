using System;
using System.Collections.Generic;
using System.Linq;

namespace CapstoneBackend.Model
{
    /// <summary>
    ///     A wrapper class to hold the events (waypoint and transportation) objects of a trip. Manages the collection of
    ///     events
    /// </summary>
    public class EventManager
    {
        /// <summary>
        ///     The waypoint manager.
        /// </summary>
        public WaypointManager WaypointManager { get; set; } = new();

        /// <summary>
        ///     The transportation manager.
        /// </summary>
        public TransportationManager TransportationManager { get; set; } = new();

        /// <summary>
        ///     Gets the sorted events on date.
        /// </summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="selectedDate">The selected date.</param>
        /// <returns> A response of the sorted events on that date </returns>
        public virtual Response<IList<IEvent>> GetEventsOnDate(int tripId, DateTime selectedDate)
        {
            var events = new List<IEvent>();
            var waypoints = WaypointManager.GetWaypointsOnDate(tripId, selectedDate).Data;
            var transportation = TransportationManager.GetTransportationOnDate(tripId, selectedDate).Data;
            events.AddRange(waypoints ?? Enumerable.Empty<IEvent>());
            events.AddRange(transportation ?? Enumerable.Empty<IEvent>());
            events.Sort();
            return new Response<IList<IEvent>>
            {
                Data = events
            };
        }

        /// <summary>
        /// Finds the clashing event with the chosen start and end dates.
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

            var clashingEvent = (from eventDate in eventDates select GetEventsOnDate(tripId, eventDate).Data into eventsOnDate where eventsOnDate is not null from eventOnDate in eventsOnDate where startDate >= eventOnDate.StartDate && startDate <= eventOnDate.EndDate || endDate >= eventOnDate.StartDate && endDate <= eventOnDate.EndDate select eventOnDate).FirstOrDefault();

            return new Response<IEvent>
            {
                Data = clashingEvent
            };
        }

    }
}