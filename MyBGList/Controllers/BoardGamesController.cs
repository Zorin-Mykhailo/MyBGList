using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBGList.DTO;
using MyBGList.Models;

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
    public async Task<RestDTO<BoardGame[]>> GetAsync()
    {
        DbSet<BoardGame> query = _context.BoardGames;

        return new RestDTO<BoardGame[]>
        {
            Data = await query.ToArrayAsync(),
            Links = [ new (Url.Action(null, "BoardGame", null, Request.Scheme)!, "self", "GET") ]
        };
    }
}