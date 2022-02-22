namespace CapstoneBackend.Utils
{

    /// <summary>
    /// Error Messages Class to hold possible error messages for the entire solution
    /// </summary>
    public static class ErrorMessages
    {
        public static readonly string InternalServerError = "Internal Server Error.";
        public static readonly string TripNotFound = "A trip with the given ID was not found.";
        public static readonly string InvalidStartDate = "Start date cannot be after the end date.";
        public static readonly string IncorrectUsername = "Username is incorrect.";
        public static readonly string IncorrectPassword = "Password is incorrect.";
        public static readonly string UsernameTaken = "Username is taken.";
        public static readonly string WaypointNotFound = "A waypoint with the given ID was not found.";
        public static readonly string PasswordsDoNotMatch = "The passwords must match.";
        public static readonly string UnknownError = "Unknown Error.";
        public static readonly string EmptyTripName = "You must enter a name for the trip.";
        public static readonly string NullDate = "You must enter a start and end date.";
        public static readonly string EmptyWaypointLocation = "You must enter a location for the waypoint.";
    }
}