using log4net;
using log4net.Config;
using log4net.Repository.Hierarchy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Log4Net.AspNetCore.Extensions;
using PG_API.Handlers;
using PG_API.Services;

namespace PG_API.Controllers;

[Route("[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ArcGISService _arcGISService;

    public UserController(ArcGISService arcGISService)
    {
        _arcGISService = arcGISService;
    }


    [HttpPost()]
    public async Task<IActionResult> Post()
    {
        return Ok(await _arcGISService.GetUsers());
    }
}
