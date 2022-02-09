using System;

namespace CapstoneBackend.Model
{
    /// <summary>
    ///     Waypoint Model
    /// </summary>
    public class Waypoint
    {
        /// <summary>
        ///     The trip identifier.
        /// </summary>
        public int TripId { get; set; }

        /// <summary>
        ///     The waypoint number.
        /// </summary>
        public int WaypointNum { get; set; }

        /// <summary>
        ///     The location.
        /// </summary>
        public string Location { get; set; } = string.Empty;

        /// <summary>
        ///     The start time.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        ///     The end time.
        /// </summary>
        public DateTime EndTime { get; set; }
    }
}