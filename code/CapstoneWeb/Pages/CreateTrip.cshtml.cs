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
    public class CreateTripModel : PageModel
    {

        [BindProperty] public string ErrorMessage { get; set; }
        [BindProperty] public string Name { get; set; }
        [BindProperty] public DateTime StartDate { get; set; } = DateTime.Today;
        [BindProperty] public DateTime EndDate { get; set; } = DateTime.Today;
        [BindProperty] public string Notes { get; set; }

        public TripManager TripManager { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            var tripManager = TripManager ?? new TripManager();
            var response = tripManager.CreateTrip(Convert.ToInt32(HttpContext.Session.GetString("userId")), Name, Notes,
                StartDate, EndDate);
            if (string.IsNullOrEmpty(response.ErrorMessage))
            {
                HttpContext.Session.SetString("tripId", $"{response.Data}");
                return RedirectToPage("index");
            }

            ErrorMessage = response.ErrorMessage;
            return Page();
        }
    }
}
