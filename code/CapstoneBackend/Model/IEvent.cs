using System;

namespace CapstoneBackend.Model
{
    /// <summary>
    ///     IEvent Interface to encapsulate the events (waypoints, transportation) in a trip
    /// </summary>
    public interface IEvent : IComparable<IEvent>, IEquatable<IEvent>
    {
        /// <summary>
        ///     The event type.
        /// </summary>
        public string EventType { get; }

        /// <summary>
        ///     The id.
        /// </summary>
        public int Id { get; }

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
        public DateTime EndDate { get; set; }

        /// <summary>
        ///     The display name.
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        ///     The full name.
        /// </summary>
        public string FullName { get; }

        /// <summary>
        ///     The notes.
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        ///     Compares the current instance with another object of the same type and returns an integer that indicates whether
        ///     the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>
        ///     A value that indicates the relative order of the objects being compared. The return value has these meanings:
        /// </returns>
        int IComparable<IEvent>.CompareTo(IEvent? other)
        {
            return StartDate.CompareTo(other?.StartDate);
        }
    }
}