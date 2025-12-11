using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

public class EmployeeService
{
    private readonly AppDbContext _context;
    private readonly IMemoryCache _cache;

    public EmployeeService(AppDbContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<PaginatedResult<EmployeeResponseDto>> GetPagedEmployeesAsync(
        int pageNumber,
        int pageSize,
        string sortBy,
        string sortDirection)
    {
        string cacheKey = $"employees_{pageNumber}_{pageSize}_{sortBy}_{sortDirection}";

        if (_cache.TryGetValue(cacheKey, out PaginatedResult<EmployeeResponseDto> cachedData))
            return cachedData;

        var query = _context.Employees
                    .Include(e => e.Leaderboards)
                    .ThenInclude(lb => lb.LeaderboardChallenges)
                    .ThenInclude(lc => lc.Challenge)
                    .AsQueryable();

        int totalRecords = await query.CountAsync();

        query = query.ApplySorting(sortBy, sortDirection);
        query = query.ApplyPagination(pageNumber, pageSize);

        var data = await query.ToListAsync();

        var dtoData = data.Select(e => new EmployeeResponseDto
{
    EmployeeId = e.EmployeeId,
    Name = e.Name,
    Leaderboards = e.Leaderboards.Select(lb => new LeaderboardResponseDto
    {
        LeaderboardId = lb.LeaderboardId,
        Score = lb.Score,
        Challenges = lb.LeaderboardChallenges.Select(lc => new ChallengeResponseDto
        {
            ChallengeId = lc.Challenge.ChallengeId,
            ChallengeName = lc.Challenge.ChallengeName,
            ChallengeType = lc.Challenge.ChallengeType
        }).ToList()

    }).ToList()

}).ToList();

        var result = new PaginatedResult<EmployeeResponseDto>
        {
            Data = dtoData,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalRecords = totalRecords
        };

        _cache.Set(cacheKey, result, TimeSpan.FromMinutes(5));

        return result;
    }
}
