using System.Collections.Generic;
namespace RockPaperScissors;
public class Players
{
    public string Name { get; set; } = "";
    // Game statistics
    public int GamesPlayed { get; set; }
    public int MatchNumber { get; set; }
    public int Wins { get; set; }
    public int TotalWins { get; set; }
    public int Losses { get; set; }
    public int TotalLosses { get; set; }
    public int Draws { get; set; }
    public int TotalDraws { get; set; }
    // Win streaks
    public int BestWinStreak { get; set; }
    public int CurrentWinStreak { get; set; }
    // Player choices
    public int RockCount { get; set; }
    public int PaperCount { get; set; }
    public int ScissorsCount { get; set; }
    // Achievements earned by the player
    public List<string> Achievements { get; set; } = new();
    // Match history
    public List<string> MatchHistory { get; set; } = new();
}