using System;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;

namespace CapstoneWeb.Pages
{
    /// <summary>
    ///     PageModel for Create Transportation Site
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel" />
    public class CreateTransportationModel : PageModel
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
        ///     The end date.
        /// </summary>
        [BindProperty]
        public string Notes { get; set; }

        /// <summary>
        ///     The transportation manager used for testing.
        /// </summary>
        public TransportationManager FakeTransportationManager { get; set; }

        /// <summary>
        ///     The event manager.
        /// </summary>
        public EventManager EventManager { get; set; } = new();
        /// <summary>
        ///     Called when [post].
        /// </summary>
        /// <returns>The redirection to the next page or the current page if there was an error </returns>
        public IActionResult OnPost(int tripId)
        {
            var clashingEvent = EventManager.FindClashingEvent(tripId, StartDate, EndDate).Data;
            if (clashingEvent is not null)
            {
                ErrorMessage = $"{Ui.ErrorMessages.ClashingEventDates} {clashingEvent.StartDate} to {clashingEvent.EndDate}.";
                return Page();
            }
            var transportationManager = FakeTransportationManager ?? new TransportationManager();

            var response = transportationManager.CreateTransportation(tripId, Method, StartDate, EndDate, Notes);
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
        /// <returns>Redirects to the trip overview page for this trip ID</returns>
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