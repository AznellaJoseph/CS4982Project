using System;

namespace CapstoneBackend.Model
{
    public interface IEvent
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
    }
}