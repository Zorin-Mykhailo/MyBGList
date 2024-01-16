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


    [HttpGet(Name = "GetBoardGames"), ResponseCache(Location = ResponseCacheLocation.Any, Duration = 60)]
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


    [HttpPost(Name = "UpdateBoardGame"), ResponseCache(NoStore = true)]
    public async Task<RestDTO<BoardGame?>> PostAsync(BoardGameDTO model)
    {
        var boardGame = await _context.BoardGames.Where(e => e.Id == model.Id).FirstOrDefaultAsync();
        if(boardGame != null)
        {
            if(!string.IsNullOrEmpty(model.Name))
                boardGame.Name = model.Name;
            if(model.Year.HasValue && model.Year.Value > 0)
                boardGame.Year = model.Year.Value;
            boardGame.LastModifiedDate = DateTime.Now;
            _context.BoardGames.Update(boardGame);
            await _context.SaveChangesAsync();
        }

        return new RestDTO<BoardGame?>()
        {
            Data = boardGame,
            Links = [new(Url.Action(null, "BoardGames", model, Request.Scheme)!, "self", "POST")]
        };
    }
}