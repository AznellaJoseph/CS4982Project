namespace CapstoneBackend.Utils
{
    /// <summary>
    /// UI Class
    /// </summary>
    public class Ui
    {
        /// <summary>
        /// StatusCode enum for Request status codes
        /// </summary>
        public enum StatusCode
        {
            Success = 200,
            BadRequest = 400,
            UnauthorizedAccess = 401,
            DataNotFound = 404,
            InternalServerError = 500
        }

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
            public static readonly string NullTripDate = "You must enter a start and end date.";
            public static readonly string NullWaypointStartDate =
                "You must enter a start date. The end date will default to the trip end date if end date and time are not entered.";
            public static readonly string EmptyWaypointLocation = "You must enter a location for the waypoint.";
            public static readonly string InvalidWaypointDate =
                "Waypoint dates cannot be before the trip start date or after the trip end date.";
            public static readonly string InvalidFields = "You must enter all of the fields.";
        }
    }
}