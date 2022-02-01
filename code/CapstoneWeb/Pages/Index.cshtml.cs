using System;
using System.Linq;
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
        public int UserId { get; private set; }

        /// <summary>
        ///     Called when [get].
        /// </summary>
        /// <returns></returns>
        public IActionResult OnGet()
        {
            if (!HttpContext.Session.Keys.Contains("userId")) return RedirectToPage("login");
            UserId = Convert.ToInt32(HttpContext.Session.GetString("userId"));
            return Page();
        }

        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Remove("userId");
            return RedirectToPage("index");
        }
    }
}