using log4net;
using log4net.Config;
using log4net.Repository.Hierarchy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Log4Net.AspNetCore.Extensions;
using PG_API.Handlers;

namespace PG_API.Controllers;

[Route("[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    /// <summary>
    /// create logging instance for UserController
    /// </summary>
    private static readonly ILog _log = LogManager.GetLogger(typeof(UserController));
    
    private readonly AsyncHandler _asyncHandler = new();
    private readonly HttpClient _httpClient = new();
    
    
    [HttpPost()]
    public string Post()
    {
        _log.Trace("post request to /user", null);
        HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post,
            "https://portalgenius.maps.arcgis.com/sharing/rest/portals/x/users?f=json&token=K2n1Br9O90sp6IZm31EjWm4dkcn2AevsFhYyb4VqZ62P9pNIi8I4LuWVz6G48Yr3XIegSenUMRAro7TROBmjoFlyTghB4dz5BOC8OH7DuNTUZq76uTW3A2C98knFMFc_FCagtBFFwfR2Lcbrword6scyX1PbiFtWgMvSsILwHAEterwSs_Sv8NVR0ohfoIk1VCrwcYxpvylqsyi19HaAuA..&searchUserAccess=*&filter=*&num=100");
        HttpResponseMessage responseMessage = _httpClient.Send(requestMessage);
        string s = _asyncHandler.Read(responseMessage.Content).Result;
       //todo: implement JsonHandler();
       
       _log.Trace("returning post request to /user", null);
        return s;
    }
}