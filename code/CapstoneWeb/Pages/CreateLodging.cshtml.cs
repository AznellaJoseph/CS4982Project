using System;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;

namespace CapstoneWeb.Pages
{
    /// <summary>
    ///     PageModel for Create Lodging Site.
    /// </summary>
    public class CreateLodgingModel : PageModel
    {
        /// <summary>
        ///     The error message.
        /// </summary>
        [BindProperty]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     The location.
        /// </summary>
        [BindProperty]
        public string Location { get; set; }

        /// <summary>
        ///     The start date.
        /// </summary>
        [BindProperty]
        public DateTime StartDate { get; set; } = DateTime.Now;

        /// <summary>
        ///     The end date.
        /// </summary>
        [BindProperty]
        public DateTime EndDate { get; set; } = DateTime.Now;

        /// <summary>
        ///     The notes.
        /// </summary>
        [BindProperty]
        public string Notes { get; set; }

        /// <summary>
        ///     The lodging manager.
        /// </summary>
        public LodgingManager LodgingManager { get; set; } = new();

        /// <summary>
        ///     The validation manager.
        /// </summary>
        public ValidationManager ValidationManager { get; set; } = new();

        /// <summary>
        ///     Called when [post].
        /// </summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <returns>
        ///     A redirection to the trip overview page or the current page if there was an error
        /// </returns>
        public IActionResult OnPost(int tripId)
        {
            var validLocationResponse = ValidationManager.DetermineIfValidLocation(Location);

            if (!string.IsNullOrEmpty(validLocationResponse.ErrorMessage))
            {
                ErrorMessage = validLocationResponse.ErrorMessage;
                return Page();
            }

            var validDatesResponse = ValidationManager.DetermineIfValidEventDates(tripId, StartDate, EndDate);

            if (!string.IsNullOrEmpty(validDatesResponse.ErrorMessage))
            {
                ErrorMessage = validDatesResponse.ErrorMessage;
                return Page();
            }

            var response = LodgingManager.CreateLodging(tripId, Location, StartDate, EndDate, Notes);
            if (response.StatusCode.Equals((uint) Ui.StatusCode.Success))
            {
                var routeValue = new RouteValueDictionary
                {
                    {"tripId", tripId}
                };
                return RedirectToPage("Trip", routeValue);
            }

            ErrorMessage = response.ErrorMessage;
            return Page();
        }

        /// <summary>
        ///     Called when [post cancel].
        /// </summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <returns>Redirects to the trip overview page for the trip specified by the trip id</returns>
        public IActionResult OnPostCancel(int tripId)
        {
            var routeValue = new RouteValueDictionary
            {
                {"tripId", tripId}
            };
            return RedirectToPage("Trip", routeValue);
        }
    }
}