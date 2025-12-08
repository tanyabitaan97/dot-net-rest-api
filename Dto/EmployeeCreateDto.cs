namespace dotnet.Dto {
public class EmployeeCreateDto
{
    public string Name { get; set; }
    public List<LeaderboardCreateDto> Leaderboards { get; set; }
}
}
