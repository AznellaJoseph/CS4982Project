using System;
using System.Linq;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;

namespace CapstoneWeb.Pages
{
    /// <summary>
    ///     PageModel for Edit Waypoint Site
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel" />
    public class EditWaypointModel : PageModel
    {
        /// <summary>
        ///     The error message.
        /// </summary>
        [BindProperty]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     The location.
        /// </summary>
        [BindProperty]
        public string Location { get; set; }

        /// <summary>
        ///     The start date.
        /// </summary>
        [BindProperty]
        public DateTime StartDate { get; set; } = DateTime.Now;

        /// <summary>
        ///     The end date.
        /// </summary>
        [BindProperty]
        public DateTime EndDate { get; set; } = DateTime.Now;

        /// <summary>
        ///     The notes.
        /// </summary>
        [BindProperty]
        public string Notes { get; set; }

        /// <summary>
        ///     The waypoint manager.
        /// </summary>
        public WaypointManager WaypointManager { get; set; } = new();

        /// <summary>
        ///     The validation manager.
        /// </summary>
        public ValidationManager ValidationManager { get; set; } = new();

        /// <summary>
        ///     Called when [get].
        /// </summary>
        /// <param name="id">The identifier of the waypoint to edit.</param>
        /// <param name="tripId">The trip identifier.</param>
        /// <returns>
        ///     Redirect to index if the user is not logged in, trip if the selected event does not exist, or the current
        ///     waypoint display
        /// </returns>
        public IActionResult OnGet(int id, int tripId)
        {
            if (!HttpContext.Session.Keys.Contains("userId"))
                return RedirectToPage("Index");

            var waypointResponse = WaypointManager.GetWaypointById(id);

            if (waypointResponse.Data is null) return RedirectToPage("Trip", tripId);

            if (waypointResponse.Data.TripId != tripId) return RedirectToPage("Trip", tripId);

            var waypoint = waypointResponse.Data;
            Location = waypoint.Location;
            StartDate = waypoint.StartDate;
            EndDate = waypoint.EndDate;
            Notes = waypoint.Notes;

            return Page();
        }

        /// <summary>
        ///     Called when [post].
        /// </summary>
        /// <param name="id">The identifier for the waypoint.</param>
        /// <param name="tripId">The trip identifier.</param>
        /// <returns>
        ///     Redirect to the trip overview page or the current page if there was an error
        /// </returns>
        public IActionResult OnPost(int id, int tripId)
        {
            var validLocationResponse = ValidationManager.DetermineIfValidLocation(Location);

            if (!string.IsNullOrEmpty(validLocationResponse.ErrorMessage))
            {
                ErrorMessage = validLocationResponse.ErrorMessage;
                return Page();
            }

            var validDatesResponse = ValidationManager.DetermineIfValidEventDates(tripId, StartDate, EndDate);

            if (!string.IsNullOrEmpty(validDatesResponse.ErrorMessage))
            {
                ErrorMessage = validDatesResponse.ErrorMessage;
                return Page();
            }

            var updatedWaypoint = new Waypoint
            {
                TripId = tripId,
                WaypointId = id,
                Location = Location,
                StartDate = StartDate,
                EndDate = EndDate,
                Notes = Notes
            };

            var clashingEventResponse =
                ValidationManager.FindClashingEvents(tripId, StartDate, EndDate, updatedWaypoint);

            if (!string.IsNullOrEmpty(clashingEventResponse.ErrorMessage))
            {
                ErrorMessage = clashingEventResponse.ErrorMessage;
                return Page();
            }

            var response = WaypointManager.EditWaypoint(updatedWaypoint);
            if (response.StatusCode.Equals((uint) Ui.StatusCode.Success))
            {
                var routeValue = new RouteValueDictionary
                {
                    {"tripId", tripId}
                };
                return RedirectToPage("Trip", routeValue);
            }

            ErrorMessage = response.ErrorMessage;
            return Page();
        }

        /// <summary>
        ///     Called when [post cancel].
        /// </summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <returns>Redirects to the trip overview page for the trip specified by the trip id </returns>
        public IActionResult OnPostCancel(int tripId)
        {
            var routeValue = new RouteValueDictionary
            {
                {"tripId", tripId}
            };
            return RedirectToPage("Trip", routeValue);
        }
    }
}