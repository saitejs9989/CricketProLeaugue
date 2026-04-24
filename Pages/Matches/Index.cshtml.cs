using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CricketLeague.Data;
using CricketLeague.Models;

namespace CricketLeague.Pages.Matches
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _db;
        public IndexModel(AppDbContext db) => _db = db;
        public List<Match> Upcoming  { get; set; } = new();
        public List<Match> Completed { get; set; } = new();

        public async Task OnGetAsync()
        {
            var all = await _db.Matches.Include(m => m.Team1).Include(m => m.Team2).ToListAsync();
            Upcoming  = all.Where(m => m.Status == "Upcoming").OrderBy(m => m.MatchDate).ToList();
            Completed = all.Where(m => m.Status == "Completed").OrderByDescending(m => m.MatchDate).ToList();
        }
    }
}
