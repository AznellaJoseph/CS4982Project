using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapstoneBackend.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CapstoneWeb.Pages
{
    public class CreateWaypointModel : PageModel
    {
        [BindProperty] public string ErrorMessage { get; set; }
        [BindProperty] public string Location { get; set; }
        [BindProperty] public DateTime StartDate { get; set; } = DateTime.Now;
        [BindProperty] public DateTime EndDate { get; set; } = DateTime.Now;
        [BindProperty] public string Notes { get; set; }

        public WaypointManager WaypointManager { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            var waypointManager = WaypointManager ?? new WaypointManager();
            var response = waypointManager.CreateWaypoint(Convert.ToInt32(HttpContext.Session.GetString("tripId")), Location, StartDate, EndDate, Notes);
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
