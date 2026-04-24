# 🏏 CricketPro League — ASP.NET Core Web Application

<div align="center">

![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-12.0-239120?style=for-the-badge&logo=csharp&logoColor=white)
![Entity Framework](https://img.shields.io/badge/EF_Core-8.0-512BD4?style=for-the-badge&logo=dotnet)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3-7952B3?style=for-the-badge&logo=bootstrap&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoft&logoColor=white)
![License](https://img.shields.io/badge/License-MIT-22c55e?style=for-the-badge)

**A full-stack cricket league management system built with ASP.NET Core 8, Razor Pages, EF Core, and Bootstrap 5**

[Features](#-features) · [Tech Stack](#-tech-stack) · [Getting Started](#-getting-started) · [Project Structure](#-project-structure) · [Data Model](#-data-model) · [SQL Capabilities](#-sql-capabilities) · [Pages Overview](#-pages-overview)

</div>

---

## 📌 Project Overview

CricketPro League is a production-grade web application that manages a complete T20 cricket league. Users can register, log in, and access team information, player stats, match schedules, league standings, and a powerful search feature — all wrapped in a custom dark cricket-themed UI.

> Built as a final team project demonstrating full-stack .NET development with real-world patterns: Identity-based auth, EF Core ORM, Razor Pages MVVM, relational data modelling, and responsive Bootstrap UI.

---

## ✨ Features

| Feature | Details |
|---|---|
| 🔐 Authentication | Register, login, logout using ASP.NET Core Identity with password hashing |
| 🏏 Teams | View all teams with coach, captain, home ground, win/loss record |
| 👤 Players | Browse players with position filters (Batsman/Bowler/All-Rounder/Keeper) |
| 📅 Matches | Upcoming fixtures and completed results with scores |
| 🏆 Standings | Live league table with computed points, NRR, Win Rate %, W/L/D |
| 📊 Player Stats | Top batsmen & bowlers leaderboards with full statistics |
| 🔍 Smart Search | Dynamic search with filtering across teams, players, matches |
| 🌱 Seed Data | Auto-seeded with 6 teams, 10 players, 8 matches on first run |
| 📱 Responsive | Mobile-first layout using Bootstrap 5 grid |

---

## 🛠️ Tech Stack

### Backend
| Technology | Purpose |
|---|---|
| ASP.NET Core 8 | Web framework |
| C# 12 | Primary language |
| Razor Pages | Page-based MVC pattern |
| Entity Framework Core 8 | ORM / database access |
| ASP.NET Core Identity | Authentication & authorisation |
| **SQL Server** | Relational database (localdb) |

### Frontend
| Technology | Purpose |
|---|---|
| Bootstrap 5.3 | Responsive grid & components |
| HTML5 / CSS3 | Structure & custom styling |
| JavaScript (vanilla) | Search filtering & nav highlight |
| Google Fonts (Bebas Neue + Inter) | Typography |
| Bootstrap Icons | Icon set |

---

## 🚀 Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- **SQL Server** (SQL Server Express or LocalDB)
- Any IDE: Visual Studio 2022, VS Code, or JetBrains Rider

### Installation & Run

```bash
# 1. Clone the repository
git clone https://github.com/YOUR_USERNAME/CricketProLeague.git
cd CricketLeague

# 2. Restore NuGet packages
dotnet restore

# 3. Run the app (database auto-creates with seed data)
dotnet run

# 4. Open your browser
# → https://localhost:5001
# → http://localhost:5000
```

> ✅ **No migrations required.** The SQL Server database is created automatically on first launch using `EnsureCreated()` with pre-seeded data.

### First Login
1. Click **Sign Up** to register a new account
2. Log in with your credentials
3. Explore Teams, Players, Matches, Standings, Stats, and Search

---

## 📁 Project Structure

```
CricketLeague/
│
├── 📁 Data/
│   └── AppDbContext.cs              # EF Core DbContext + seed data
│
├── 📁 Models/
│   └── Models.cs                   # ApplicationUser, Team, Player, Match
│
├── 📁 Pages/
│   ├── 📁 Shared/
│   │   └── _Layout.cshtml          # Master layout (navbar, footer)
│   ├── 📁 Account/
│   │   ├── Login.cshtml / .cs      # Login page
│   │   ├── Register.cshtml / .cs   # Registration page
│   │   └── Logout.cshtml / .cs     # Logout handler
│   ├── 📁 Teams/
│   │   └── Index.cshtml / .cs      # All teams view
│   ├── 📁 Players/
│   │   └── Index.cshtml / .cs      # Players with position filter
│   ├── 📁 Matches/
│   │   └── Index.cshtml / .cs      # Upcoming + completed matches
│   ├── 📁 Standings/
│   │   └── Index.cshtml / .cs      # League table with computed stats
│   ├── 📁 PlayerStats/
│   │   └── Index.cshtml / .cs      # Batting + bowling leaderboards
│   ├── 📁 Search/
│   │   └── Index.cshtml / .cs      # Dynamic search with SQL queries
│   ├── Index.cshtml / .cs          # Home / hero dashboard
│   ├── _ViewImports.cshtml         # Global using directives
│   └── _ViewStart.cshtml           # Default layout assignment
│
├── 📁 wwwroot/
│   ├── css/site.css                # Custom cricket dark theme CSS
│   └── js/site.js                  # Client-side interactivity
│
├── Program.cs                      # App entry point, DI config
├── appsettings.json                # Connection strings
├── CricketLeague.csproj            # Project + NuGet references
└── README.md
```

---

## 🗃️ Data Model

```
ApplicationUser  ──────  ASP.NET Identity
  ├── FullName
  └── CreatedAt

Team  ──────────────────────────────────────────
  ├── Id, Name, Coach, Captain
  ├── Country, HomeGround, LogoEmoji
  ├── Wins, Losses, Draws, NRR
  ├── Points     (computed: Wins×2 + Draws)
  ├── MatchesPlayed (computed: W+L+D)
  └── Players[]  ──── one-to-many ──► Player

Player  ────────────────────────────────────────
  ├── Id, Name, Position, Nationality, Age
  ├── TeamId  ──── many-to-one ──► Team
  ├── Batting:  Runs, BattingAverage, StrikeRate,
  │             Centuries, HalfCenturies, HighestScore, Fours, Sixes
  └── Bowling:  Wickets, BowlingAverage, Economy,
                BestBowling, FiveWickets

Match  ─────────────────────────────────────────
  ├── Id, MatchDate, Venue, Status, MatchType
  ├── Team1Id ──► Team,  Team2Id ──► Team
  ├── Team1Score, Team2Score
  └── Result, WinnerTeamId
```

---

## SQL Capabilities

This project demonstrates advanced SQL Server skills through both **Entity Framework Core** and **raw T-SQL queries**:

### 1. Dynamic Search with Raw SQL
- Parameterized queries using `FromSqlRaw()`
- Dynamic filtering across multiple entities (teams, players, matches)
- Handles empty/missing filter values gracefully

```csharp
// Example: Raw SQL search query
var playerSql = @"
    SELECT p.*, t.Name AS TeamName
    FROM Players p
    INNER JOIN Teams t ON p.TeamId = t.Id
    WHERE p.Name LIKE @searchTerm 
       OR p.Position LIKE @searchTerm 
       OR p.Nationality LIKE @searchTerm";
```

### 2. Computed Standings with JOINs
- Multi-table JOINs to compute standings
- Conditional aggregation using `CASE WHEN`
- Computed columns (Points, Win Rate, Matches Played)

```csharp
// Example: SQL with computed columns
var standingsSql = @"
    SELECT 
        t.Id, t.Name, t.Wins, t.Losses, t.Draws, t.NRR,
        (t.Wins + t.Losses + t.Draws) AS MatchesPlayed,
        (t.Wins * 2 + t.Draws) AS Points,
        CASE 
            WHEN (t.Wins + t.Losses + t.Draws) > 0 
            THEN ROUND(CAST(t.Wins AS FLOAT) / (t.Wins + t.Losses + t.Draws) * 100, 1)
            ELSE 0 
        END AS WinRate
    FROM Teams t
    ORDER BY Points DESC, t.Wins DESC, t.NRR DESC";
```

### 3. Player Statistics with Aggregations
- Aggregate functions: `SUM`, `AVG`, `MAX`, `COUNT`
- `GROUP BY` for position-based statistics
- Window functions (`ROW_NUMBER()`) for ranking

```csharp
// Example: Aggregation with GROUP BY
var statsSql = @"
    SELECT 
        p.Position,
        COUNT(p.Id) AS PlayerCount,
        SUM(p.Runs) AS TotalRuns,
        SUM(p.Wickets) AS TotalWickets,
        AVG(p.BattingAverage) AS AvgBattingAvg,
        MAX(p.HighestScore) AS HighestScore
    FROM Players p
    GROUP BY p.Position
    ORDER BY TotalRuns DESC";
```

### 4. Relational Data Design
- Foreign key relationships (Team → Players, Match → Teams)
- One-to-many and many-to-one relationships
- Proper constraint enforcement at database level

---

## 🔒 Authentication & Authorisation

All data pages require login. Unauthenticated users are redirected to `/Account/Login`.

| Route | Access |
|---|---|
| `/` | Public |
| `/Account/Login` | Public |
| `/Account/Register` | Public |
| `/Teams` `/Players` `/Matches` | 🔐 Authenticated only |
| `/Standings` `/PlayerStats` `/Search` | 🔐 Authenticated only |

---

## 📄 Pages Overview

### 🏠 Home
Hero section with animated cricket ball, league stats counter, and recent match results.

### 🔐 Login / Register
Clean auth forms with error feedback, password validation, and redirect on success.

### 🏏 Teams
Card grid showing all 6 teams — coach, captain, home ground, W/L/D record, and points.

### 👤 Players
Filterable player grid by position (Batsman / Bowler / All-Rounder / Wicket-Keeper) with batting and bowling stat pills.

### 📅 Matches
Split view: upcoming fixtures (date, venue, teams) and completed results (scores, result string, winner highlighted).

### 🏆 Standings
Full league table with rank medals, computed Win Rate %, NRR, and all stats. Sorted by Points → Wins → NRR.

### 📊 Player Stats
Top 10 batsmen and all active bowlers in detailed leaderboard tables, plus summary highlight cards showing aggregate statistics.

### 🔍 Search
Keyword search across all entities with filter buttons (All / Teams / Players / Matches). Uses raw SQL for dynamic query building.

---

## 🤝 Contributing

1. Fork the repo
2. Create a feature branch: `git checkout -b feature/my-feature`
3. Commit: `git commit -m "Add my feature"`
4. Push: `git push origin feature/my-feature`
5. Open a Pull Request

---

## 📜 License

MIT License — see [LICENSE](LICENSE) for details.

---

<div align="center">
  <strong>🏏 CricketPro League · Built with ASP.NET Core 8 · Where Champions Are Made</strong>
</div>