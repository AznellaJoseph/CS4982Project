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
        [BindProperty] public string Username { get; set; }

        [BindProperty] public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public UserManager FakeUserManager { get; set; }

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
            using var userManager = FakeUserManager ?? new UserManager();
            var response = userManager.GetUserByCredentials(Username ?? string.Empty, Password ?? string.Empty);
            if (string.IsNullOrEmpty(response.ErrorMessage) && response.Data != null)
            {
                HttpContext.Session.SetString("userId", $"{response.Data.Id}");
                return RedirectToPage("Index");
            }

            ErrorMessage = response.ErrorMessage;

            return Page();
        }
    }
}