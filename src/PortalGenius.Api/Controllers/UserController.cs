using Microsoft.AspNetCore.Mvc;
using PortalGenius.Core.Services;

namespace PortalGenius.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IArcGISService _arcGISService;

    public UserController(IArcGISService arcGISService)
    {
        _arcGISService = arcGISService;
    }

    [HttpPost()]
    public async Task<IActionResult> GetAllUsers()
    {
        // 12-01-2022 MME: gewoon 2 regels doen
        // 13-01-2022 MS: goed punt, dit is leesbaarder zo en biedt iets meer debugmogelijkheden
        var users = await _arcGISService.GetAllUsersAsync();

        return Ok(users);
    }
}
