using Microsoft.AspNetCore.Mvc;
using PortalGenius.Core.Services;

namespace PG_API.Controllers;

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
        return Ok(await _arcGISService.GetAllUsersAsync());
    }
}
