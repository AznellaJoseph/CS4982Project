using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CapstoneWeb.Pages
{
    /// <summary>
    ///     PageModel for Create Account Site
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
        ///     The confirmed password.
        /// </summary>
        [BindProperty]
        public string ConfirmedPassword { get; set; }

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
        ///     The user manager.
        /// </summary>
        public UserManager UserManager { get; set; } = new();

        /// <summary>
        ///     Called when [post].
        /// </summary>
        /// <returns>Redirect to index or current page if there was an error. </returns>
        public IActionResult OnPost()
        {
            if (Password != ConfirmedPassword)
            {
                ErrorMessage = Ui.ErrorMessages.PasswordsDoNotMatch;
                return Page();
            }

            var response = UserManager.RegisterUser(Username, Password,
                FirstName, LastName);
            if (response.StatusCode == (uint) Ui.StatusCode.Success)
            {
                HttpContext.Session.SetString("userId", $"{response.Data}");
                return RedirectToPage("Index");
            }

            ErrorMessage = response.ErrorMessage ?? Ui.ErrorMessages.UnknownError;

            return Page();
        }


        /// <summary>Called when [post cancel].</summary>
        /// <returns>Redirect to login page</returns>
        public IActionResult OnPostCancel()
        {
            return RedirectToPage("login");
        }
    }
}