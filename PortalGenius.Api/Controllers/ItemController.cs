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

    public ItemController(IArcGISService arcGISService)
    {
        _argGISService = arcGISService;
    }

    [HttpGet("/")]
    public async Task<IActionResult> GetAllItems()
    {
        return Ok(await _argGISService.GetAllItemsAsync());
    }

    [HttpGet("/allDataParallel")]
    public async Task<List<object>> GetDataFromItems()
    {
        var result = new List<object>();
        List<string> items = await GetItemIds();

        await items.ParallelForEachAsync(async item =>
        {
            var response = await _argGISService.GetDataFromItemAsync(item);
            //Console.WriteLine(response);
            result.Add(response);

        }, maxDegreeOfParallelism: 10);

        return result;
    }

    [HttpGet("/allDataSequential")]
    public async Task<List<object>> GetDataFromItemsSequential()
    {
        var result = new List<object>();
        List<string> items = await GetItemIds();

        foreach (var item in items)
        {
            var response = await _argGISService.GetDataFromItemAsync(item);
            //Console.WriteLine(response);
            result.Add(response);
        }

        return result;
    }

    private async Task<List<string>> GetItemIds()
    {
        var result = new List<string>();
        var items = await _argGISService.GetAllItemsAsync();
        Parallel.ForEach(items.Results, item =>
        {
            result.Add(item.Id);
        });
        return result.ToList();
    }
}

