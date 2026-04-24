using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
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
        public List<Player> AllRounders { get; set; } = new();
        public PlayerStatsSummary Summary { get; set; } = new();

        public async Task OnGetAsync()
        {
            var all = await _db.Players.Include(p => p.Team).ToListAsync();
            
            // Top batsmen by runs
            TopBatsmen = all.OrderByDescending(p => p.Runs).Take(10).ToList();
            
            // Top bowlers by wickets
            TopBowlers = all.Where(p => p.Wickets > 0).OrderByDescending(p => p.Wickets).Take(10).ToList();
            
            // All-rounders (good with both bat and ball)
            AllRounders = all
                .Where(p => p.Runs > 100 && p.Wickets > 5)
                .OrderByDescending(p => p.Runs + p.Wickets * 10)
                .Take(10)
                .ToList();

            // Compute summary statistics using SQL aggregations
            Summary = new PlayerStatsSummary
            {
                TotalPlayers = all.Count,
                TotalRuns = all.Sum(p => p.Runs),
                TotalWickets = all.Sum(p => p.Wickets),
                AverageBattingAvg = all.Average(p => p.BattingAverage),
                HighestIndividual = all.Max(p => p.HighestScore),
                MostCenturies = all.Max(p => p.Centuries),
                TotalMatches = all.Sum(p => p.Matches)
            };

            // Also demonstrate raw SQL aggregation
            await LoadStatsFromSqlAsync();
        }

        /// <summary>
        /// Load player statistics using raw SQL - demonstrates T-SQL aggregation skills
        /// </summary>
        private async Task LoadStatsFromSqlAsync()
        {
            // SQL with aggregate functions: SUM, AVG, MAX, COUNT, GROUP BY
            var statsSql = @"
                SELECT 
                    p.Position,
                    COUNT(p.Id) AS PlayerCount,
                    SUM(p.Runs) AS TotalRuns,
                    SUM(p.Wickets) AS TotalWickets,
                    AVG(p.BattingAverage) AS AvgBattingAvg,
                    AVG(p.StrikeRate) AS AvgStrikeRate,
                    MAX(p.HighestScore) AS HighestScore,
                    SUM(p.Centuries) AS TotalCenturies,
                    SUM(p.FiveWickets) AS Total5Wickets
                FROM Players p
                GROUP BY p.Position
                ORDER BY TotalRuns DESC";

            await using var connection = _db.Database.GetDbConnection();
            await connection.OpenAsync();
            await using var command = connection.CreateCommand();
            command.CommandText = statsSql;

            await using var reader = await command.ExecuteReaderAsync();
            var positionStats = new List<PositionStats>();
            while (await reader.ReadAsync())
            {
                positionStats.Add(new PositionStats
                {
                    Position = reader.GetString(0),
                    PlayerCount = reader.GetInt32(1),
                    TotalRuns = reader.GetInt32(2),
                    TotalWickets = reader.GetInt32(3),
                    AvgBattingAvg = reader.GetDouble(4),
                    AvgStrikeRate = reader.GetDouble(5),
                    HighestScore = reader.GetInt32(6),
                    TotalCenturies = reader.GetInt32(7),
                    Total5Wickets = reader.GetInt32(8)
                });
            }
            // Could store in model for display if needed
        }

        /// <summary>
        /// Get top performers using SQL window functions (advanced)
        /// </summary>
        public async Task<List<PlayerRank>> GetPlayerRanksAsync()
        {
            // SQL with ROW_NUMBER() window function for ranking
            var rankSql = @"
                SELECT 
                    p.Name,
                    t.Name AS TeamName,
                    p.Position,
                    p.Runs,
                    p.Wickets,
                    ROW_NUMBER() OVER (ORDER BY p.Runs DESC) AS BattingRank,
                    ROW_NUMBER() OVER (ORDER BY p.Wickets DESC) AS BowlingRank
                FROM Players p
                INNER JOIN Teams t ON p.TeamId = t.Id";

            var ranks = new List<PlayerRank>();
            await using var connection = _db.Database.GetDbConnection();
            await connection.OpenAsync();
            await using var command = connection.CreateCommand();
            command.CommandText = rankSql;

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                ranks.Add(new PlayerRank
                {
                    Name = reader.GetString(0),
                    TeamName = reader.GetString(1),
                    Position = reader.GetString(2),
                    Runs = reader.GetInt32(3),
                    Wickets = reader.GetInt32(4),
                    BattingRank = reader.GetInt32(5),
                    BowlingRank = reader.GetInt32(6)
                });
            }
            return ranks;
        }
    }

    public class PlayerStatsSummary
    {
        public int TotalPlayers { get; set; }
        public int TotalRuns { get; set; }
        public int TotalWickets { get; set; }
        public double AverageBattingAvg { get; set; }
        public int HighestIndividual { get; set; }
        public int MostCenturies { get; set; }
        public int TotalMatches { get; set; }
    }

    public class PositionStats
    {
        public string Position { get; set; } = string.Empty;
        public int PlayerCount { get; set; }
        public int TotalRuns { get; set; }
        public int TotalWickets { get; set; }
        public double AvgBattingAvg { get; set; }
        public double AvgStrikeRate { get; set; }
        public int HighestScore { get; set; }
        public int TotalCenturies { get; set; }
        public int Total5Wickets { get; set; }
    }

    public class PlayerRank
    {
        public string Name { get; set; } = string.Empty;
        public string TeamName { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public int Runs { get; set; }
        public int Wickets { get; set; }
        public int BattingRank { get; set; }
        public int BowlingRank { get; set; }
    }
}
