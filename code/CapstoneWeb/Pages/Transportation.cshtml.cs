using System.Linq;
using CapstoneBackend.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;

namespace CapstoneWeb.Pages
{
    /// <summary>
    ///     PageModel for Transportation Site
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel" />
    public class TransportationModel : PageModel
    {
        /// <summary>
        ///     The current transportation.
        /// </summary>
        public Transportation CurrentTransportation { get; private set; }

        /// <summary>
        ///     The transportation manager.
        /// </summary>
        public TransportationManager TransportationManager { get; set; } = new();

        /// <summary>
        ///     Called when [get].
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="tripId">The trip identifier.</param>
        /// <returns>
        ///     Redirect to index if the user is not logged in, trip if the selected event does not exist, or the current
        ///     transportation display
        /// </returns>
        public IActionResult OnGet(int id, int tripId)
        {
            if (!HttpContext.Session.Keys.Contains("userId"))
                return RedirectToPage("Index");

            var transportationResponse = TransportationManager.GetTransportationById(id);

            if (transportationResponse.Data is null) return RedirectToPage("Trip", tripId);

            if (transportationResponse.Data.TripId != tripId) return RedirectToPage("Trip", tripId);

            CurrentTransportation = transportationResponse.Data;
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