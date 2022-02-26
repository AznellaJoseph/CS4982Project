using System;
using System.Linq;
using CapstoneBackend.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
        public IActionResult OnGet(int? tripId)
        {
            
            if (!HttpContext.Session.Keys.Contains("userId")) 
                return RedirectToPage("Index");

            Console.WriteLine("USER ID");
            
            UserId = Convert.ToInt32(HttpContext.Session.GetString("userId"));
            if (tripId is null)
                return RedirectToPage("Index");
            
            Console.WriteLine("TRIP ID");
            
            var tripManager = FakeTripManager ?? new TripManager();
            var response = tripManager.GetTripByTripId((int) tripId);
            
            if (response.StatusCode.Equals(200) && TripBelongsToUser(response.Data))
            {
                CurrentTrip = response.Data;
                return Page();
            }

            return RedirectToPage("Index");
        }

        public IActionResult OnGetAjax(int tripId, string selectedDate)
        {
            Console.WriteLine(selectedDate);
            var manager = FakeWaypointManager ?? new WaypointManager();
            return new JsonResult(manager.GetWaypointsOnDate(tripId,DateTime.Parse(selectedDate)));
        }

        private bool TripBelongsToUser(Trip trip)
        {
            return trip.UserId == UserId;
        }
    }
}