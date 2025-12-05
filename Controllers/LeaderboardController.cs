using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dotnet.Models;
using Microsoft.AspNetCore.Authorization;


[ApiController]
[Route("api/leaderboard")]
public class LeaderboardController : ControllerBase
{
    private readonly AppDbContext _context;

    public LeaderboardController(AppDbContext context)
    {
        _context = context;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(Leaderboard leaderboard)
    {
        _context.Leaderboards.Add(leaderboard);
        await _context.SaveChangesAsync();
        return Ok(leaderboard);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var list = await _context.Leaderboards
            .Include(l => l.Employee)
            .Include(l => l.Challenges)
            .ToListAsync();

        return Ok(list);
    }

    [Authorize]
    //Many to Many
    [HttpPost("{leaderboardId}/assign-challenge/{challengeId}")]
    public async Task<IActionResult> AssignChallenge(int leaderboardId, int challengeId)
    {
        var leaderboard = await _context.Leaderboards
            .Include(l => l.Challenges)
            .FirstOrDefaultAsync(l => l.LeaderboardId == leaderboardId);

        var challenge = await _context.Challenges.FindAsync(challengeId);

        if (leaderboard == null || challenge == null)
            return NotFound("Invalid IDs");

        leaderboard.Challenges.Add(challenge);
        await _context.SaveChangesAsync();

        return Ok("Challenge assigned successfully");
    }
}
