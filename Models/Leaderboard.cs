using System.Text.Json.Serialization;

public class Leaderboard
{
    public int LeaderboardId { get; set; }
    public int EmployeeId { get; set; }

    public int Score { get; set; }

    public Employee Employee { get; set; }

    public List<LeaderboardChallenge> LeaderboardChallenges { get; set; } = new();
}

