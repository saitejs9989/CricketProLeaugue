using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CricketLeague.Data;
using CricketLeague.Models;

namespace CricketLeague.Pages.Search
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _db;
        public IndexModel(AppDbContext db) => _db = db;

        [BindProperty(SupportsGet = true)] public string Query  { get; set; } = string.Empty;
        [BindProperty(SupportsGet = true)] public string Filter { get; set; } = "all";

        public List<Team>   Teams   { get; set; } = new();
        public List<Player> Players { get; set; } = new();
        public List<Match>  Matches { get; set; } = new();

        public async Task OnGetAsync()
        {
            if (string.IsNullOrWhiteSpace(Query)) return;
            var q = Query.ToLower();

            if (Filter is "all" or "teams")
                Teams = await _db.Teams.Where(t =>
                    t.Name.ToLower().Contains(q) || t.Captain.ToLower().Contains(q) ||
                    t.Coach.ToLower().Contains(q) || t.Country.ToLower().Contains(q) ||
                    t.HomeGround.ToLower().Contains(q)).ToListAsync();

            if (Filter is "all" or "players")
                Players = await _db.Players.Include(p => p.Team).Where(p =>
                    p.Name.ToLower().Contains(q) ||
                    p.Position.ToLower().Contains(q) ||
                    p.Nationality.ToLower().Contains(q)).ToListAsync();

            if (Filter is "all" or "matches")
                Matches = await _db.Matches.Include(m => m.Team1).Include(m => m.Team2).Where(m =>
                    m.Venue.ToLower().Contains(q) ||
                    m.Status.ToLower().Contains(q) ||
                    (m.Team1 != null && m.Team1.Name.ToLower().Contains(q)) ||
                    (m.Team2 != null && m.Team2.Name.ToLower().Contains(q))).ToListAsync();
        }
    }
}
