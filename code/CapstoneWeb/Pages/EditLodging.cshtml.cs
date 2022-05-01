using System;
using System.Linq;
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
    public class EditLodgingModel : PageModel
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
        ///     The end date.
        /// </summary>
        [BindProperty]
        public string Notes { get; set; }

        /// <summary>
        ///     The transportation manager.
        /// </summary>
        public LodgingManager LodgingManager { get; set; } = new();

        /// <summary>
        ///     The validation manager.
        /// </summary>
        public ValidationManager ValidationManager { get; set; } = new();


        public IActionResult OnGet(int id, int tripId)
        {
            if (!HttpContext.Session.Keys.Contains("userId"))
                return RedirectToPage("Index");

            var lodgingResponse = LodgingManager.GetLodgingById(id);

            if (lodgingResponse.Data is null) return RedirectToPage("Trip", tripId);
            if (lodgingResponse.Data.TripId != tripId) return RedirectToPage("Trip", tripId);

            var lodging = lodgingResponse.Data;
            Location = lodging.Location;
            StartDate = lodging.StartDate;
            EndDate = lodging.EndDate;
            Notes = lodging.Notes;

            return Page();
        }

        /// <summary>
        ///     Called when [post].
        /// </summary>
        /// <param name="tripId">The trip identifier.</param>
        /// <returns>
        ///     A redirection to the trip overview page or the current page if there was an error
        /// </returns>
        public IActionResult OnPost(int id, int tripId)
        {
            var validDatesResponse = ValidationManager.DetermineIfValidEventDates(tripId, StartDate, EndDate);
            var validLocationResponse = ValidationManager.DetermineIfValidLocation(Location);

            if (!string.IsNullOrEmpty(validLocationResponse.ErrorMessage))
            {
                ErrorMessage = validLocationResponse.ErrorMessage;
                return Page();
            }

            if (!string.IsNullOrEmpty(validDatesResponse.ErrorMessage))
            {
                ErrorMessage = validDatesResponse.ErrorMessage;
                return Page();
            }

            var updatedLodging = new Lodging
            {
                LodgingId = id,
                TripId = tripId,
                StartDate = StartDate,
                EndDate = EndDate,
                Location = Location,
                Notes = Notes
            };

            var response = LodgingManager.EditLodging(updatedLodging);
            if (response.StatusCode.Equals((uint)Ui.StatusCode.Success))
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
