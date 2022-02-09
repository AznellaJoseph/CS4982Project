using System;

namespace CapstoneBackend.Model
{
    /// <summary>
    /// A model class for the Trip object
    /// </summary>
    public class Trip
    {
        /// <summary>
        /// The Id of the Trip
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// The Id of User of The trip
        /// </summary>
        public int UserId { get; set; }
        
        /// <summary>
        /// The name of the trip
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The start date of the  trip
        /// </summary>
        public DateTime StartDate { get; set; }
        
        /// <summary>
        /// The end date of the trip
        /// </summary>
        public DateTime EndDate { get; set; }
    }
}