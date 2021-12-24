using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using Microsoft.Extensions.Logging;
using PG_API.Data;
using PortalGenius.Core.Models;

namespace PortalGenius.Core.Services
{
    public class ArcGISService : IArcGISService
    {
        private readonly IHttpService _httpService;

        private readonly ILogger<ArcGISService> _logger;

        public ArcGISService(
            IHttpService httpService,
            ILogger<ArcGISService> logger
        )
        {
            _httpService = httpService;
            _logger = logger;
        }

        public async Task<object> GetAllItemsAsync()
        {
            return await _httpService.GetAsync<object>($"rest/search?q=accountid:{UserData.accountID}&f=json&token={UserData.genToken}");
        }

        public async Task<object> GetAllUsersAsync()
        {
            return await _httpService.GetAsync<object>($"rest/portals/x/users?f=json&token={UserData.genToken}&searchUserAccess=*&filter=*&num=100");
        }

        public async Task<GenerateToken> GetGenToken(string username = "", string password = "", bool newCreds = false)
        {
            if (username.Equals("")) { username = UserData.username; } 
            if (password.Equals("")) { password = UserData.password; }
            
            GenerateToken token = await _httpService.PostAsync<GenerateToken>("generateToken", new StringContent(
                 $"referer='https://portalgenius.maps.arcgis.com/rest'&username={username}&password={password}&client=referer&f=json", Encoding.UTF8, "application/x-www-form-urlencoded"));
            try 
            {
                if (!token.Token.Equals("null"))
                {
                    UserData.genToken = token.Token;
                    _logger.LogTrace($"new token: {UserData.genToken}");
                    if (newCreds)
                    {
                        UserData.username = username;
                        _logger.LogTrace("new username inserted");
                        UserData.password = password;
                        _logger.LogTrace("new password inserted");
                    }
                }

                return token;
            }
            catch (NullReferenceException e)
            {
                return null;
            }
        }

        public async Task<GenerateToken> GetGenTokenWithNewCreds(string username, string password)
        {
            GenerateToken token = GetGenToken(username, password, true).Result;
            return token;
        }
    }

}

