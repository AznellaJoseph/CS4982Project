using System;
using System.Collections.Generic;
using System.Linq;

namespace CapstoneBackend.Model
{
    /// <summary>
    ///     A wrapper class to hold and manage the events (waypoint and transportation) of a trip.
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
    }
}