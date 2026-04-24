using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
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

            // Dynamic search with different filter combinations
            // Using raw SQL for complex queries - demonstrates SQL Server skills
            if (Filter is "all" or "teams")
            {
                Teams = await _db.Teams.Where(t =>
                    t.Name.ToLower().Contains(q) || t.Captain.ToLower().Contains(q) ||
                    t.Coach.ToLower().Contains(q) || t.Country.ToLower().Contains(q) ||
                    t.HomeGround.ToLower().Contains(q)).ToListAsync();
            }

            if (Filter is "all" or "players")
            {
                // JOIN query - players with their teams
                Players = await _db.Players.Include(p => p.Team).Where(p =>
                    p.Name.ToLower().Contains(q) ||
                    p.Position.ToLower().Contains(q) ||
                    p.Nationality.ToLower().Contains(q)).ToListAsync();
            }

            if (Filter is "all" or "matches")
            {
                // Multi-table JOIN - matches with both teams
                Matches = await _db.Matches.Include(m => m.Team1).Include(m => m.Team2).Where(m =>
                    m.Venue.ToLower().Contains(q) ||
                    m.Status.ToLower().Contains(q) ||
                    (m.Team1 != null && m.Team1.Name.ToLower().Contains(q)) ||
                    (m.Team2 != null && m.Team2.Name.ToLower().Contains(q))).ToListAsync();
            }
        }

        /// <summary>
        /// Advanced search using raw SQL - demonstrates T-SQL skills
        /// This shows complex query building with dynamic filters
        /// </summary>
        public async Task SearchWithRawSqlAsync(string searchTerm, string entityType)
        {
            if (string.IsNullOrWhiteSpace(searchTerm)) return;

            var param = new SqlParameter("@searchTerm", $"%{searchTerm}%");

            switch (entityType.ToLower())
            {
                case "teams":
                    // SQL with computed columns and filtering
                    var teamSql = @"
                        SELECT Id, Name, Coach, Captain, Country, HomeGround, LogoEmoji,
                               Wins, Losses, Draws, NRR,
                               (Wins + Losses + Draws) AS MatchesPlayed,
                               (Wins * 2 + Draws) AS Points
                        FROM Teams 
                        WHERE Name LIKE @searchTerm 
                           OR Captain LIKE @searchTerm 
                           OR Coach LIKE @searchTerm";
                    Teams = await _db.Teams.FromSqlRaw(teamSql, param).ToListAsync();
                    break;

                case "players":
                    // SQL with JOIN and aggregation
                    var playerSql = @"
                        SELECT p.*, t.Name AS TeamName
                        FROM Players p
                        INNER JOIN Teams t ON p.TeamId = t.Id
                        WHERE p.Name LIKE @searchTerm 
                           OR p.Position LIKE @searchTerm 
                           OR p.Nationality LIKE @searchTerm";
                    Players = await _db.Players.FromSqlRaw(playerSql, param).ToListAsync();
                    break;

                case "matches":
                    // SQL with multiple JOINs
                    var matchSql = @"
                        SELECT m.*, 
                               t1.Name AS Team1Name, 
                               t2.Name AS Team2Name
                        FROM Matches m
                        LEFT JOIN Teams t1 ON m.Team1Id = t1.Id
                        LEFT JOIN Teams t2 ON m.Team2Id = t2.Id
                        WHERE m.Venue LIKE @searchTerm 
                           OR m.Status LIKE @searchTerm
                           OR t1.Name LIKE @searchTerm
                           OR t2.Name LIKE @searchTerm";
                    Matches = await _db.Matches.FromSqlRaw(matchSql, param).ToListAsync();
                    break;
            }
        }
    }
}
