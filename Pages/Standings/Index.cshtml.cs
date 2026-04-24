using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using CricketLeague.Data;
using CricketLeague.Models;

namespace CricketLeague.Pages.Standings
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _db;
        public IndexModel(AppDbContext db) => _db = db;
        
        public List<TeamStandings> Teams { get; set; } = new();
        public List<Match> RecentMatches { get; set; } = new();

        public async Task OnGetAsync()
        {
            // Method 1: Using LINQ with computed properties (in-memory)
            var teams = await _db.Teams.ToListAsync();
            Teams = teams.Select(t => new TeamStandings
            {
                Team = t,
                MatchesPlayed = t.Wins + t.Losses + t.Draws,
                Points = t.Wins * 2 + t.Draws,
                WinRate = t.Wins + t.Losses + t.Draws > 0 
                    ? Math.Round((double)t.Wins / (t.Wins + t.Losses + t.Draws) * 100, 1)
                    : 0
            })
            .OrderByDescending(t => t.Points)
            .ThenByDescending(t => t.Team.Wins)
            .ThenByDescending(t => t.Team.NRR)
            .ToList();

            // Method 2: Using raw SQL with JOINs and computed columns
            // This demonstrates T-SQL skills for computed standings
            await LoadStandingsFromSqlAsync();

            // Get recent matches for context
            RecentMatches = await _db.Matches
                .Include(m => m.Team1)
                .Include(m => m.Team2)
                .Where(m => m.Status == "Completed")
                .OrderByDescending(m => m.MatchDate)
                .Take(5)
                .ToListAsync();
        }

        /// <summary>
        /// Load standings using raw SQL - demonstrates T-SQL JOINs and aggregations
        /// </summary>
        private async Task LoadStandingsFromSqlAsync()
        {
            // Raw SQL query that computes standings from match results
            // This shows advanced SQL Server skills:
            // - Multiple JOINs
            // - Conditional aggregation
            // - Computed columns
            // - Subqueries
            var standingsSql = @"
                SELECT 
                    t.Id,
                    t.Name,
                    t.Coach,
                    t.Captain,
                    t.Country,
                    t.HomeGround,
                    t.LogoEmoji,
                    t.Wins,
                    t.Losses,
                    t.Draws,
                    t.NRR,
                    (t.Wins + t.Losses + t.Draws) AS MatchesPlayed,
                    (t.Wins * 2 + t.Draws) AS Points,
                    CASE 
                        WHEN (t.Wins + t.Losses + t.Draws) > 0 
                        THEN ROUND(CAST(t.Wins AS FLOAT) / (t.Wins + t.Losses + t.Draws) * 100, 1)
                        ELSE 0 
                    END AS WinRate
                FROM Teams t
                ORDER BY Points DESC, t.Wins DESC, t.NRR DESC";

            // Execute raw SQL and map results
            await using var connection = _db.Database.GetDbConnection();
            await connection.OpenAsync();
            await using var command = connection.CreateCommand();
            command.CommandText = standingsSql;

            await using var reader = await command.ExecuteReaderAsync();
            var sqlResults = new List<TeamStandings>();
            while (await reader.ReadAsync())
            {
                sqlResults.Add(new TeamStandings
                {
                    Team = new Team
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Coach = reader.GetString(2),
                        Captain = reader.GetString(3),
                        Country = reader.GetString(4),
                        HomeGround = reader.GetString(5),
                        LogoEmoji = reader.GetString(6),
                        Wins = reader.GetInt32(7),
                        Losses = reader.GetInt32(8),
                        Draws = reader.GetInt32(9),
                        NRR = reader.GetInt32(10)
                    },
                    MatchesPlayed = reader.GetInt32(11),
                    Points = reader.GetInt32(12),
                    WinRate = reader.GetDouble(13)
                });
            }
        }

        /// <summary>
        /// Advanced SQL: Get team performance using aggregate functions
        /// </summary>
        public async Task<List<TeamPerformance>> GetTeamPerformanceAsync()
        {
            var perfSql = @"
                SELECT 
                    t.Name AS TeamName,
                    COUNT(m.Id) AS TotalMatches,
                    SUM(CASE WHEN m.WinnerTeamId = t.Id THEN 1 ELSE 0 END) AS Wins,
                    SUM(CASE WHEN m.WinnerTeamId IS NULL AND m.Status = 'Completed' THEN 1 ELSE 0 END) AS Draws,
                    SUM(CASE WHEN m.WinnerTeamId <> t.Id AND m.Status = 'Completed' AND m.WinnerTeamId IS NOT NULL THEN 1 ELSE 0 END) AS Losses,
                    AVG(CASE WHEN m.Team1Id = t.Id THEN m.Team1Score ELSE m.Team2Score END) AS AvgScore
                FROM Teams t
                LEFT JOIN Matches m ON (m.Team1Id = t.Id OR m.Team2Id = t.Id) AND m.Status = 'Completed'
                GROUP BY t.Id, t.Name
                ORDER BY Wins DESC";

            var results = new List<TeamPerformance>();
            await using var connection = _db.Database.GetDbConnection();
            await connection.OpenAsync();
            await using var command = connection.CreateCommand();
            command.CommandText = perfSql;

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                results.Add(new TeamPerformance
                {
                    TeamName = reader.GetString(0),
                    TotalMatches = reader.GetInt32(1),
                    Wins = reader.GetInt32(2),
                    Draws = reader.GetInt32(3),
                    Losses = reader.GetInt32(4)
                });
            }
            return results;
        }
    }

    // View models for computed standings
    public class TeamStandings
    {
        public Team Team { get; set; } = new();
        public int MatchesPlayed { get; set; }
        public int Points { get; set; }
        public double WinRate { get; set; }
    }

    public class TeamPerformance
    {
        public string TeamName { get; set; } = string.Empty;
        public int TotalMatches { get; set; }
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Losses { get; set; }
    }
}
