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
        var users = await _arcGISService.GetAllUsersAsync();

        return Ok(users);
    }
}
