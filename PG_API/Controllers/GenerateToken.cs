using System.Text;
using log4net;
using Microsoft.AspNetCore.Mvc;
using PG_API.Handlers;

namespace PG_API.Controllers;

[Route("[controller]")]
[ApiController]
public class GenerateToken : Controller
{
    private static readonly ILog _log = LogManager.GetLogger(typeof(UserController));
    private readonly AsyncHandler _asyncHandler = new();
    private readonly HttpClient _httpClient = new();
    private readonly HttpRequestMessage _requestMessage =
        new(HttpMethod.Post, "https://arcgis.com/sharing/generateToken");

    private string username = "Windesheim";
    private string password = "vwa*dbq-amb4waf!KPA";

    [HttpPost("/generate")]
    public ObjectResult GenerateNewToken()
    {
        try
        {
            _requestMessage.Content = new StringContent(
                $"referer=https://portalgenius.maps.arcgis.com/rest&username={username}&password={password}&client=referer&f=json",
                Encoding.UTF8, "application/x-www-form-urlencoded");
            _requestMessage.Headers.Add("Cache-Control", "no-cache");
            if (_asyncHandler.SetToken(_httpClient.Send(_requestMessage).Content))
            {
                return Ok("Successfully created a new token");
            } 
            throw new Exception("Wrong username/password");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BadRequestObjectResult(e.Message);
        }
    }

    [HttpPost("/set/{username}/{password}")]
    public ObjectResult SetAccount(string username, string password)
    {
        try
        {
            _requestMessage.Content = new StringContent(
                $"referer=https://portalgenius.maps.arcgis.com/rest&username={username}&password={password}&client=referer&f=json",
                Encoding.UTF8, "application/x-www-form-urlencoded");
            _requestMessage.Headers.Add("Cache-Control", "no-cache");
            if (_asyncHandler.SetToken(_httpClient.Send(_requestMessage).Content))
            {
                return Ok("Successful created a new token");
            } 
            throw new Exception("Wrong username/password");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Unauthorized(e.Message);
        }
        
        _httpClient.Send(new HttpRequestMessage(HttpMethod.Post, "generate"));
    }
}