﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using PG_API.Data;
using PortalGenius.Core.Models;

namespace PortalGenius.Core.Services
{
    public class HttpService
    {
        private readonly HttpClient _httpClient;

        private readonly ILogger<HttpService> _logger;

        public HttpService(
            IHttpClientFactory httpClientFactory,
            IOptions<HttpServiceOptions> httpServiceOptions,
            ILogger<HttpService> logger
        )
        {
            _httpClient = httpClientFactory.CreateClient(httpServiceOptions.Value.HttpClientName);
            _logger = logger;
        }

        public async Task<T> GetAsync<T>(string path)
        {
            // Het resultaat is standaard de "standaard" waarde van T (meestal null).
            T result = default;

            var apiUrl = $"{_httpClient.BaseAddress}/{path}";

            try
            {
                var response = await _httpClient.GetAsync(apiUrl, HttpCompletionOption.ResponseContentRead);
                if (response.IsSuccessStatusCode)
                    result = await ParseHttpResponseToJsonAsync<T>(response);
                else
                    _logger.LogWarning($"[HTTP GET {response.StatusCode}] Something went wrong while connecting with: ({apiUrl}).", response.StatusCode);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"[HTTP GET 500] Error while connecting with: ({apiUrl}).");
                _logger.LogError(ex.Message);
            }

            return result;
        }

        /*public async Task<T> PostAsync<T>(string path, object requestBody)
        {
            // Het resultaat is standaard de "standaard" waarde van T (meestal null).
            T result = default;

            var apiUrl = $"{_httpClient.BaseAddress}/{path}";

            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, MediaTypeNames.Application.Json);

                var response = await _httpClient.PostAsync(apiUrl, content);
                if (response.IsSuccessStatusCode)
                    result = await ParseHttpResponseToJsonAsync<T>(response);
                else
                    _logger.LogWarning($"[HTTP POST {response.StatusCode}] Something went wrong while connecting with: ({apiUrl}).", response.StatusCode);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"[HTTP POST 500] Error while connecting with: ({apiUrl}).");
                _logger.LogError(ex.Message);
            }
            
            return result;
        }*/

        public async Task<T> PostAsync<T>(string path, StringContent stringContent)
        {
            // Het resultaat is standaard de "standaard" waarde van T (meestal null).
            T result = default;
            string apiUrl = $"{_httpClient.BaseAddress}/{path}";
            HttpResponseMessage responseMessage;    
            
            try
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_httpClient.BaseAddress}/{path}");
                requestMessage.Content = stringContent;
                requestMessage.Headers.Add("Cache-Control", "no-cache");
                responseMessage = _httpClient.Send(requestMessage);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"[HTTP POST 500] Error while connecting with: ({apiUrl}).");
                _logger.LogError(ex.Message);
                return result;
            }

            if (!responseMessage.IsSuccessStatusCode) return result;
            {
                try
                {
                    result = await ParseHttpResponseToJsonAsync<T>(responseMessage);
                }
                catch (RegexParseException ex)
                {
                    _logger.LogError("could not parse input to Jons");
                    _logger.LogError(ex.Message);
                }
            }
            return result;
           
        }
        
        /// <summary>
        /// Converteer de JSON HTTP response naar de gewenste vorm.
        /// </summary>
        /// <typeparam name="T">De vorm waar de JSON naartoe geconverteerd moet worden.</typeparam>
        /// <param name="response">De HTTP-response message.</param>
        /// <returns>De geserialiseerde JSON in de gewenste vorm.</returns>
        private async Task<T> ParseHttpResponseToJsonAsync<T>(HttpResponseMessage response)
        {
            var contentStream = await response.Content.ReadAsStreamAsync();

            using var streamReader = new StreamReader(contentStream);
            using var jsonReader = new JsonTextReader(streamReader);

            return new JsonSerializer().Deserialize<T>(jsonReader);
        }
    }
}
