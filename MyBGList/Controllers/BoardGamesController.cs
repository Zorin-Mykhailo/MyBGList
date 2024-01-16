using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBGList.DTO;
using MyBGList.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MyBGList.Controllers;
[Route("[controller]")]
[ApiController]
public class BoardGamesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<BoardGamesController> _logger;

    public BoardGamesController(AppDbContext appDbContext, ILogger<BoardGamesController> logger)
    {
        _context = appDbContext;
        _logger = logger;
    }

    [HttpGet(Name = "GetBoardGames")]
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 60)]
    public async Task<RestDTO<BoardGame[]>> GetAsync(int pageIndex = 0, int pageSize = 10, string? sortColumn = "Name", string? sortOrder = "ASC", string? nameFilter = null)
    {
        var query = _context.BoardGames.AsQueryable();
        int? filteredRecordsCount = null;
        if (!string.IsNullOrWhiteSpace(sortColumn))
        {
            if(!string.IsNullOrWhiteSpace(nameFilter) && string.Equals(sortColumn, "Name", StringComparison.InvariantCultureIgnoreCase))
            {
                query = query.Where(b => b.Name.Contains(nameFilter));
                filteredRecordsCount = await query.CountAsync();
            }
            query = query.OrderBy($"{sortColumn} {sortOrder ?? "ASC"}");
        }
        query = query
            .Skip(pageIndex * pageSize)
            .Take(pageSize);

        return new RestDTO<BoardGame[]>
        {
            Data = await query.ToArrayAsync(),
            PageIndex = pageIndex,
            PageSize = pageSize,
            RecordCount = filteredRecordsCount ?? await _context.BoardGames.CountAsync(),
            Links = [new(Url.Action(null, "BoardGame", new { pageIndex, pageSize }, Request.Scheme)!, "self", "GET")]
        };
    }
}