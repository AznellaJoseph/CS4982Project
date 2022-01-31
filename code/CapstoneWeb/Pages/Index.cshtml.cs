using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CapstoneWeb.Pages
{
    /// <summary>
    ///     Index Model
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel" />
    public class IndexModel : PageModel
    {
        public int UserId { get; set; } = -1;
        private readonly ILogger<IndexModel> _logger;

        /// <summary>
        ///     Initializes a new instance of the <see cref="IndexModel" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        /// <summary>
        ///     Called when [get].
        /// </summary>
        /// <returns></returns>
        public IActionResult OnGet()
        {
            string userIdString;
            if (!this.Request.Cookies.TryGetValue("userId", out userIdString))
            {
                return RedirectToPage("login");
            }

            this.UserId = Convert.ToInt32(userIdString);
            return Page();

        }

        public IActionResult OnPostLogout()
        {
            return RedirectToPage("index");
        }
    }
}