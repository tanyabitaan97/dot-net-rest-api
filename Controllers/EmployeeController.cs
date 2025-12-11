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
    private readonly EmployeeService _employeeService;

    public EmployeeController(AppDbContext context,EmployeeService employeeService)
    {
        _context = context;
        _employeeService = employeeService;
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

[HttpGet("employees")]
public async Task<IActionResult> GetEmployees(
    int pageNumber = 1,
    int pageSize = 10,
    string sortBy = "EmployeeId",
    string sortDirection = "asc")
{
    var result = await _employeeService.GetPagedEmployeesAsync(
        pageNumber, pageSize, sortBy, sortDirection);

    return Ok(result);
}

}
