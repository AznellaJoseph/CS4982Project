using System;

namespace CapstoneBackend.Model
{
    public class Transportation : IEvent
    {
        /// <summary>
        ///     The trip id.
        /// </summary>
        public int TripId { get; set; }

        /// <summary>
        ///     The waypoint id.
        /// </summary>
        public int TransportationId { get; set; }

        /// <summary>
        ///     The location.
        /// </summary>
        public string Method { get; set; } = string.Empty;

        /// <summary>
        ///     The start date.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        ///     The end date.
        /// </summary>
        public DateTime? EndDate { get; set; }
    }
}