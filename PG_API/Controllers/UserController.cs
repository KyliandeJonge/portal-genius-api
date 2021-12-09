using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace PG_API.Controllers;

[Route("[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    [HttpPost()]
    public string Post()
    {
        HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post,
            "https://portalgenius.maps.arcgis.com/sharing/rest/portals/x/users?f=json&token=6Jv9FkkWq7T78yD4Egh_2ZIDhv39DWlMBN_ps49ibz0EBihF8pJ7hgrO6Ru_yjGRXWoT9IKAKGEwikddYyBlTfLXo-zYk0eW1EVqdgR7MI2LGtLoRg8YNoNaHp01kCRoVfbAmh6Xm_6IJQcz2le647fvR9FXwtb7EQ-SRwAz2Zbf6xwPohkF6lBjhcoEoPdTHw-6X5iwKADpJEKOtb2fwQ..&searchUserAccess=*&filter=*&num=100");
        HttpResponseMessage responseMessage = new HttpClient().Send(requestMessage);
        return responseMessage.Content.ToJson();
    }
}