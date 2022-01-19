using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalGenius.Infrastructure.Data;

namespace PortalGenius.WPF.Controllers;

public class TestController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public TestController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("/")]
    public async Task<IActionResult> Demo()
    {
        var test = await _dbContext.Items.ToListAsync();

        return Ok("Dit is gevonden");
    }
}
