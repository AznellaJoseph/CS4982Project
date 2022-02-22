using CapstoneBackend.Model;
using Microsoft.AspNetCore.Http;
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
        ///     The fake user manager.
        /// </summary>
        public UserManager FakeUserManager { get; set; }

        /// <summary>
        ///     Called when [post].
        /// </summary>
        /// <returns> A redirect to the next page or the current page if there was an error </returns>
        public IActionResult OnPost()
        {
            var userManager = FakeUserManager ?? new UserManager();
            var response = userManager.GetUserByCredentials(Username ?? string.Empty, Password ?? string.Empty);
            if (response.Data is not null)
            {
                HttpContext.Session.SetString("userId", $"{response.Data.UserId}");
                return RedirectToPage("index");
            }

            ErrorMessage = response.ErrorMessage;

            return Page();
        }

        /// <summary>
        ///     Called when [post create account].
        /// </summary>
        /// <returns>A redirect to the next page or the current page if there was an error </returns>
        public IActionResult OnPostCreateAccount()
        {
            return RedirectToPage("createaccount");
        }
    }
}