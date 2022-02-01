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
        public string ErrorMessage { get; set; }

        [BindProperty] public string Username { get; set; }

        [BindProperty] public string Password { get; set; }

        [BindProperty] public string FirstName { get; set; }

        [BindProperty] public string LastName { get; set; }

        public UserManager FakeUserManager { get; set; }

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