using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapstoneBackend.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CapstoneWeb.Pages
{
    public class LodgingModel : PageModel
    {
        public Lodging CurrentLodging { get; private set; }

        public LodgingManager LodgingManager { get; set; } = new();

        /// <summary>
        /// Called when [get].
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="tripId">The trip identifier.</param>
        /// <returns>Redirect to index if the user is not logged in or the current lodging display</returns>
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
    }
}
