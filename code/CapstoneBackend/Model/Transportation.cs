using System;

namespace CapstoneBackend.Model
{
    /// <summary>
    ///     A model class for the Transportation object
    /// </summary>
    /// <seealso cref="CapstoneBackend.Model.IEvent" />
    public class Transportation : IEvent
    {
        public string EventType { get; } = nameof(Transportation);

        public int Id => TransportationId;

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

        /// <summary>
        ///     The display name.
        /// </summary>
        public string DisplayName => Method;

        /// <summary>
        ///     The notes.
        /// </summary>
        public string Notes { get; set; } = string.Empty;
    }
}