using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CricketLeague.Models;

namespace CricketLeague.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Team>   Teams   { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Match>  Matches { get; set; }

        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);

            // ── Teams ──────────────────────────────────────────────────────────
            b.Entity<Team>().HasData(
                new Team { Id=1, Name="Mumbai Strikers",   Coach="Ravi Shastri",    Captain="Rohit Sharma",    Country="India",        HomeGround="Wankhede Stadium",          LogoEmoji="🔵", Wins=9, Losses=2, Draws=0, NRR= 62 },
                new Team { Id=2, Name="Delhi Dynamos",     Coach="Anil Kumble",     Captain="Virat Kohli",     Country="India",        HomeGround="Feroz Shah Kotla",          LogoEmoji="🔴", Wins=8, Losses=3, Draws=0, NRR= 45 },
                new Team { Id=3, Name="Chennai Kings",     Coach="Stephen Fleming", Captain="MS Dhoni",        Country="India",        HomeGround="MA Chidambaram Stadium",    LogoEmoji="🟡", Wins=7, Losses=4, Draws=0, NRR= 31 },
                new Team { Id=4, Name="Kolkata Knights",   Coach="Trevor Bayliss",  Captain="KL Rahul",        Country="India",        HomeGround="Eden Gardens",              LogoEmoji="🟣", Wins=6, Losses=5, Draws=0, NRR= 18 },
                new Team { Id=5, Name="Bangalore Royals",  Coach="Mike Hesson",     Captain="Faf du Plessis",  Country="South Africa", HomeGround="M. Chinnaswamy Stadium",    LogoEmoji="🟤", Wins=5, Losses=6, Draws=0, NRR=-12 },
                new Team { Id=6, Name="Hyderabad Hawks",   Coach="Tom Moody",       Captain="Kane Williamson", Country="New Zealand",  HomeGround="Rajiv Gandhi Intl. Stadium",LogoEmoji="🟠", Wins=3, Losses=8, Draws=0, NRR=-44 }
            );

            // ── Players ────────────────────────────────────────────────────────
            b.Entity<Player>().HasData(
                // Mumbai (1)
                new Player { Id=1,  Name="Rohit Sharma",    Position="Batsman",       Nationality="Indian",       Age=36, TeamId=1, Matches=11, Runs=612, BattingAverage=55.6, StrikeRate=142.3, Centuries=2, HalfCenturies=4, HighestScore=124, Fours=58, Sixes=22, Wickets=0,  BowlingAverage=0,    Economy=0,   BestBowling="0/0" },
                new Player { Id=2,  Name="Jasprit Bumrah",  Position="Bowler",        Nationality="Indian",       Age=30, TeamId=1, Matches=11, Runs=18,  BattingAverage=6.0,  StrikeRate=90.0,  Centuries=0, HalfCenturies=0, HighestScore=14,  Fours=2,  Sixes=1,  Wickets=24, BowlingAverage=17.2, Economy=6.4, BestBowling="5/27" },
                new Player { Id=3,  Name="Hardik Pandya",   Position="All-Rounder",   Nationality="Indian",       Age=30, TeamId=1, Matches=11, Runs=310, BattingAverage=31.0, StrikeRate=152.4, Centuries=0, HalfCenturies=2, HighestScore=76,  Fours=26, Sixes=18, Wickets=14, BowlingAverage=26.1, Economy=8.2, BestBowling="3/28" },
                // Delhi (2)
                new Player { Id=4,  Name="Virat Kohli",     Position="Batsman",       Nationality="Indian",       Age=35, TeamId=2, Matches=11, Runs=694, BattingAverage=63.1, StrikeRate=138.8, Centuries=3, HalfCenturies=2, HighestScore=131, Fours=72, Sixes=19, Wickets=0,  BowlingAverage=0,    Economy=0,   BestBowling="0/0" },
                new Player { Id=5,  Name="Mohammed Shami",  Position="Bowler",        Nationality="Indian",       Age=33, TeamId=2, Matches=11, Runs=22,  BattingAverage=5.5,  StrikeRate=88.0,  Centuries=0, HalfCenturies=0, HighestScore=12,  Fours=3,  Sixes=0,  Wickets=21, BowlingAverage=19.8, Economy=7.1, BestBowling="4/22" },
                // Chennai (3)
                new Player { Id=6,  Name="MS Dhoni",        Position="Wicket-Keeper", Nationality="Indian",       Age=42, TeamId=3, Matches=11, Runs=335, BattingAverage=47.9, StrikeRate=162.1, Centuries=0, HalfCenturies=3, HighestScore=84,  Fours=28, Sixes=21, Wickets=0,  BowlingAverage=0,    Economy=0,   BestBowling="0/0" },
                new Player { Id=7,  Name="Ravindra Jadeja", Position="All-Rounder",   Nationality="Indian",       Age=35, TeamId=3, Matches=11, Runs=298, BattingAverage=29.8, StrikeRate=143.2, Centuries=0, HalfCenturies=2, HighestScore=82,  Fours=24, Sixes=12, Wickets=18, BowlingAverage=22.4, Economy=7.3, BestBowling="4/33" },
                // Kolkata (4)
                new Player { Id=8,  Name="KL Rahul",        Position="Batsman",       Nationality="Indian",       Age=32, TeamId=4, Matches=11, Runs=502, BattingAverage=45.6, StrikeRate=134.5, Centuries=1, HalfCenturies=4, HighestScore=108, Fours=46, Sixes=16, Wickets=0,  BowlingAverage=0,    Economy=0,   BestBowling="0/0" },
                // Bangalore (5)
                new Player { Id=9,  Name="Faf du Plessis",  Position="Batsman",       Nationality="South African",Age=39, TeamId=5, Matches=11, Runs=448, BattingAverage=40.7, StrikeRate=132.1, Centuries=1, HalfCenturies=3, HighestScore=96,  Fours=41, Sixes=14, Wickets=0,  BowlingAverage=0,    Economy=0,   BestBowling="0/0" },
                // Hyderabad (6)
                new Player { Id=10, Name="Kane Williamson", Position="Batsman",       Nationality="New Zealander",Age=33, TeamId=6, Matches=11, Runs=378, BattingAverage=34.4, StrikeRate=121.9, Centuries=0, HalfCenturies=3, HighestScore=89,  Fours=36, Sixes=9,  Wickets=0,  BowlingAverage=0,    Economy=0,   BestBowling="0/0" }
            );

            // ── Matches ────────────────────────────────────────────────────────
            b.Entity<Match>().HasData(
                new Match { Id=1, Team1Id=1, Team2Id=2, MatchDate=DateTime.UtcNow.AddDays(-14), Venue="Wankhede Stadium, Mumbai",           Status="Completed", MatchType="T20", Team1Score="198/4", Team2Score="194/7", Result="Mumbai Strikers won by 4 runs",       WinnerTeamId=1 },
                new Match { Id=2, Team1Id=3, Team2Id=4, MatchDate=DateTime.UtcNow.AddDays(-10), Venue="MA Chidambaram Stadium, Chennai",     Status="Completed", MatchType="T20", Team1Score="215/3", Team2Score="188/8", Result="Chennai Kings won by 27 runs",        WinnerTeamId=3 },
                new Match { Id=3, Team1Id=5, Team2Id=6, MatchDate=DateTime.UtcNow.AddDays(-7),  Venue="M. Chinnaswamy Stadium, Bangalore",   Status="Completed", MatchType="T20", Team1Score="172/6", Team2Score="168/9", Result="Bangalore Royals won by 4 runs",      WinnerTeamId=5 },
                new Match { Id=4, Team1Id=2, Team2Id=3, MatchDate=DateTime.UtcNow.AddDays(-4),  Venue="Feroz Shah Kotla, Delhi",             Status="Completed", MatchType="T20", Team1Score="187/5", Team2Score="182/6", Result="Delhi Dynamos won by 5 runs",         WinnerTeamId=2 },
                new Match { Id=5, Team1Id=1, Team2Id=4, MatchDate=DateTime.UtcNow.AddDays(-2),  Venue="Wankhede Stadium, Mumbai",           Status="Completed", MatchType="T20", Team1Score="201/5", Team2Score="178/9", Result="Mumbai Strikers won by 23 runs",      WinnerTeamId=1 },
                new Match { Id=6, Team1Id=1, Team2Id=3, MatchDate=DateTime.UtcNow.AddDays(2),   Venue="Wankhede Stadium, Mumbai",           Status="Upcoming",  MatchType="T20", Team1Score="-",      Team2Score="-",     Result=null,                                  WinnerTeamId=null },
                new Match { Id=7, Team1Id=2, Team2Id=5, MatchDate=DateTime.UtcNow.AddDays(5),   Venue="Feroz Shah Kotla, Delhi",             Status="Upcoming",  MatchType="T20", Team1Score="-",      Team2Score="-",     Result=null,                                  WinnerTeamId=null },
                new Match { Id=8, Team1Id=4, Team2Id=6, MatchDate=DateTime.UtcNow.AddDays(8),   Venue="Eden Gardens, Kolkata",               Status="Upcoming",  MatchType="T20", Team1Score="-",      Team2Score="-",     Result=null,                                  WinnerTeamId=null }
            );
        }
    }
}
