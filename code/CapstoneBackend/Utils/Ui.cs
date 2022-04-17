namespace CapstoneBackend.Utils
{
    /// <summary>
    ///     UI Class that holds resources that are used by different areas of the project
    /// </summary>
    public class Ui
    {
        /// <summary>
        ///     StatusCode enum for Request status codes
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
        ///     Error Messages Class to hold possible error messages for the solution
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

            public static readonly string InvalidEventDate =
                "You must enter a start date. The end date will default to the trip end date if end date and time are not entered.";

            public static readonly string EmptyLocation = "You must enter a location.";
            public static readonly string EmptyTransportationMethod = "You must enter a method for the transportation.";
            public static readonly string InvalidUsername = "You must enter a username.";
            public static readonly string InvalidPassword = "You must enter a password.";
            public static readonly string InvalidFirstName = "You must enter a first name.";
            public static readonly string InvalidLastName = "You must enter a last name.";
            public static readonly string InvalidConfirmedPassword = "You must confirm your password.";
            public static readonly string InvalidLocation = "You must enter a valid location.";
            public static readonly string TransportationNotFound = "A transportation with the given ID was not found.";
            public static readonly string EventStartDateBeforeTripStartDate = "The event start date cannot be before ";
            public static readonly string EventStartDateAfterTripEndDate = "The event start date cannot be after ";
            public static readonly string EventEndDateAfterTripEndDate = "The event end date cannot be after ";
            public static readonly string EventEndDateBeforeTripStartDate = "The event end date cannot be before ";

            public static readonly string ClashingEventDates =
                "There is already an event from";

            public static readonly string ClashingTripDates = "There is already a trip from";
            public static readonly string LodgingNotFound = "A lodging with the given ID was not found.";
        }
    }
}