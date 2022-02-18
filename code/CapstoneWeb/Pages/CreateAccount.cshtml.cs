using CapstoneBackend.Model;
using Microsoft.AspNetCore.Http;
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
        /// <summary>
        ///     The error message.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     The username.
        /// </summary>
        [BindProperty]
        public string Username { get; set; }

        /// <summary>
        ///     The password.
        /// </summary>
        [BindProperty]
        public string Password { get; set; }

        /// <summary>
        ///     The first name.
        /// </summary>
        [BindProperty]
        public string FirstName { get; set; }

        /// <summary>
        ///     The last name.
        /// </summary>
        [BindProperty]
        public string LastName { get; set; }

        /// <summary>
        ///     The fake user manager.
        /// </summary>
        public UserManager FakeUserManager { get; set; }

        /// <summary>
        ///     Called when [post].
        /// </summary>
        /// <returns> The page to go to after [post] </returns>
        public IActionResult OnPost()
        {
            var userManager = FakeUserManager ?? new UserManager();
            var response = userManager.RegisterUser(Username ?? string.Empty, Password ?? string.Empty,
                FirstName ?? string.Empty, LastName ?? string.Empty);
            if (response.StatusCode == 200)
            {
                HttpContext.Session.SetString("userId", $"{response.Data}");
                return RedirectToPage("Index");
            }

            ErrorMessage = response.ErrorMessage ?? "Unknown Error.";

            return Page();
        }
    }
}