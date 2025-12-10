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

 

}
