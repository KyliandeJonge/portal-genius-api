using Microsoft.AspNetCore.Mvc;
using PortalGenius.Core.Services;
using Dasync.Collections;
using Microsoft.AspNetCore.Mvc.Abstractions;
using PortalGenius.Core.Models;
using System.Diagnostics;
using System.Collections.Concurrent;

namespace PortalGenius.Api.Controllers;

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
        // 12-01-2022 MME: je zal in de documentatie moeten duiken maar List is niet thread safe!
        // oftewel als 2 threads 10 items in de lijst wegschrijven is er met List<T> geen enkele garantie dat je uiteindelijk 20 items in je lijst hebt!
        // ConcurrentBag is volgens mij een alternatief, die wordt in Portal Genius gebruikt
        // https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentbag-1?view=netframework-4.7.2
        // overal locks in de code gebruiken is geen oplossing want dan kun je waarschijnlijk beter geen threading gebruiken
        // https://chrisstclair.co.uk/multithreading-made-easy-parallel-foreach/
        var result = new ConcurrentBag<string>();
        var items = await _argGISService.GetAllItemsAsync();
        // 12-01-2022 MME: het task parallel framework maakt indien nodig inderdaad gebruik van de threadpool
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

