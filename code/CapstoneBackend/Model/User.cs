namespace CapstoneBackend.Model
{
    /// <summary>
    ///     A model class for the User object
    /// </summary>
    public class User
    {
        /// <summary>
        ///     The user id.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        ///     The username.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        ///     The password.
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        ///     The first name.
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        ///     The last name.
        /// </summary>
        public string LastName { get; set; } = string.Empty;
    }
}