using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dotnet.Models;
using Microsoft.AspNetCore.Authorization;


[ApiController]
[Route("api/challenge")]
public class ChallengeController : ControllerBase
{
    private readonly AppDbContext _context;

    public ChallengeController(AppDbContext context)
    {
        _context = context;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(Challenge challenge)
    {
        _context.Challenges.Add(challenge);
        await _context.SaveChangesAsync();
        return Ok(challenge);
    }

    [Authorize]   
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var list = await _context.Challenges
            .Include(c => c.Leaderboards)
            .ToListAsync();

        return Ok(list);
    }
}
