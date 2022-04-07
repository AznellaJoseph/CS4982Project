using CapstoneBackend.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CapstoneWeb.Pages
{
    /// <summary>
    ///     PageModel for Login Site
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel" />
    public class LoginModel : PageModel
    {
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
        ///     The error message.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     The user manager.
        /// </summary>
        public UserManager UserManager { get; set; } = new();

        /// <summary>
        ///     Called when [post].
        /// </summary>
        /// <returns>Redirect to index if the user was not logged in or current page if there was an error </returns>
        public IActionResult OnPostLogin()
        {
            var response = UserManager.GetUserByCredentials(Username, Password);
            if (response.Data is not null)
            {
                HttpContext.Session.SetString("userId", $"{response.Data.UserId}");
                return RedirectToPage("Index");
            }

            ErrorMessage = response.ErrorMessage;

            return Page();
        }

        /// <summary>
        ///     Called when [post create account].
        /// </summary>
        /// <returns>A redirect to create account page </returns>
        public IActionResult OnPostCreateAccount()
        {
            return RedirectToPage("CreateAccount");
        }
    }
}