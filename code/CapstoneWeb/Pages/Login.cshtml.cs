using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Threading.Tasks;
using CapstoneBackend.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CapstoneWeb.Pages
{
    /// <summary>
    ///     Login Model
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel" />
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        /// <summary>
        ///     Called when [get].
        /// </summary>
        public void OnGet()
        {
        }

        /// <summary>
        ///     Called when [post].
        /// </summary>
        public IActionResult OnPost()
        {
            var response = UserManager.GetUserByCredentials(Username ?? string.Empty, Password ?? string.Empty);
            if (string.IsNullOrEmpty(response.ErrorMessage) && response.Data != null)
            {
                IndexModel.UserId = response.Data.Id;
                return RedirectToPage("Index");
            }

            else
            {
                ErrorMessage = response.ErrorMessage;
            }

            return Page();
        }
    }
}