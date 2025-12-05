public class Leaderboard
{
    public int LeaderboardId { get; set; }
    public int Score { get; set; }

    //depicts one to many
    public int EmployeeId { get; set; }
    public Employee? Employee { get; set; }

    //depicts many to many
    public ICollection<Challenge> Challenges { get; set; } = new List<Challenge>();
}
