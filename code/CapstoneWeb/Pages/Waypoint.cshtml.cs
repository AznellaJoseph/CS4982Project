using System.Linq;
using CapstoneBackend.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;

namespace CapstoneWeb.Pages
{
    /// <summary>
    ///     PageModel for Waypoint Site
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel" />
    public class WaypointModel : PageModel
    {
        /// <summary>
        ///     The current waypoint.
        /// </summary>
        public Waypoint CurrentWaypoint { get; private set; }

        /// <summary>
        ///     The waypoint manager.
        /// </summary>
        public WaypointManager WaypointManager { get; set; } = new();

        /// <summary>
        ///     Called when [get].
        /// </summary>
        /// <param name="id">The identifier.</param>
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

            CurrentWaypoint = waypointResponse.Data;
            return Page();
        }

        /// <summary>
        ///     Called when [post logout].
        /// </summary>
        /// <returns>Redirect to index</returns>
        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Remove("userId");
            return RedirectToPage("Index");
        }

        /// <summary>
        ///     Called when [post back].
        /// </summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <returns>Redirect to trip</returns>
        public IActionResult OnPostBack(int tripId)
        {
            var routeValue = new RouteValueDictionary
            {
                {"tripId", tripId}
            };
            return RedirectToPage("Trip", routeValue);
        }
    }
}