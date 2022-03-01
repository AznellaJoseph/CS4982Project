using System;

namespace CapstoneBackend.Model
{
    /// <summary>
    ///     A model class for the Transportation object
    /// </summary>
    /// <seealso cref="CapstoneBackend.Model.IEvent" />
    public class Transportation : IEvent
    {
        /// <summary>
        ///     The transportation id.
        /// </summary>
        public int TransportationId { get; set; }

        /// <summary>
        ///     The method.
        /// </summary>
        public string Method { get; set; } = string.Empty;

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

        public string DisplayName => Method;
    }
}