using System.Collections.Generic;

namespace CapstoneBackend.Model
{
    /// <summary>
    ///     User Model Class
    /// </summary>
    public class User
    {
        private readonly TripManager _tripManager = new TripManager();
        
        /// <summary>
        ///     The id of the user.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     The username of the user
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        ///     The password of the user.
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        ///     The first name of the user.
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        ///     The last name of the user
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        public IList<Trip> Trips => this.getTrips();

        private IList<Trip> getTrips()
        {
            var response = this._tripManager.GetTripsByUser(this.Id);
            return response.StatusCode == 200 && response.Data is not null ? response.Data : new List<Trip>();
        }
    }
}