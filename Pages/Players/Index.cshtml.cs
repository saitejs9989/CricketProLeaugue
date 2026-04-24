using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CricketLeague.Data;
using CricketLeague.Models;

namespace CricketLeague.Pages.Players
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _db;
        public IndexModel(AppDbContext db) => _db = db;
        public List<Player> Players        { get; set; } = new();
        public string       PositionFilter { get; set; } = "All";

        public async Task OnGetAsync(string? pos)
        {
            PositionFilter = string.IsNullOrEmpty(pos) ? "All" : pos;
            var q = _db.Players.Include(p => p.Team).AsQueryable();
            if (PositionFilter != "All") q = q.Where(p => p.Position == PositionFilter);
            Players = await q.OrderByDescending(p => p.Runs).ToListAsync();
        }
    }
}
