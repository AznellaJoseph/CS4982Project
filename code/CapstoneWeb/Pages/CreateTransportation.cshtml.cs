using System;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;

namespace CapstoneWeb.Pages
{
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
        ///     The transportation manager used for testing.
        /// </summary>
        public TransportationManager FakeTransportationManager { get; set; }

        /// <summary>
        ///     Called when [post].
        /// </summary>
        /// <returns>The redirection to the next page or the current page if there was an error </returns>
        public IActionResult OnPost(int tripId)
        {
            var transportationManager = FakeTransportationManager ?? new TransportationManager();
            var response = transportationManager.CreateTransportation(tripId, Method, StartDate, EndDate);
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
    }
}