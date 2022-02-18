using System;
using CapstoneBackend.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CapstoneWeb.Pages
{
    public class CreateWaypointModel : PageModel
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
        ///     The waypoint manager.
        /// </summary>
        public WaypointManager WaypointManager { get; set; }

        /// <summary>
        ///     Called when [post].
        /// </summary>
        /// <returns>The redirection to the next page or the current page if there was an error </returns>
        public IActionResult OnPost()
        {
            var waypointManager = WaypointManager ?? new WaypointManager();
            var response = waypointManager.CreateWaypoint(Convert.ToInt32(HttpContext.Session.GetString("tripId")),
                Location, StartDate, EndDate, Notes);
            if (string.IsNullOrEmpty(response.ErrorMessage))
            {
                HttpContext.Session.SetString("waypointId", $"{response.Data}");
                return RedirectToPage("index");
            }

            ErrorMessage = response.ErrorMessage;
            return Page();
        }
    }
}