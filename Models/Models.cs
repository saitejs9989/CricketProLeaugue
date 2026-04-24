using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CricketLeague.Models
{
    // ─── Identity User ────────────────────────────────────────────────────────
    public class ApplicationUser : IdentityUser
    {
        [Required] public string FullName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    // ─── Team ─────────────────────────────────────────────────────────────────
    public class Team
    {
        public int    Id      { get; set; }
        [Required] public string Name    { get; set; } = string.Empty;
        [Required] public string Coach   { get; set; } = string.Empty;
        [Required] public string Captain { get; set; } = string.Empty;
        public string Country  { get; set; } = string.Empty;
        public string HomeGround { get; set; } = string.Empty;
        public string LogoEmoji  { get; set; } = "🏏";

        // Season record
        public int Wins   { get; set; }
        public int Losses { get; set; }
        public int Draws  { get; set; }
        public int NRR    { get; set; }   // Net run rate × 100 (stored as int)

        // Computed — not stored in DB
        [NotMapped] public int MatchesPlayed => Wins + Losses + Draws;
        [NotMapped] public int Points        => Wins * 2 + Draws;

        public ICollection<Player> Players { get; set; } = new List<Player>();
    }

    // ─── Player ───────────────────────────────────────────────────────────────
    public class Player
    {
        public int Id { get; set; }
        [Required] public string Name        { get; set; } = string.Empty;
        public string Position    { get; set; } = string.Empty; // Batsman | Bowler | All-Rounder | Wicket-Keeper
        public string Nationality { get; set; } = string.Empty;
        public int    Age         { get; set; }

        public int  TeamId { get; set; }
        public Team? Team  { get; set; }

        // Batting
        public int    Matches        { get; set; }
        public int    Runs           { get; set; }
        public double BattingAverage { get; set; }
        public double StrikeRate     { get; set; }
        public int    Centuries      { get; set; }
        public int    HalfCenturies  { get; set; }
        public int    HighestScore   { get; set; }
        public int    Fours          { get; set; }
        public int    Sixes          { get; set; }

        // Bowling
        public int    Wickets        { get; set; }
        public double BowlingAverage { get; set; }
        public double Economy        { get; set; }
        public string BestBowling    { get; set; } = "0/0";
        public int    FiveWickets    { get; set; }
    }

    // ─── Match ────────────────────────────────────────────────────────────────
    public class Match
    {
        public int Id { get; set; }

        public int   Team1Id { get; set; }
        public Team? Team1   { get; set; }
        public int   Team2Id { get; set; }
        public Team? Team2   { get; set; }

        public DateTime MatchDate { get; set; }
        public string   Venue     { get; set; } = string.Empty;
        public string   Status    { get; set; } = "Upcoming"; // Upcoming | Live | Completed

        public string Team1Score { get; set; } = "-";
        public string Team2Score { get; set; } = "-";
        public string? Result    { get; set; }
        public int?    WinnerTeamId { get; set; }
        public string  MatchType    { get; set; } = "T20";     // T20 | ODI | Test
    }
}
