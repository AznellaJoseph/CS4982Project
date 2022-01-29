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
            return RedirectToPage("login");
        }
    }
}