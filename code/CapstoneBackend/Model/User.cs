namespace CapstoneBackend.Model
{
    /// <summary>
    ///     A model class for the User object
    /// </summary>
    public class User
    {
        /// <summary>
        ///     The id of the user.
        /// </summary>
        public int UserId { get; set; }

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
    }
}