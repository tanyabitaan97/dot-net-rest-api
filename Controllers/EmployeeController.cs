using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dotnet.Models;
using dotnet.Dto;
using Microsoft.AspNetCore.Authorization;
using EFCore.BulkExtensions;



[ApiController]
[Route("api/employee")]
public class EmployeeController : ControllerBase
{
    private readonly AppDbContext _context;

    public EmployeeController(AppDbContext context)
    {
        _context = context;
    }

    [Authorize]  
    [HttpPost]
    public async Task<IActionResult> Create(Employee employee)
    {
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
        return Ok(employee);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var employees = await _context.Employees
            .Include(e => e.Leaderboards)
            .ToListAsync();

        return Ok(employees);
    }


[HttpPost("create-full-bulk")]
public async Task<IActionResult> CreateFullBulk(List<EmployeeCreateDto> dtos)
{
    var employees = dtos.Select(dto => new Employee
    {
        Name = dto.Name,
        Leaderboards = dto.Leaderboards.Select(lbDto => new Leaderboard
        {
            Score = lbDto.Score,

            LeaderboardChallenges = lbDto.Challenges.Select(chDto =>
{
    if (chDto.ChallengeId.HasValue)
    {
        // Link to existing challenge
        return new LeaderboardChallenge
        {
            ChallengeId = chDto.ChallengeId.Value
        };
    }
    else
    {
        // Create a new challenge row and link to it
        return new LeaderboardChallenge
        {
            Challenge = new Challenge
            {
                ChallengeName = chDto.ChallengeName!,
                ChallengeType = chDto.ChallengeType!
            }
        };
    }
}).ToList()

        }).ToList()

    }).ToList();

    await _context.BulkInsertAsync(
        employees,
        new BulkConfig
        {
            IncludeGraph = true,
            SetOutputIdentity = true
        }
    );

    return Ok("Bulk Insert Successful");
}


}
