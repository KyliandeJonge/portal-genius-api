var client = new HttpClient();

await Main();

async Task Main()
{
    try
    {
        var local = new Uri("https://localhost:7032/User");
        var arcgis =
            new Uri(
                "https://portalgenius.maps.arcgis.com/sharing/rest/portals/x/users?f=json&token=np8MwYSSuAkSeuAm7bS2Ad_m3NL-rzJWYtKW7YZQ0q16r12nZxLU-aG3BF1ve54ZbrZp2fJ2_aVhCU_RGXd4bMEIK0zlbp5En4iGVgjAeLkeIobsqKotef7ydUFrdc2tTuplnZ9Nnwiz0s9NN74HvZX_d0XFXVS29a0mjUcN_JM9VjruFW38y0A5PxTqP9FPa8Po-86JuahC60jYgsGWGQ..&searchUserAccess=*&filter=*&num=100");

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, local);
        var responseMessage = await client.SendAsync(requestMessage);
        var responseBody = await responseMessage.Content.ReadAsStringAsync();
        Console.WriteLine(responseBody);
    }
    catch (HttpRequestException e)
    {
        Console.WriteLine(e.Message);
    }
}
