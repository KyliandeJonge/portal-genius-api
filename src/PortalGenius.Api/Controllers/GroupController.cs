using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace PortalGenius.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class GroupController : ControllerBase
{
    [HttpPost()]
    public string Post()
    {
        throw new NotImplementedException();
    }
}
