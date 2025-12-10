using System.Text.Json.Serialization;

public class Challenge
{
    public int ChallengeId { get; set; }
    public string? ChallengeName { get; set; }
    public string? ChallengeType { get; set; }

    public List<LeaderboardChallenge> LeaderboardChallenges { get; set; } = new();
}
