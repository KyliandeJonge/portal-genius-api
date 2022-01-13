using Microsoft.Extensions.Logging;
using PortalGenius.Core.Data;
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
        ///     Maakt eerst nieuwe token aan en gebruikt die om item's op te halen.
        /// </summary>
        /// <returns>items in object</returns>
        public async Task<List<Item>> GetAllItemsAsync()
        {
            await TryGetToken();
            return await _httpService.GetSearchResultsAsync<Item>($"rest/search?q=accountid:{UserData.accountID}&f=json&token={UserData.genToken}");
        }

        private async Task TryGetToken()
        {
            try
            {
                await GetGenTokenAsync();
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
            await TryGetToken();

            return await _httpService.GetAsync<object>($"rest/content/items/{item_id}/data?f=json&token={UserData.genToken}");
        }

        public async Task<object> GetAllUsersAsync()
        {
            await TryGetToken();

            return await _httpService.GetAsync<object>($"rest/portals/x/users?f=json&token={UserData.genToken}&searchUserAccess=*&filter=*&num=100");
        }

        /// <summary>
        /// probeert token op te halen met bekende of nieuwe credentials
        /// </summary>
        /// <param name="username">Niet verplicht, als die niet is ingevuld wordt de oude opgeslagen gebruikersnaam gebruikt</param>
        /// <param name="password">Niet verplicht, als die niet is ingevuld wordt het oude opgeslagen wachtwoord gebruikt gebruikt</param>
        /// <param name="newCreds">Niet verplicht, als deze op true staat worden de meegegeven username en password opgelagen als deze juist zijn</param>
        /// <returns>GenerateToken</returns>
        public async Task<GenerateToken> GetGenTokenAsync(string username = "", string password = "", bool newCreds = false)
        {
            if (username.Equals("")) { username = UserData.username; } 
            if (password.Equals("")) { password = UserData.password; }
            
            GenerateToken token = await _httpService.PostAsync<GenerateToken>("generateToken", new StringContent(
                 $"referer='https://portalgenius.maps.arcgis.com/rest'&username={username}&password={password}&client=referer&f=json", Encoding.UTF8, "application/x-www-form-urlencoded"));
            try 
            {
                // 12-01-2022 MME: test voor de tekst "null"?
                // string.isnullorempty(token.Token) en als de token geen waarde heeft dan geef je string.empty terug, je geeft vanuit de API nooit een tekst 'null' terug
                // eigenlijk moet je je afvragen als de token er niet is of je ipv http 200 niet een http 404 moet terug geven, je doet een postasync en dan kun je ook de http code uitlezen
                // is die http code niet 200 dan stop je dus
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
            // 12-01-2022 MME: het afvangen van een nullreference exception doe je niet
            // - maak slim gebruik van null coalescing operator
            // een test van object == null -> return is in runtime veel efficienter dan een try/catch
            // try/catch clauses zijn heel erg slecht voor de performance van je applicatie
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public async Task<GenerateToken> GetGenTokenWithNewCredsAsync(string username, string password)
        {
            return await GetGenTokenAsync(username, password, true); ;
        }

        public Task<GenerateToken> GetGenToken(string username = "", string password = "", bool newCreds = false)
        {
            throw new NotImplementedException();
        }
    }

}

