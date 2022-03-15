using System;
using System.Linq;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;

namespace CapstoneWeb.Pages
{
    /// <summary>
    ///     PageModel for Single Trip Overview
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel" />
    public class TripModel : PageModel
    {
        /// <summary>
        ///     The user id.
        /// </summary>
        public int UserId { get; private set; }

        /// <summary>
        ///     The current trip.
        /// </summary>
        public Trip CurrentTrip { get; private set; }

        /// <summary>
        ///     The trip manager.
        /// </summary>
        public TripManager TripManager { get; set; } = new();

        /// <summary>
        ///     The waypoint manager.
        /// </summary>
        public EventManager EventManager { get; set; } = new();

        /// <summary>
        ///     The waypoint manager.
        /// </summary>
        public WaypointManager WaypointManager { get; set; } = new();

        /// <summary>
        ///     The transportation manager.
        /// </summary>
        public TransportationManager TransportationManager { get; set; } = new();

        /// <summary>
        ///     The lodging manager.
        /// </summary>
        public LodgingManager LodgingManager { get; set; } = new();

        /// <summary>
        ///     Called when [get].
        /// </summary>
        /// <returns>
        ///     Redirect to index or current page if there is an error
        /// </returns>
        public IActionResult OnGet(int tripId)
        {
            if (!HttpContext.Session.Keys.Contains("userId"))
                return RedirectToPage("Index");

            UserId = Convert.ToInt32(HttpContext.Session.GetString("userId"));
            var response = TripManager.GetTripByTripId(tripId);

            if (!response.StatusCode.Equals((uint) Ui.StatusCode.Success) || response.Data?.UserId != UserId)
                return RedirectToPage("Index");
            CurrentTrip = response.Data;
            return Page();
        }

        /// <summary>
        ///     Called when [get lodging].
        /// </summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="selectedDate">The selected date.</param>
        /// <returns>A JSON response containing the lodging data for the specified date</returns>
        public IActionResult OnGetLodging(int tripId, string selectedDate)
        {
            return new JsonResult(LodgingManager.GetLodgingsOnDate(tripId, DateTime.Parse(selectedDate)));
        }

        /// <summary>
        ///     Called when [get events].
        /// </summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="selectedDate">The selected date.</param>
        /// <returns> A json response of the events on the selected date.</returns>
        public IActionResult OnGetEvents(int tripId, string selectedDate)
        {
            return new JsonResult(EventManager.GetEventsOnDate(tripId, DateTime.Parse(selectedDate)));
        }

        /// <summary>
        ///     Called when [post create lodging].
        /// </summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <returns>
        ///     Redirect to create lodging form.
        /// </returns>
        public IActionResult OnPostCreateLodging(int tripId)
        {
            var routeValue = new RouteValueDictionary
            {
                {"tripId", tripId}
            };
            return RedirectToPage("CreateLodging", routeValue);
        }

        /// <summary>
        /// Called when [post create waypoint].
        /// </summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <returns>
        /// Redirect to create waypoint form
        /// </returns>
        public IActionResult OnPostCreateWaypoint(int tripId)
        {
            var routeValue = new RouteValueDictionary
            {
                {"tripId", tripId}
            };
            return RedirectToPage("CreateWaypoint", routeValue);
        }

        /// <summary>
        /// Called when [post create transportation].
        /// </summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <returns>
        /// Redirect to create transportation form
        /// </returns>
        public IActionResult OnPostCreateTransportation(int tripId)
        {
            var routeValue = new RouteValueDictionary
            {
                {"tripId", tripId}
            };
            return RedirectToPage("CreateTransportation", routeValue);
        }

        /// <summary>
        ///     Called when [post back].
        /// </summary>
        /// <returns> Redirect to index </returns>
        public IActionResult OnPostBack()
        {
            return RedirectToPage("Index");
        }

        /// <summary>
        ///     Called when [post logout].
        /// </summary>
        /// <returns> Redirect to index </returns>
        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Remove("userId");
            return RedirectToPage("Index");
        }

        /// <summary>
        /// Called when [post remove].
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// The json result of removing the waypoint specified by the id
        /// </returns>
        public IActionResult OnGetRemoveWaypoint(int id)
        {
            return new JsonResult(WaypointManager.RemoveWaypoint(id));
        }
        /// <summary>
        /// Called when [get remove transportation].
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// The json result of removing the transportation specified by the id
        /// </returns>
        public IActionResult OnGetRemoveTransportation(int id)
        {
            return new JsonResult(TransportationManager.RemoveTransportation(id));
        }

        /// <summary>
        ///     Called when [get remove lodging].
        /// </summary>
        /// <param name="id">The id for the lodging record to remove</param>
        /// <returns>
        ///     A JSON result of removing the lodging
        /// </returns>
        public IActionResult OnGetRemoveLodging(int id)
        {
            return new JsonResult(LodgingManager.RemoveLodging(id));
        }
        
    }
}
