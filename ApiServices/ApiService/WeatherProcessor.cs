using ApiServices.ApiService.Interfaces;
using CsvHelper;
using CsvHelper.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ApiServices.ApiService
{
    public sealed class WeatherProcessor : IWeatherProcessor
    {
        public readonly IHttpClientFactory _httpClientFactory;

        public WeatherProcessor(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> LoadWeatherInformation(string url, string header)
        {
            var client = _httpClientFactory.CreateClient();

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url),
                Headers =
                {
                   { "x-rapidapi-host", header },
                   { "x-rapidapi-key", "328527dd63mshd12fa2f7a05e3e5p1e3b37jsn240a75026ae1" },
                },
            };
            using (var response = await client.SendAsync(request))
            {

                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
            
                var json = JObject.Parse(body).ToString();
                var result = json.Split("\r\n");
           
                string filePath = @"D:\OmegaSoftware\Task_Manager\Task_Manager\bin\ForecastData\Forecast.csv";
                File.WriteAllLines(filePath, result);

                return "";
            }
        }
    }
}
