using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using Xunit;

namespace PortalGenius.IntegrationTests.ApiEndpoints
{
    [Collection("Sequential")]
    public class ItemList
    {
        private readonly HttpClient _client;

        public ItemList()
        {
            
        }
    }
}
