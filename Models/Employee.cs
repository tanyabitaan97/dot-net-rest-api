public class Employee
{
    public int EmployeeId { get; set; }
    public string Name { get; set; }

    // one to many
    public ICollection<Leaderboard> Leaderboards { get; set; } = new List<Leaderboard>();
}
