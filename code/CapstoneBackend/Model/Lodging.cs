using System;

namespace CapstoneBackend.Model
{
    public class Lodging
    {
        /// <summary>
        ///     The location id.
        /// </summary>
        public int LodgingId { get; set; }

        /// <summary>
        ///     The location.
        /// </summary>
        public string Location { get; set; } = string.Empty;

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
        ///     The notes.
        /// </summary>
        public string Notes { get; set; } = string.Empty;
    }
}