using System;
using System.Collections.Generic;
using System.Linq;

namespace CapstoneBackend.Model
{
    public class EventManager
    {

        public WaypointManager WaypointManager { get; set; } = new();
        public TransportationManager TransportationManager { get; set; } = new();
        
        /// <summary>
        ///     Gets the sorted events on date.
        /// </summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="selectedDate">The selected date.</param>
        /// <returns> A response of the sorted events on that date </returns>
        public virtual Response<IList<IEvent>> GetEventsOnDate(int tripId, DateTime selectedDate)
        {
            List<IEvent> events = new List<IEvent>();
            IList<Waypoint>? waypoints = WaypointManager.GetWaypointsOnDate(tripId, selectedDate).Data; 
            IList<Transportation>? transportation = TransportationManager.GetTransportationOnDate(tripId, selectedDate).Data;
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