using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CapstoneWeb.Pages
{
    /// <summary>
    ///     Login Model
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel" />
    public class LoginModel : PageModel
    {
        public string Username { get; set; }
        public string Password { get; set; }

        /// <summary>
        ///     Called when [get].
        /// </summary>
        public void OnGet()
        {
        }

        /// <summary>
        ///     Called when [post].
        /// </summary>
        public void OnPost()
        {
            Debug.WriteLine("LOGIN");
        }
    }
}