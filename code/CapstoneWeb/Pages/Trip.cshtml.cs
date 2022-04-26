using System;
using System.Collections.Generic;
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
    ///     PageModel for Trip Site
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
        ///     The lodgings.
        /// </summary>
        public IList<Lodging> Lodgings { get; private set; }

        /// <summary>
        ///     The trip manager.
        /// </summary>
        public TripManager TripManager { get; set; } = new();

        /// <summary>
        ///     The event manager.
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
        /// <param name="tripId">The trip identifier.</param>
        /// <returns>
        ///     Redirect to index if the user is not logged in or the current trip display
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
            Lodgings = LodgingManager.GetLodgingsByTripId(CurrentTrip.TripId).Data;
            return Page();
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
        ///     Called when [post create waypoint].
        /// </summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <returns>
        ///     Redirect to create waypoint form
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
        ///     Called when [post create transportation].
        /// </summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <returns>
        ///     Redirect to create transportation form
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
        ///     Called when [post remove].
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///     The json result of removing the waypoint specified by the id
        /// </returns>
        public IActionResult OnGetRemoveWaypoint(int id)
        {
            return new JsonResult(WaypointManager.RemoveWaypoint(id));
        }

        /// <summary>
        ///     Called when [get remove transportation].
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///     The json result of removing the transportation specified by the id
        /// </returns>
        public IActionResult OnGetRemoveTransportation(int id)
        {
            return new JsonResult(TransportationManager.RemoveTransportation(id));
        }

        /// <summary>
        ///     Called when [post remove lodging].
        /// </summary>
        /// <param name="id">The id for the lodging record to remove</param>
        /// <param name="tripId">The trip identifier.</param>
        /// <returns>Redirect to trip with removed lodging</returns>
        public IActionResult OnPostRemoveLodging(int id, int tripId)
        {
            LodgingManager.RemoveLodging(id);

            var routeValue = new RouteValueDictionary {{"tripId", tripId}};
            return RedirectToPage("Trip", routeValue);
        }

        /// <summary>
        ///     Called when [get view waypoint].
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="tripId">The trip identifier.</param>
        /// <returns>
        ///     Redirect to waypoint
        /// </returns>
        public IActionResult OnGetViewWaypoint(int tripId, int id)
        {
            var routeValues = new RouteValueDictionary
            {
                {"id", id},
                {"tripId", tripId}
            };
            return RedirectToPage("Waypoint", routeValues);
        }

        /// <summary>
        ///     Called when [get view transportation].
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="tripId">The trip identifier.</param>
        /// <returns>
        ///     Redirect to transportation
        /// </returns>
        public IActionResult OnGetViewTransportation(int id, int tripId)
        {
            var routeValues = new RouteValueDictionary
            {
                {"id", id},
                {"tripId", tripId}
            };
            return RedirectToPage("Transportation", routeValues);
        }

        /// <summary>
        ///     Called when [get edit waypoint].
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="tripId">The trip identifier.</param>
        /// <returns>
        ///     Redirect to waypoint
        /// </returns>
        public IActionResult OnGetEditWaypoint(int tripId, int id)
        {
            var routeValues = new RouteValueDictionary
            {
                {"id", id},
                {"tripId", tripId}
            };
            return RedirectToPage("Waypoint", routeValues);
        }

        /// <summary>
        ///     Called when [get edit transportation].
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="tripId">The trip identifier.</param>
        /// <returns>
        ///     Redirect to transportation
        /// </returns>
        public IActionResult OnGetEditTransportation(int id, int tripId)
        {
            var routeValues = new RouteValueDictionary
            {
                {"id", id},
                {"tripId", tripId}
            };
            return RedirectToPage("Transportation", routeValues);
        }

    }
}