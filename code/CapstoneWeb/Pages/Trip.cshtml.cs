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
    ///     TripOverview Model
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
        ///     The fake trip manager.
        /// </summary>
        public TripManager FakeTripManager { get; set; }
        
        /// <summary>
        ///     The fake waypoint manager.
        /// </summary>
        public EventManager FakeEventManager { get; set; }

        /// <summary>
        ///     The fake waypoint manager.
        /// </summary>
        public WaypointManager FakeWaypointManager { get; set; }
        
        /// <summary>
        ///     The fake transportation manager.
        /// </summary>
        public TransportationManager FakeTransportationManager { get; set; }

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
            var tripManager = FakeTripManager ?? new TripManager();
            var response = tripManager.GetTripByTripId(tripId);

            if (!response.StatusCode.Equals((uint) Ui.StatusCode.Success) || response.Data?.UserId != UserId)
                return RedirectToPage("Index");
            CurrentTrip = response.Data;
            return Page();
        }

        /// <summary>
        ///     Called when [get ajax].
        /// </summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <param name="selectedDate">The selected date.</param>
        /// <returns></returns>
        public IActionResult OnGetEvents(int tripId, string selectedDate)
        {
            var manager = FakeEventManager ?? new EventManager();
            return new JsonResult(manager.GetEventsOnDate(tripId, DateTime.Parse(selectedDate)));
        }

        /// <summary>
        ///     Called when [post create].
        /// </summary>
        /// <returns> The action to take when going to create waypoint form </returns>
        public IActionResult OnPostCreateWaypoint(int tripId)
        {
            var routeValue = new RouteValueDictionary
            {
                {"tripId", tripId}
            };
            return RedirectToPage("CreateWaypoint", routeValue);
        }

        /// <summary>
        ///     Called when [post create].
        /// </summary>
        /// <returns> The action to take when going to create Transportation form </returns>
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
        /// <returns> The action to take when going back to trips page </returns>
        public IActionResult OnPostBack()
        {
            return RedirectToPage("Index");
        }

        /// <summary>
        ///     Called when [post logout].
        /// </summary>
        /// <returns> The action to take when logging out </returns>
        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Remove("userId");
            return RedirectToPage("Index");
        }

        /// <summary>
        ///     Called when [post remove].
        /// </summary>
        /// <returns></returns>
        public IActionResult OnGetRemoveWaypoint(int tripId, int id)
        {
            var manager = FakeWaypointManager ?? new WaypointManager();
            return new JsonResult(manager.RemoveWaypoint(id));
        }
        public IActionResult OnGetRemoveTransportation(int tripId, int id)
        {
            var manager = FakeTransportationManager ?? new TransportationManager();
            return new JsonResult(manager.RemoveTransportation(id));
        }
        
    }
}