using System;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CapstoneWeb.Pages
{
    /// <summary>
    ///     PageModel for Create Trip Site
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel" />
    public class CreateTripModel : PageModel
    {
        /// <summary>
        ///     The error message.
        /// </summary>
        [BindProperty]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     The trip name.
        /// </summary>
        [BindProperty]
        public string TripName { get; set; }

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
        public TripManager TripManager { get; set; } = new();

        /// <summary>
        ///     The validation manager.
        /// </summary>
        public ValidationManager ValidationManager { get; set; } = new();

        /// <summary>
        ///     Called when [post].
        /// </summary>
        /// <returns>Redirect to index or the current page if there is an error </returns>
        public IActionResult OnPost()
        {

            var userId = Convert.ToInt32(HttpContext.Session.GetString("userId"));


            var clashingTripResponse = ValidationManager.FindClashingTrip(userId, StartDate, EndDate);
            if (!string.IsNullOrEmpty(clashingTripResponse.ErrorMessage))
            {
                ErrorMessage = clashingTripResponse.ErrorMessage;
                return Page();
            }
            var response = TripManager.CreateTrip(userId, TripName,
                Notes,
                StartDate, EndDate);
            if (string.IsNullOrEmpty(response.ErrorMessage))
            {
                HttpContext.Session.SetString("tripId", $"{response.Data}");
                return RedirectToPage("index");
            }

            ErrorMessage = response.ErrorMessage;
            return Page();
        }

        /// <summary>
        ///     Called when [post cancel].
        /// </summary>
        /// <returns>Redirects to landing page.</returns>
        public IActionResult OnPostCancel()
        {
            return RedirectToPage("index");
        }
    }
}