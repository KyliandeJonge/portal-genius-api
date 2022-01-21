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
        return !string.IsNullOrEmpty(obj.Token) ? Ok(obj) : Problem("could not set new username/password");
    }
}
