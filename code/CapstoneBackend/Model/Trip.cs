using System;

namespace CapstoneBackend.Model
{
    /// <summary>
    ///     A model class for the Trip object
    /// </summary>
    public class Trip
    {
        /// <summary>
        ///     The trip id
        /// </summary>
        public virtual int TripId { get; set; }

        /// <summary>
        ///     The user id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        ///     The name
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        ///     The notes
        /// </summary>
        public string Notes { get; set; } = string.Empty;

        /// <summary>
        ///     The start date
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        ///     The end date
        /// </summary>
        public DateTime EndDate { get; set; }
    }
}