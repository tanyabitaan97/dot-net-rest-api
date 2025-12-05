public class Challenge
{
    public int ChallengeId { get; set; }
    public string ChallengeName { get; set; }
    public string ChallengeType { get; set; }

    //depicts many to many
    public ICollection<Leaderboard> Leaderboards { get; set; } = new List<Leaderboard>();
}
