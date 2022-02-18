using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapstoneBackend.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CapstoneWeb.Pages
{
    public class TripOverviewModel : PageModel
    {

        public Trip CurrentTrip;

        public TripManager FakeTripManager;

        public IActionResult OnGet()
        {
            //var tripManager = FakeTripManager ?? new TripManager();

            CurrentTrip = new Trip();
            CurrentTrip.Name = "Test Trip";
            CurrentTrip.Notes = "";
            CurrentTrip.StartDate = new DateTime(2022, 2, 8);
            CurrentTrip.EndDate = new DateTime(2022, 2, 20);
            
            return Page();
        }
    }
}
