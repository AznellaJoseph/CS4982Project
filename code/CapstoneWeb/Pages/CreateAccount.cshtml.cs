using System.Diagnostics;
using CapstoneBackend.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CapstoneWeb.Pages
{
    /// <summary>
    ///     Create Account Model
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel" />
    public class CreateAccountModel : PageModel
    {
        public int Id { get; set; }
        public string ErrorMessage { get; set; }

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        [BindProperty]
        public string FirstName { get; set; }

        [BindProperty]
        public string LastName { get; set; }

        /// <summary>
        ///     Called when [get].
        /// </summary>
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            var response = UserManager.RegisterUser(Username ?? string.Empty, Password ?? string.Empty, FirstName ?? string.Empty, LastName ?? string.Empty);
            if (response.StatusCode == 200)
            {
                IndexModel.UserId = response.Data;
                return RedirectToPage("Index");
            }
            else
            {
                ErrorMessage = response.ErrorMessage ?? "Unknown Error.";
            }

            return Page();
        }
    }
}