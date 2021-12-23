using Microsoft.AspNetCore.Mvc;
using PortalGenius.Core.Services;
using Dasync.Collections;
using Microsoft.AspNetCore.Mvc.Abstractions;
using PortalGenius.Core.Models;

namespace PG_API.Controllers;

[Route("[controller]")]
[ApiController]
public class ItemController : ControllerBase
{
    private readonly IArcGISService _argGISService;
    //Test lijst met item IDs
    private List<string> itemIDs = new List<string>()
    {
        "594f514df776476ab58345fc09bdaba0", "83c0c9609f644d1687af837d2c5f13d1", "6e1cbb6a28094cec9cf628acf39a2d25"
    };


    public ItemController(IArcGISService arcGISService)
    {
        _argGISService = arcGISService;
    }

    [HttpGet("/")]
    public async Task<IActionResult> GetAllItems()
    {
        return Ok(await _argGISService.GetAllItemsAsync());
    }

    //[HttpGet]
    //[Route("{item_id}/data")]
    //public async Task<IActionResult> GetDataFromItem(string item_id)
    //{
    //    string data = await _argGISService.GetDataFromItem(item_id);
    //    return Ok(data);
    //}


    //public object GetDataFromItemsThread(List<string> items)
    //{
    //    //Lijst van testIDs
    //    items = itemIDs;
    //    //loop door al de IDs en haal data hiervan op
    //    var test = Parallel.ForEach(items, async i =>  await this.GetDataFromItem(i));
    //    return Ok(test);
    //}

    [HttpGet("/alledata")]
    public async Task<IEnumerable<object>> GetDataFromItems(/*List<string> items*/)
    {
        var result = new List<object>();
        var items = itemIDs;
        await items.ParallelForEachAsync(async item =>
        {
            var response = await _argGISService.GetDataFromItem(item);
            //Console.WriteLine(response);
            result.Add(response);
        }, maxDegreeOfParallelism: 10);
        return result;
    }

    [HttpGet("/alleItemIds")]
    public async Task<IEnumerable<string>> GetItemIds()
    {
        var result = new List<string>();
        var items = await _argGISService.GetAllItems();
        Parallel.ForEach(items.Results, item =>
        {
            result.Add(item.Id);
        });
        return result;

//    "6e1cbb6a28094cec9cf628acf39a2d25",
//    "83c0c9609f644d1687af837d2c5f13d1",
//    "218ce37846ac43119a4c0140c91066d4",
//    "43935cbc6fe74cc087b675bf8021b313",
//    "ba31a93eea334386bd5263be0c0337c1",
//    "0d1d547d554e4d48b128e797a4095998",
//    "97eb8ea2be41421fb5284c9eea454863",
//    "d5ca5559014844a99d2631dc6f219237",
//    "787d67bc9cae4d8fa7a438134e6eede9",
//    "594f514df776476ab58345fc09bdaba0"
    }
}
