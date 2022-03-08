using System;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;

namespace CapstoneWeb.Pages
{
    /// <summary>
    ///     PageModel for Create Waypoint Site
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel" />
    public class CreateWaypointModel : PageModel
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
        public WaypointManager WaypointManager { get; set; }

        /// <summary>
        ///     The event manager.
        /// </summary>
        public EventManager EventManager { get; set; } = new();

        /// <summary>
        ///     Called when [post].
        /// </summary>
        /// <returns>The redirection to the next page or the current page if there was an error </returns>
        public IActionResult OnPost(int tripId)
        {
            var clashingEvent = EventManager.FindClashingEvent(tripId, StartDate, EndDate).Data;
            if (clashingEvent is not null)
            {
                ErrorMessage = $"{Ui.ErrorMessages.ClashingEventDates} {clashingEvent.StartDate} to {clashingEvent.EndDate}.";
                return Page();
            }
            var waypointManager = WaypointManager ?? new WaypointManager();
            var response = waypointManager.CreateWaypoint(tripId, Location, StartDate, EndDate, Notes);
            if (response.StatusCode.Equals((uint)Ui.StatusCode.Success))
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
        /// <param name="tripId">The trip identifier to add a waypoint to.</param>
        /// <returns>Redirects to the trip overview page for the trip the waypoint was added to </returns>
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