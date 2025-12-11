public class LeaderboardResponseDto
{
    public int LeaderboardId { get; set; }
    public int Score { get; set; }
    public List<ChallengeResponseDto> Challenges { get; set; }
}
