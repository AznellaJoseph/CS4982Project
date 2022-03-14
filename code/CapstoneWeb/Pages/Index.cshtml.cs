using System;
using System.Collections.Generic;
using System.Linq;
using CapstoneBackend.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CapstoneWeb.Pages
{
    /// <summary>
    ///     PageModel for Index Site
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel" />
    public class IndexModel : PageModel
    {
        /// <summary>
        ///     The user id.
        /// </summary>
        public int UserId { get; private set; }

        /// <summary>
        ///     The trips.
        /// </summary>
        public IList<Trip> Trips { get; private set; }

        /// <summary>
        ///     The fake trip manager used for testing.
        /// </summary>
        public TripManager TripManager { get; set; } = new();

        /// <summary>
        ///     Called when [get].
        /// </summary>
        /// <returns>Current page with trips or login if the user is not logged in. </returns>
        public IActionResult OnGet()
        {
            if (!HttpContext.Session.Keys.Contains("userId")) return RedirectToPage("Login");
            UserId = Convert.ToInt32(HttpContext.Session.GetString("userId"));
            Trips = TripManager.GetTripsByUser(UserId).Data;
            return Page();
        }

        /// <summary>
        ///     Called when [post create].
        /// </summary>
        /// <returns> Redirect to create trip form </returns>
        public IActionResult OnPostCreate()
        {
            return RedirectToPage("CreateTrip");
        }

        /// <summary>
        ///     Called when [post logout].
        /// </summary>
        /// <returns> Redirect to index </returns>
        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Remove("userId");
            return RedirectToPage("Index");
        }
    }
}