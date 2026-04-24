using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CricketLeague.Data;
using CricketLeague.Models;

namespace CricketLeague.Pages.PlayerStats
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _db;
        public IndexModel(AppDbContext db) => _db = db;
        public List<Player> TopBatsmen { get; set; } = new();
        public List<Player> TopBowlers { get; set; } = new();

        public async Task OnGetAsync()
        {
            var all = await _db.Players.Include(p => p.Team).ToListAsync();
            TopBatsmen = all.OrderByDescending(p => p.Runs).Take(10).ToList();
            TopBowlers = all.Where(p => p.Wickets > 0).OrderByDescending(p => p.Wickets).ToList();
        }
    }
}
