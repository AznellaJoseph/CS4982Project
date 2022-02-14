using System;

namespace CapstoneBackend.Model
{
    /// <summary>
    ///     Waypoint Model
    /// </summary>
    public class Waypoint
    {
        /// <summary>
        ///     The trip id.
        /// </summary>
        public int TripId { get; set; }

        /// <summary>
        ///     The waypoint id.
        /// </summary>
        public int WaypointId { get; set; }

        /// <summary>
        ///     The location.
        /// </summary>
        public string Location { get; set; } = string.Empty;

        /// <summary>
        ///     The start date.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        ///     The end date.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        ///     The notes.
        /// </summary>
        public string Notes { get; set; } = string.Empty;
    }
}