using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using PortalGenius.Core.Services;

namespace PG_API.Controllers;

[Route("[controller]")]
[ApiController]
public class ItemController : ControllerBase
{
    private readonly ArcGISService _argGISService;

    public ItemController(ArcGISService arcGISService)
    {
        _argGISService = arcGISService; 
    }

    [HttpGet]
    public async Task<IActionResult> GetAllItems()
    {
        return Ok(await _argGISService.GetAllItems());
    }
}
