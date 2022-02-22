using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CapstoneBackend.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CapstoneWeb.Pages
{
    /// <summary>
    ///     Index Model
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel" />
    public class IndexModel : PageModel
    {
        /// <summary>
        ///     The user id.
        /// </summary>
        public int UserId { get; private set; }
        public IList<Trip> Trips { get; private set; }
        
        public TripManager FakeTripManager { get; set; }

        /// <summary>
        ///     Called when [get].
        /// </summary>
        /// <returns> The page to go to when [get] </returns>
        public IActionResult OnGet()
        {
            if (!HttpContext.Session.Keys.Contains("userId")) return RedirectToPage("login");
            var tripManager = FakeTripManager ?? new TripManager();
            UserId = Convert.ToInt32(HttpContext.Session.GetString("userId"));
            Trips = tripManager.GetTripsByUser(UserId).Data;
            return Page();
        }

        /// <summary>
        ///     Called when [post logout].
        /// </summary>
        /// <returns> The action to take when logging out </returns>
        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Remove("userId");
            return RedirectToPage("index");
        }
    }
}