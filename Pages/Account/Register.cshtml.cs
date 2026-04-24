using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CricketLeague.Models;

namespace CricketLeague.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser>   _um;
        private readonly SignInManager<ApplicationUser> _sm;
        public RegisterModel(UserManager<ApplicationUser> um, SignInManager<ApplicationUser> sm)
        { _um = um; _sm = sm; }

        [BindProperty] public string FullName       { get; set; } = string.Empty;
        [BindProperty] public string Email          { get; set; } = string.Empty;
        [BindProperty] public string Password       { get; set; } = string.Empty;
        [BindProperty] public string ConfirmPassword{ get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new();

        public async Task<IActionResult> OnPostAsync()
        {
            if (Password != ConfirmPassword) { Errors.Add("Passwords do not match."); return Page(); }
            var user = new ApplicationUser { UserName = Email, Email = Email, FullName = FullName };
            var result = await _um.CreateAsync(user, Password);
            if (result.Succeeded) { await _sm.SignInAsync(user, false); return RedirectToPage("/Index"); }
            Errors = result.Errors.Select(e => e.Description).ToList();
            return Page();
        }
    }
}
