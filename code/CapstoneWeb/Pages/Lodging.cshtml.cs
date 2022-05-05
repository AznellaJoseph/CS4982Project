using System.Linq;
using CapstoneBackend.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;

namespace CapstoneWeb.Pages
{
    /// <summary>
    ///     PageModel for Lodging Site.
    /// </summary>
    public class LodgingModel : PageModel
    {
        /// <summary>
        ///     The current lodging.
        /// </summary>
        public Lodging CurrentLodging { get; private set; }

        /// <summary>
        ///     The lodging manager.
        /// </summary>
        public LodgingManager LodgingManager { get; set; } = new();

        /// <summary>
        ///     Called when [get].
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="tripId">The trip identifier.</param>
        /// <returns>
        ///     Redirect to index if the user is not logged in, trip if the selected event does not exist, or the current
        ///     lodging display
        /// </returns>
        public IActionResult OnGet(int id, int tripId)
        {
            if (!HttpContext.Session.Keys.Contains("userId"))
                return RedirectToPage("Index");

            var lodgingResponse = LodgingManager.GetLodgingById(id);

            if (lodgingResponse.Data is null) return RedirectToPage("Trip", tripId);

            if (lodgingResponse.Data.TripId != tripId) return RedirectToPage("Trip", tripId);

            CurrentLodging = lodgingResponse.Data;
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
        /// <returns>Redirects to the trip overview page for the trip specified by the trip id</returns>
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