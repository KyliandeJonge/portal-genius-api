using Microsoft.AspNetCore.Mvc;
using PortalGenius.Core.Services;

namespace PG_API.Controllers;

[Route("[controller]")]
[ApiController]
public class ItemController : ControllerBase
{
    private readonly IArcGISService _argGISService;

    public ItemController(IArcGISService arcGISService)
    {
        _argGISService = arcGISService;
    }

    [HttpGet("/")]
    public async Task<IActionResult> GetAllItems()
    {
        return Ok(await _argGISService.GetAllItemsAsync());
    }

    [HttpGet]
    [Route("{item_id}/data")]
    public async Task<IActionResult> GetDataFromItem(string item_id)
    {
        return Ok(await _argGISService.GetDataFromItem(item_id));
    }
}
