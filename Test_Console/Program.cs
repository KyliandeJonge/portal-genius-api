using System.Net;

HttpClient client = new HttpClient();

await Main();

async Task Main()
{
    try
    {
        HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44244/User");
        HttpResponseMessage responseMessage = await client.SendAsync(requestMessage);
        string responseBody = await responseMessage.Content.ReadAsStringAsync();
        Console.WriteLine(responseBody);
    }
    catch (HttpRequestException e)
    {
        Console.WriteLine(e.Message);
    }
}
