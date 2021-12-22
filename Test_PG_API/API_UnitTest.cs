using PG_API.Controllers;
using PortalGenius.Core.Services;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Test_PG_API;

public class API_UnitTest
{  
    public API_UnitTest()
    {
        
    }



    [Fact]
    public async Task Get_AllItems_ReturnsOkResult()
    {
        // arrange
      
        
        // act
        // assert
    }

    [Fact]
    public async Task Get_DataFromItem_ReturnsOkResult()
    {
        // arrange
        //var items = await _argGISService.GetDataFromItem("594f514df776476ab58345fc09bdaba0").ConfigureAwait(false);

        // act
        // assert
    }

    public async Task Get_DataFromItem_ReturnsBadResult()
    {
        // arrange
        //var items = await _argGISService.GetDataFromItem("12345").ConfigureAwait(false);

        // act
        // assert
    }
}
