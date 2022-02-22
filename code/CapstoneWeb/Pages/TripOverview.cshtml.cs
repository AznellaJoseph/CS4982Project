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
    public class TripOverviewModel : PageModel
    {
        public int UserId { get; set; }

        public Trip CurrentTrip { get; set; }

        public TripManager FakeTripManager { get; set; }

        /// <summary>
        ///     Called when [get].
        /// </summary>
        /// <returns>
        ///     Redirects to Index when ID passed in query params
        ///     Should redirect to 404 page when Trip not found
        ///     Returns full Trip Overview Page otherwise
        /// </returns>
        public IActionResult OnGet()
        {
            if (!HttpContext.Session.Keys.Contains("userId")) 
                return RedirectToPage("Index");

            UserId = Convert.ToInt32(HttpContext.Session.GetString("userId"));
            var tripId = GetTripIdFromQuery();

            if (tripId is null) 
                return RedirectToPage("Index");

            var tripManager = FakeTripManager ?? new TripManager();
            var response = tripManager.GetTripByTripId((int) tripId);

            if (response.StatusCode.Equals(200) && TripBelongsToUser(response.Data))
            {
                CurrentTrip = response.Data;
                return Page();
            }

            return RedirectToPage("Index");
        }

        private bool TripBelongsToUser(Trip trip)
        {
            return trip.UserId == UserId;
        }

        private int? GetTripIdFromQuery()
        {
            int result;

            try
            {
                var query = HttpContext.Request.Query["id"][0];
                result = Convert.ToInt32(query);
            }
            catch
            {
                return null;
            }

            return result;
        }
    }
}