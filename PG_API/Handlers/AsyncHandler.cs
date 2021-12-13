namespace PG_API.Handlers;

public class AsyncHandler
{
    public async Task<string> Read(HttpContent httpContent)
    {
        return await httpContent.ReadAsStringAsync();
    }
}