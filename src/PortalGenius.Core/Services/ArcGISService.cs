using Microsoft.Extensions.Logging;
using PG_API.Data;
using PortalGenius.Core.Models;
using System.Text;

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

        /// <summary>
        /// Maakt eerst nieuwe token aan en gebruikt die om item's op te halen.
        /// </summary>
        /// <returns>items in object</returns>
        public async Task<SearchResult<Item>> GetAllItemsAsync()
        {
            TryGetToken();
            return await _httpService.GetAsync<SearchResult<Item>>($"rest/search?q=accountid:{UserData.accountID}&f=json&token={UserData.genToken}");
        }

        private async void TryGetToken()
        {
            try
            {
                await GetGenToken();
                _logger.LogTrace("New token generated");
            }
            catch (Exception e)
            {
                _logger.LogError("Could not get new token");
                _logger.LogError(e.Message);
                throw;
            }
        }

        public async Task<object> GetDataFromItemAsync(string item_id)
        {
            TryGetToken();
            return await _httpService.GetAsync<object>($"rest/content/items/{item_id}/data?f=json&token={UserData.genToken}");
        }

        public async Task<object> GetAllUsersAsync()
        {
            TryGetToken();
            return await _httpService.GetAsync<object>($"rest/portals/x/users?f=json&token={UserData.genToken}&searchUserAccess=*&filter=*&num=100");
        }

        /// <summary>
        /// probeert token op te halen met bekende of nieuwe credentials
        /// </summary>
        /// <param name="username">Niet verplicht, als die niet is ingevuld wordt de oude opgeslagen gebruikersnaam gebruikt</param>
        /// <param name="password">Niet verplicht, als die niet is ingevuld wordt het oude opgeslagen wachtwoord gebruikt gebruikt</param>
        /// <param name="newCreds">Niet verplicht, als deze op true staat worden de meegegeven username en password opgelagen als deze juist zijn</param>
        /// <returns>GenerateToken</returns>
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

