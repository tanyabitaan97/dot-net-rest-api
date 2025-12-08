using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dotnet.Models;
using dotnet.Dto;
using Microsoft.AspNetCore.Authorization;


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

    [HttpPost("create-full")]
    public async Task<IActionResult> CreateFull(EmployeeCreateDto dto)
    {
        var employee = new Employee
        {
            Name = dto.Name
        };

        foreach (var lbDto in dto.Leaderboards)
        {
            var leaderboard = new Leaderboard
            {
                Score = lbDto.Score,
                Employee = employee
            };


            foreach (var chDto in lbDto.Challenges)
            {
                var challenge = new Challenge
                {
                    ChallengeName = chDto.ChallengeName,
                    ChallengeType = chDto.ChallengeType
                };

                leaderboard.Challenges.Add(challenge);
            }

            employee.Leaderboards.Add(leaderboard);
        }

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        return Ok(employee);
    }

}
