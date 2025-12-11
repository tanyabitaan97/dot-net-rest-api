using System.Text.Json.Serialization;

public class LeaderboardChallenge
{
    public int LeaderboardId { get; set; }
    [JsonIgnore]
    public Leaderboard Leaderboard { get; set; }

    public int ChallengeId { get; set; }

    public Challenge Challenge { get; set; }
}
