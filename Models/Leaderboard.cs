using System.Text.Json.Serialization;

public class Leaderboard
{
    public int LeaderboardId { get; set; }
    public int Score { get; set; }

    public int EmployeeId { get; set; }
     [JsonIgnore] 
    public Employee? Employee { get; set; }

    public List<Challenge> Challenges { get; set; } = new();
}
