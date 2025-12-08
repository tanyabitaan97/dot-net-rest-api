namespace dotnet.Dto
{
    public class LeaderboardCreateDto
    {
        public int Score { get; set; }
        public List<ChallengeCreateDto> Challenges { get; set; } = new();
    }
}
