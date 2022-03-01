using System;

namespace CapstoneBackend.Model
{
    /// <summary>
    /// IEvent Interface to maintain the events (waypoints, transportation) in a trip
    public interface IEvent : IComparable<IEvent>
    {
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
        
        public string DisplayName { get; }

        int IComparable<IEvent>.CompareTo(IEvent? other)
        {
            return StartDate.CompareTo(other?.StartDate);
        }
    }
}
