using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CricketLeague.Models;

namespace CricketLeague.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _sm;
        public LogoutModel(SignInManager<ApplicationUser> sm) => _sm = sm;
        public async Task<IActionResult> OnGetAsync()
        { await _sm.SignOutAsync(); return RedirectToPage("/Index"); }
    }
}
