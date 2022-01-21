using Microsoft.AspNetCore.Mvc;
using PortalGenius.Core.Services;
using Dasync.Collections;
using Microsoft.AspNetCore.Mvc.Abstractions;
using PortalGenius.Core.Models;
using System.Diagnostics;
using System.Collections.Concurrent;
using PortalGenius.Core.Interfaces;

namespace PortalGenius.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class ItemController : ControllerBase
{
    private readonly IArcGISService _argGISService;
    private readonly IRepository<Item> _itemRepository;

    public ItemController(IArcGISService arcGISService, IRepository<Item> itemRepository)
    {
        _argGISService = arcGISService;
        _itemRepository = itemRepository;
    }

    [HttpGet("/")]
    public async Task<IActionResult> GetAllItems()
    {

        return Ok(await _itemRepository.GetAllAsync());
    }

    [HttpGet("/allDataParallel")]
    public async Task<ConcurrentBag<object>> GetDataFromItems()
    {
        var result = new ConcurrentBag<object>();
        ConcurrentBag<string> items = await GetItemIds();

        var stopwatch = Stopwatch.StartNew();


        await items.ParallelForEachAsync(async item =>
        {
            var response = await _argGISService.GetDataFromItemAsync(item);
                //Console.WriteLine(response);
                result.Add(response);

        }, maxDegreeOfParallelism: 10);

        stopwatch.Stop();
        var time = stopwatch.ElapsedMilliseconds;
        Console.WriteLine("#################################\nTijd: " + time + " ms\n#################################");
        stopwatch.Reset();

        return result;
    }

    [HttpGet("/allDataSequential")]
    public async Task<ConcurrentBag<object>> GetDataFromItemsSequential()
    {
        var result = new ConcurrentBag<object>();
        ConcurrentBag<string> items = await GetItemIds();

        var stopwatch = Stopwatch.StartNew();

        foreach (var item in items)
        {
            var response = await _argGISService.GetDataFromItemAsync(item);
            //Console.WriteLine(response);
            result.Add(response);
        }
        stopwatch.Stop();
        var time = stopwatch.ElapsedMilliseconds;
        Console.WriteLine("#################################\nTijd: " + time + " ms\n#################################");
        stopwatch.Reset();


        return result;
    }

    private async Task<ConcurrentBag<string>> GetItemIds()
    {
        var result = new ConcurrentBag<string>();
        var items = await _itemRepository.GetAllAsync();

        Parallel.ForEach(items, item =>
        {
            result.Add(item.Id);
        });
        return result;
    }

    [HttpGet("{item_id}/data")]
    public async Task<IActionResult> GetDataFromItem(string item_id)
    {
        return Ok(await _argGISService.GetDataFromItemAsync(item_id));
    }
}

