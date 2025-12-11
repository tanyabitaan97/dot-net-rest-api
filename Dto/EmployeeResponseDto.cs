public class EmployeeResponseDto
{
    public int EmployeeId { get; set; }
    public string Name { get; set; }
    public List<LeaderboardResponseDto> Leaderboards { get; set; }
}
