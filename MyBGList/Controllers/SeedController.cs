using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBGList.Models;
using MyBGList.Models.Csv;
using System.Globalization;

namespace MyBGList.Controllers;

[Route("[controller]")]
[ApiController]
public class SeedController : ControllerBase
{
    private readonly AppDbContext _context;

    private readonly IWebHostEnvironment _env;

    private readonly ILogger<SeedController> _logger;

    public SeedController(AppDbContext context, IWebHostEnvironment env, ILogger<SeedController> logger)
    {
        _context = context;
        _env = env;
        _logger = logger;
    }

    [HttpPut(Name = "Seed")]
    [ResponseCache(NoStore = true)]
    public async Task<JsonResult> PutAsync(int? id = null)
    {
        CsvConfiguration config = new (CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            Delimiter = ";",
        };
        using StreamReader reader = new(Path.Combine(_env.ContentRootPath, "Data/bgg_dataset.csv"));
        using CsvReader csv = new (reader, config);
        Dictionary<int, BoardGame> existingBoardGames = await _context.BoardGames.ToDictionaryAsync(k => k.Id);
        Dictionary<string, Domain> existingDomains = await _context.Domains.ToDictionaryAsync(k => k.Name);
        Dictionary<string, Mechanic> existingMechanics = await _context.Mechanics.ToDictionaryAsync(k => k.Name);
        DateTime now = DateTime.Now;

        
        IEnumerable<BggRecord> records = csv.GetRecords<BggRecord>();
        int skippedRows = 0;
        foreach (BggRecord record in records)
        {
            if (!record.ID.HasValue 
                || string.IsNullOrEmpty(record.Name) 
                || existingBoardGames.ContainsKey(record.ID.Value) 
                || (id.HasValue && id.Value != record.ID.Value))
            {
                skippedRows++;
                continue;
            }
            BoardGame boardGame = new()
            {
                Id = record.ID.Value,
                Name = record.Name,
                BGGRank = record.BGGRank ?? 0,
                ComplexityAverage = record.ComplexityAverage ?? 0,
                MaxPlayers = record.MaxPlayers ?? 0,
                MinAge = record.MinAge ?? 0,
                MinPlayers = record.MinPlayers ?? 0,
                OwnedUsers = record.OwnedUsers ?? 0,
                PlayTime = record.PlayTime ?? 0,
                RatingAverage = record.RatingAverage ?? 0,
                UsersRelated = record.UsersRated ?? 0,
                Year = record.YearPublished ?? 0,
                CreatedDate = now,
                LastModifiedDate = now,
            };
            _context.BoardGames.Add(boardGame);

            if(!string.IsNullOrEmpty(record.Domains))
                foreach(var domainName in record.Domains.Split(",", StringSplitOptions.TrimEntries).Distinct(StringComparer.InvariantCultureIgnoreCase))
                {
                    Domain? domain = existingDomains.GetValueOrDefault(domainName);
                    if(domain == null)
                    {
                        domain = new()
                        {
                            Name = domainName,
                            CreatedDate = now,
                            LastModifiedDate = now
                        };
                        _context.Domains.Add(domain);
                        existingDomains.Add(domainName, domain);
                    }
                    _context.BoardGames_Domains.Add(new ()
                    {
                        BoardGame = boardGame,
                        Domain = domain,
                        CreatedDate = now
                    });
                }

            if(!string.IsNullOrEmpty(record.Mechanics))
                foreach(string? mechanicName in record.Mechanics.Split(",", StringSplitOptions.TrimEntries).Distinct(StringComparer.InvariantCultureIgnoreCase))
                {
                    Mechanic? mechanic = existingMechanics.GetValueOrDefault(mechanicName);
                    if(mechanic == null)
                    {
                        mechanic = new()
                        {
                            Name = mechanicName,
                            CreatedDate = now,
                            LastModifiedDate = now
                        };
                        _context.Mechanics.Add(mechanic);
                        existingMechanics.Add(mechanicName, mechanic);
                    }
                    _context.BoardGames_Mechanics.Add(new()
                    {
                        BoardGame = boardGame,
                        Mechanic = mechanic,
                        CreatedDate= now
                    });
                }
        }

        string sqlQuery = $"SET IDENTITY_INSERT {typeof(BoardGame).Name}s";

        using var transaction = _context.Database.BeginTransaction();
        _context.Database.ExecuteSqlRaw($"{sqlQuery} ON");
        await _context.SaveChangesAsync();
        _context.Database.ExecuteSqlRaw($"{sqlQuery} OFF");
        transaction.Commit();

        return new JsonResult(new
        {
            BoardGames = _context.BoardGames.Count(),
            Domains = _context.Domains.Count(),
            Mechanics = _context.Mechanics.Count(),
            SkippedRows = skippedRows
        });
    }
}