using System;

namespace CapstoneBackend.Model
{
    /// <summary>
    ///     A model class for the Waypoint object
    /// </summary>
    public class Waypoint : IEvent
    {
        /// <summary>
        ///     The waypoint id.
        /// </summary>
        public int WaypointId { get; set; }

        /// <summary>
        ///     The location.
        /// </summary>
        public string Location { get; set; } = string.Empty;

        /// <summary>
        ///     The event type.
        /// </summary>
        public string EventType => nameof(Waypoint);

        /// <summary>
        ///     The id.
        /// </summary>
        public int Id => WaypointId;

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
        public DateTime EndDate { get; set; }

        /// <summary>
        ///     The display name.
        /// </summary>
        public string DisplayName => Location[..Location.IndexOf(",", StringComparison.Ordinal)];

        /// <summary>
        ///     The full name.
        /// </summary>
        public string FullName => Location;

        /// <summary>
        ///     Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        ///     True if the current object is equal to the parameter, false otherwise />.
        /// </returns>
        public bool Equals(IEvent? other)
        {
            return other is not null && other.Id.Equals(Id) &&
                   other.EventType.Equals(EventType) &&
                   other.TripId.Equals(TripId);
        }

        /// <summary>
        ///     Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        ///     A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(Id, DisplayName, EventType);
        }
    }
}