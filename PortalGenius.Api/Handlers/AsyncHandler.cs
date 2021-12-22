namespace PG_API.Handlers;

public class AsyncHandler
{
    public async Task<string> Read(HttpContent httpContent)
    {
        return await httpContent.ReadAsStringAsync();
    }
    public bool SetToken(HttpContent httpContent)
    {
        Task<string> task = Read(httpContent);
        Console.WriteLine(task.Result);
        //string token = task.Result.Substring(10, 216);

        //todo: store token
        return true;
    }
}