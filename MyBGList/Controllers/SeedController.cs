using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBGList.Models;

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
}
