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
    ///     PageModel for Edit Transportation Site
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel" />
    public class EditTransportationModel : PageModel
    {
        /// <summary>
        ///     The error message.
        /// </summary>
        [BindProperty]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     The method.
        /// </summary>
        [BindProperty]
        public string Method { get; set; }

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
        ///     The transportation manager.
        /// </summary>
        public TransportationManager TransportationManager { get; set; } = new();

        /// <summary>
        ///     The validation manager.
        /// </summary>
        public ValidationManager ValidationManager { get; set; } = new();


        /// <summary>
        ///     Called when [get].
        /// </summary>
        /// <param name="id">The identifier of the transportation to edit.</param>
        /// <param name="tripId">The trip identifier.</param>
        /// <returns>
        ///     Redirect to index if the user is not logged in, trip if the selected event does not exist, or the current
        ///     transportation display
        /// </returns>
        public IActionResult OnGet(int id, int tripId)
        {
            if (!HttpContext.Session.Keys.Contains("userId"))
                return RedirectToPage("Index");

            var transportationResponse = TransportationManager.GetTransportationById(id);

            if (transportationResponse.Data is null) return RedirectToPage("Trip", tripId);

            if (transportationResponse.Data.TripId != tripId) return RedirectToPage("Trip", tripId);

            var transportation = transportationResponse.Data;
            Method = transportation.Method;
            StartDate = transportation.StartDate;
            EndDate = transportation.EndDate;
            Notes = transportation.Notes;

            return Page();
        }

        /// <summary>
        ///     Called when [post].
        /// </summary>
        /// <param name="id">The identifier of the transportation to update.</param>
        /// <param name="tripId">The trip identifier.</param>
        /// <returns>Redirect to trip or the current page if there was an error </returns>
        public IActionResult OnPost(int id, int tripId)
        {
            var validDatesResponse = ValidationManager.DetermineIfValidEventDates(tripId, StartDate, EndDate);

            if (!string.IsNullOrEmpty(validDatesResponse.ErrorMessage))
            {
                ErrorMessage = validDatesResponse.ErrorMessage;
                return Page();
            }

            var updatedTransportation = new Transportation
            {
                TransportationId = id,
                TripId = tripId,
                Method = Method,
                StartDate = StartDate,
                EndDate = EndDate,
                Notes = Notes
            };

            var clashingEventResponse =
                ValidationManager.FindClashingEvents(tripId, StartDate, EndDate, updatedTransportation);

            if (!string.IsNullOrEmpty(clashingEventResponse.ErrorMessage))
            {
                ErrorMessage = clashingEventResponse.ErrorMessage;
                return Page();
            }

            var response = TransportationManager.EditTransportation(updatedTransportation);
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
        /// <param name="tripId">The trip identifier to add transportation to.</param>
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