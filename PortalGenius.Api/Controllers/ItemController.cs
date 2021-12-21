using Microsoft.AspNetCore.Mvc;
using PortalGenius.Core.Services;

namespace PG_API.Controllers;

[Route("[controller]")]
[ApiController]
public class ItemController : ControllerBase
{
    private readonly ArcGISService _argGISService;
    //Test lijst met item IDs
    private List<string> itemIDs = new List<string>()
    {
        "594f514df776476ab58345fc09bdaba0", "83c0c9609f644d1687af837d2c5f13d1", "6e1cbb6a28094cec9cf628acf39a2d25"
    };
    

    public ItemController(ArcGISService arcGISService)
    {
        _argGISService = arcGISService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllItems()
    {
        return Ok(await _argGISService.GetAllItems());
    }

    [HttpGet]
    [Route("{item_id}/data")]
    public async Task<IActionResult> GetDataFromItem(string item_id)
    {
        string data = await _argGISService.GetDataFromItem(item_id);
        return Ok(data);
    }

    [HttpPost]
    [Route("/alldata")]
    public async Task<IActionResult> GetDataFromItems(List<string> items)
    {
        //Lijst van testIDs
        items = itemIDs;
        //loop door al de IDs en haal data hiervan op
        var test = Parallel.ForEach(items, async i =>  await this.GetDataFromItem(i));
        return Ok(test);
    }


}
