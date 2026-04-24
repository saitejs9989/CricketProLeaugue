using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CricketLeague.Data;
using CricketLeague.Models;

namespace CricketLeague.Pages.Teams
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _db;
        public IndexModel(AppDbContext db) => _db = db;
        public List<Team> Teams { get; set; } = new();
        public async Task OnGetAsync() =>
            Teams = await _db.Teams.Include(t => t.Players).OrderByDescending(t => t.Wins * 2 + t.Draws).ThenByDescending(t => t.Wins).ToListAsync();
    }
}
