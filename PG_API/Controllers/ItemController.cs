using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using PG_API.Services;

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

    [HttpPost()]
    public string Post()
    {
        //return Ok(await _argGISService.GetItems());

        //todo: set link to get /Item
        HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post,
            "https://portalgenius.maps.arcgis.com/sharing/rest/portals/x/users?f=json&token=np8MwYSSuAkSeuAm7bS2Ad_m3NL-rzJWYtKW7YZQ0q16r12nZxLU-aG3BF1ve54ZbrZp2fJ2_aVhCU_RGXd4bMEIK0zlbp5En4iGVgjAeLkeIobsqKotef7ydUFrdc2tTuplnZ9Nnwiz0s9NN74HvZX_d0XFXVS29a0mjUcN_JM9VjruFW38y0A5PxTqP9FPa8Po-86JuahC60jYgsGWGQ..&searchUserAccess=*&filter=*&num=100");
        HttpResponseMessage responseMessage = new HttpClient().Send(requestMessage);
        return responseMessage.Content.ToJson();
    }
    // 
}
