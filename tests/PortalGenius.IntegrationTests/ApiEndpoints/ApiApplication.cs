﻿using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using PortalGenius.Infrastructure.Data;

namespace PortalGenius.IntegrationTests.ApiEndpoints;

internal class ApiApplication : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        var root = new InMemoryDatabaseRoot();

        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<AppDbContext>));
            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase("Testing", root));
        });

        return base.CreateHost(builder);
    }

    public static async Task<T> ParseHttpResponseToJsonAsync<T>(HttpResponseMessage response)
    {
        var contentStream = await response.Content.ReadAsStreamAsync();

        using var streamReader = new StreamReader(contentStream);
        using var jsonReader = new JsonTextReader(streamReader);

        return new JsonSerializer().Deserialize<T>(jsonReader);
    }
}
