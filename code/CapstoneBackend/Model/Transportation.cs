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
        ///     The event type.
        /// </summary>
        public string EventType => nameof(Transportation);

        /// <summary>
        ///     The id.
        /// </summary>
        public int Id => TransportationId;

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
        public string DisplayName => Method;

        /// <summary>
        ///     The full name.
        /// </summary>
        public string FullName => Method;

        /// <summary>
        ///     The notes.
        /// </summary>
        public string Notes { get; set; } = string.Empty;

        /// <summary>
        ///     Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        ///     True of the other object is equal to the current object, false otherwise
        /// </returns>
        public bool Equals(IEvent? other)
        {
            return other is not null && other.Id.Equals(Id) && other.DisplayName.Equals(DisplayName) &&
                   other.EndDate.Equals(EndDate) &&
                   other.EventType.Equals(EventType) && other.Notes.Equals(Notes) &&
                   other.StartDate.Equals(StartDate) && other.TripId.Equals(TripId);
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