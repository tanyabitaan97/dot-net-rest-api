using System.Text.Json.Serialization;

public class Challenge
{
    public int ChallengeId { get; set; }


    public int LeaderboardId { get; set; }

    public string ChallengeName { get; set; }
    public string ChallengeType { get; set; }


    public Leaderboard Leaderboard { get; set; }
}
