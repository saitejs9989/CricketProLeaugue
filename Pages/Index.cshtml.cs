using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CricketLeague.Data;
using CricketLeague.Models;

namespace CricketLeague.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _db;
        public IndexModel(AppDbContext db) => _db = db;

        public int TeamCount   { get; set; }
        public int PlayerCount { get; set; }
        public int MatchCount  { get; set; }
        public List<Match> RecentMatches { get; set; } = new();

        public async Task OnGetAsync()
        {
            TeamCount   = await _db.Teams.CountAsync();
            PlayerCount = await _db.Players.CountAsync();
            MatchCount  = await _db.Matches.CountAsync();
            RecentMatches = await _db.Matches
                .Include(m => m.Team1).Include(m => m.Team2)
                .Where(m => m.Status == "Completed")
                .OrderByDescending(m => m.MatchDate)
                .Take(3).ToListAsync();
        }
    }
}
