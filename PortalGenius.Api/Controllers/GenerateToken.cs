using Microsoft.AspNetCore.Mvc;
using PortalGenius.Core.Services;

namespace PG_API.Controllers;

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
    public async Task<IActionResult> GenerateNewToken()
    {
        return Ok(await _arcGisService.GetGenToken());
    }

    [HttpPut("/{username}/{password}")]
    public async Task<IActionResult> SetNewCreds(string username, string password)
    {
        var obj = await _arcGisService.GetGenTokenWithNewCreds(username, password);
        try
        {
            if (obj.Token.Equals("null"))
            {
                return Problem("could not set new username/password");
            }
        }
        catch (NullReferenceException e)
        {
            return Problem("could not set new username/password");
        }
        return Ok(obj);
    }
}
