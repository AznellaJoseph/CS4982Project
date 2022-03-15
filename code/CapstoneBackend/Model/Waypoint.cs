using System;

namespace CapstoneBackend.Model
{
    /// <summary>
    ///     A model class for the Waypoint object
    /// </summary>
    public class Waypoint : IEvent
    {
        /// <summary>
        ///     The event type.
        /// </summary>
        public string EventType { get; } = nameof(Waypoint);

        /// <summary>
        ///     The id.
        /// </summary>
        public int Id => WaypointId;
        
        /// <summary>
        ///     The waypoint id.
        /// </summary>
        public int WaypointId { get; set; }

        /// <summary>
        ///     The location.
        /// </summary>
        public string Location { get; set; } = string.Empty;

        /// <summary>
        ///     The notes.
        /// </summary>
        public string Notes { get; set; } = string.Empty;

        /// <summary>
        ///     The trip id.
        /// </summary>
        public int TripId { get; set; }

        /// <summary>
        ///     The start date.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        ///     The end date.
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        ///     The display name.
        /// </summary>
        public string DisplayName => Location;
    }
}