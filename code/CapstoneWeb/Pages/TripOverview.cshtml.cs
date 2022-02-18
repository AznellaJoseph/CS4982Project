using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CapstoneBackend.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CapstoneWeb.Pages
{
    public class TripOverviewModel : PageModel
    {

        public Trip CurrentTrip;

        public TripManager FakeTripManager;

        /// <summary>
        /// Called when [get].
        /// </summary>
        /// <returns>
        /// Redirects to Index when ID passed in query params
        /// Should redirect to 404 page when Trip not found
        /// Returns full Trip Overview Page otherwise
        /// </returns>
        public IActionResult OnGet()
        {
            int? tripId = GetTripIdFromQuery();

            if (tripId is null)
            {
                return RedirectToPage("Index");
            }

            var tripManager = FakeTripManager ?? new TripManager();
            var response = tripManager.GetTripByTripId((int)tripId);

            if (response.StatusCode.Equals(200))
            {
                CurrentTrip = response.Data;
                return Page();
            }
            else
            {
                return RedirectToPage("Index");
            }
        }

        private int? GetTripIdFromQuery()
        {
            int result;

            try
            {
                string query = HttpContext.Request.Query["id"][0];
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
