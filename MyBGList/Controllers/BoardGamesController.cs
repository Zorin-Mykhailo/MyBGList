﻿using System.ComponentModel.DataAnnotations;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBGList.ValidationAttributes;
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


    [HttpGet(Name = "GetBoardGames"), ResponseCache(Location = ResponseCacheLocation.Any, Duration = 60)]
    public async Task<RestDTO<BoardGame[]>> GetAsync(
        int pageIndex = 0, 
        [Range(1, 100)] 
        int pageSize = 10,
        [NameOfProperty(typeof(BoardGameDTO))]
        string? sortColumn = "Name",
        [AllowedStrings(["ASC", "DESC"])]
        string? sortOrder = "ASC", 
        string? nameFilter = null)
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
        BoardGame? boardGame = await _context.BoardGames.Where(e => e.Id == model.Id).FirstOrDefaultAsync();
        if(boardGame != null)
        {
            if(!string.IsNullOrEmpty(model.Name))
                boardGame.Name = model.Name;
            if(model.Year.HasValue && model.Year.Value > 0)
                boardGame.Year = model.Year.Value;
            if(model.MinPlayers.HasValue && model.MinPlayers > 0)
                boardGame.MinPlayers = model.MinPlayers.Value;
            if(model.MaxPlayers.HasValue && model.MaxPlayers > 0)
                boardGame.MaxPlayers = model.MaxPlayers.Value;
            if(model.PlayTime.HasValue && model.PlayTime.Value > 0)
                boardGame.PlayTime = model.PlayTime.Value;
            if(model.MinAge.HasValue &&  model.MinAge.Value > 0)
                boardGame.MinAge = model.MinAge.Value;

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


    [HttpDelete(Name = "DeleteBoardGame"), ResponseCache(NoStore = true)]
    public async Task<RestDTO<BoardGame[]?>> DeleteAsync(int[] id)
    {
        HashSet<int> ids = new (id);
        BoardGame[] boardGames = await _context.BoardGames.Where(e => ids.Contains(e.Id)).ToArrayAsync();

        if(boardGames.Any())
        {
            _context.BoardGames.RemoveRange(boardGames);
            await _context.SaveChangesAsync();
        }

        //BoardGame? boardGame = await _context.BoardGames.Where(e => e.Id == id).FirstOrDefaultAsync();
        //if (boardGame != null)
        //{
        //    _context.BoardGames.Re.Remove(boardGame);
        //    await _context.SaveChangesAsync();
        //}

        return new RestDTO<BoardGame[]?>()
        {
            Data = boardGames,
            RecordCount = boardGames?.Length ?? 0,
            Links = [new(Url.Action(null, "BoardGames", id, Request.Scheme)!, "self", "DELETE")]
        };
    }
}