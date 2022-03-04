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
        /// Determines if the entered dates clash with another event's dates.
        /// </summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns></returns>
        public virtual Response<bool> DetermineIfEventDatesClash(int tripId, DateTime startDate, DateTime endDate)
        {
            var eventDates = Enumerable.Range(0,
                    (endDate - startDate).Days + 1)
                .Select(day => startDate.AddDays(day)).ToList();

            var clashes = !eventDates.Select(eventDate => GetEventsOnDate(tripId, eventDate).Data).Where(currentEvents => currentEvents != null).All(currentEvents => currentEvents != null && !currentEvents.Any(currentEvent => startDate >= currentEvent.StartDate && startDate <= currentEvent.EndDate || endDate >= currentEvent.StartDate && endDate <= currentEvent.EndDate));

            return new Response<bool>
            {
                Data = clashes
            };
        }

    }
}