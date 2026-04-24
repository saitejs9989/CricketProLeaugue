using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CricketLeague.Models;

namespace CricketLeague.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _sm;
        public LoginModel(SignInManager<ApplicationUser> sm) => _sm = sm;

        [BindProperty] public string Email    { get; set; } = string.Empty;
        [BindProperty] public string Password { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;

        public async Task<IActionResult> OnPostAsync()
        {
            var r = await _sm.PasswordSignInAsync(Email, Password, false, false);
            if (r.Succeeded) return RedirectToPage("/Index");
            Error = "Invalid email or password. Please try again.";
            return Page();
        }
    }
}
