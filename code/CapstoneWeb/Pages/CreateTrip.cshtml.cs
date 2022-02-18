using System;
using CapstoneBackend.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CapstoneWeb.Pages
{
    public class CreateTripModel : PageModel
    {
        /// <summary>
        ///     The error message.
        /// </summary>
        [BindProperty]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     The name.
        /// </summary>
        [BindProperty]
        public string Name { get; set; }

        /// <summary>
        ///     The start date.
        /// </summary>
        [BindProperty]
        public DateTime StartDate { get; set; } = DateTime.Today;

        /// <summary>
        ///     The end date.
        /// </summary>
        [BindProperty]
        public DateTime EndDate { get; set; } = DateTime.Today;

        /// <summary>
        ///     The notes.
        /// </summary>
        [BindProperty]
        public string Notes { get; set; }

        /// <summary>
        ///     The trip manager.
        /// </summary>
        public TripManager TripManager { get; set; }

        /// <summary>
        ///     Called when [post].
        /// </summary>
        /// <returns>The next page to redirect to or the current page if there is an error </returns>
        public IActionResult OnPost()
        {
            var tripManager = TripManager ?? new TripManager();
            var response = tripManager.CreateTrip(Convert.ToInt32(HttpContext.Session.GetString("userId")), Name, Notes,
                StartDate, EndDate);
            if (string.IsNullOrEmpty(response.ErrorMessage))
            {
                HttpContext.Session.SetString("tripId", $"{response.Data}");
                return RedirectToPage("index");
            }

            ErrorMessage = response.ErrorMessage;
            return Page();
        }
    }
}