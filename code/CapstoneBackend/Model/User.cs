namespace CapstoneBackend.Model
{
    /// <summary>
    /// User Model Class
    /// </summary>
    public class User
    {
        /// <summary>
        /// The id of the user.
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// The username of the user
        /// </summary>
        public string Username { get; set; }
        
        /// <summary>
        /// The password of the user.
        /// </summary>
        public string Password { get; set; }
        
        /// <summary>
        /// The first name of the user.
        /// </summary>
        public string FirstName { get; set; }
        
        /// <summary>
        /// The last name of the user
        /// </summary>
        public string LastName { get; set; }

    }
}