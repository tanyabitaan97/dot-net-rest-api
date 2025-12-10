using Microsoft.EntityFrameworkCore;
using dotnet.Models;

public class AppDbContext : DbContext
{
     //this constructor is important to avoid this error
    //AddDbContext was called with configuration, but the context type 'AppDbContext'
   //only declares a parameterless constructor. AppDbContext should declare a constructor 
  //that accepts DbContextOptions<AppDbContext> and pass it to base constructor.
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Leaderboard> Leaderboards { get; set; }
    public DbSet<Challenge> Challenges { get; set; }

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<Leaderboard>()
        .HasOne(lb => lb.Employee)
        .WithMany(emp => emp.Leaderboards)
        .HasForeignKey(lb => lb.EmployeeId);


    modelBuilder.Entity<LeaderboardChallenge>()
        .HasKey(lc => new { lc.LeaderboardId, lc.ChallengeId });

    modelBuilder.Entity<LeaderboardChallenge>()
        .HasOne(lc => lc.Leaderboard)
        .WithMany(lb => lb.LeaderboardChallenges)
        .HasForeignKey(lc => lc.LeaderboardId);

    modelBuilder.Entity<LeaderboardChallenge>()
        .HasOne(lc => lc.Challenge)
        .WithMany(ch => ch.LeaderboardChallenges)
        .HasForeignKey(lc => lc.ChallengeId);
}

}
