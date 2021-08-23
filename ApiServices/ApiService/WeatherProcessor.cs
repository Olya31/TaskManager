using ApiServices.ApiService.Interfaces;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ApiServices.ApiService
{
    public sealed class WeatherProcessor : IWeatherProcessor
    {
        private const string Key = "328527dd63mshd12fa2f7a05e3e5p1e3b37jsn240a75026ae1";
        private readonly IHttpClientFactory _httpClientFactory;

        public WeatherProcessor(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> LoadWeatherInformation(
            string url,
            string header,
            CancellationToken token)
        {
            using var client = _httpClientFactory.CreateClient();

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url),
                Headers =
                {
                   { "x-rapidapi-host", header },
                   { "x-rapidapi-key",  Key},
                },
            };

            var response = await client.SendAsync(request, token);

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
