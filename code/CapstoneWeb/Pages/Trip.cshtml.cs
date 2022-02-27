using System;
using System.Linq;
using CapstoneBackend.Model;
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
        public int UserId { get; private set; }
        public Trip CurrentTrip { get; private set; }
        public TripManager FakeTripManager { get; set; }
        public WaypointManager FakeWaypointManager { get; set; }

        /// <summary>
        ///     Called when [get].
        /// </summary>
        /// <returns>
        ///     Redirects to Index when ID passed in query params
        ///     Should redirect to 404 page when Trip not found
        ///     Returns full Trip Overview Page otherwise
        /// </returns>
        public IActionResult OnGet(int tripId)
        {
            
            if (!HttpContext.Session.Keys.Contains("userId")) 
                return RedirectToPage("Index");

            UserId = Convert.ToInt32(HttpContext.Session.GetString("userId"));
            var tripManager = FakeTripManager ?? new TripManager();
            var response = tripManager.GetTripByTripId(tripId);
            
            if (response.StatusCode.Equals(200) && response.Data?.UserId == UserId)
            {
                CurrentTrip = response.Data;
                return Page();
            }

            return RedirectToPage("Index");
        }

        public IActionResult OnGetAjax(int tripId, string selectedDate)
        {
            var manager = FakeWaypointManager ?? new WaypointManager();
            return new JsonResult(manager.GetWaypointsOnDate(tripId,DateTime.Parse(selectedDate)));
        }
        
        /// <summary>
        ///     Called when [post logout].
        /// </summary>
        /// <returns> The action to take when logging out </returns>
        public IActionResult OnPostCreate(int tripId)
        {
            var routeValue = new RouteValueDictionary
            {
                {"tripId", tripId}
            };
            return RedirectToPage("CreateWaypoint", routeValue);
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
        
    }
}