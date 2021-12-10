namespace PG_API.Controllers;

public class AsyncHandler
{
    public async Task<string> Read(HttpContent httpContent)
    {
        return await httpContent.ReadAsStringAsync();
    }
}