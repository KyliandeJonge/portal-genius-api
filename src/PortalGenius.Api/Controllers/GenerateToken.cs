using Microsoft.AspNetCore.Mvc;
using PortalGenius.Core.Services;

namespace PortalGenius.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class GenerateToken : Controller
{
    private readonly IArcGISService _arcGisService;

    public GenerateToken(IArcGISService arcGisService)
    {
        _arcGisService = arcGisService;
    }

    [HttpGet("/generate")]
    private async Task<IActionResult> GenerateNewToken()
    {
        return Ok(await _arcGisService.GetGenTokenAsync());
    }

    [HttpPut("/{username}/{password}")]
    public async Task<IActionResult> SetNewCreds(string username, string password)
    {
        var obj = await _arcGisService.GetGenTokenWithNewCredsAsync(username, password);
        try
        {
            if (obj.Token.Equals("null"))
            {
                return Problem("could not set new username/password");
            }
        }
        // 12-01-2022 MME: zie eerder opmerking, geen teksten terug geven met "null" en ook geen try-catch voor nullreference exceptions
        catch (NullReferenceException)
        {
            return Problem("could not set new username/password");
        }
        return Ok(obj);
    }
}
