using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Sample.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sample.ClientSide.Api
{
    public class ApiClient
    {
        private readonly HttpClient client;
        private readonly ApiClientOptions options;

        public ApiClient(IOptions<ApiClientOptions> options, HttpClient client)
        {
            this.client = client;
            this.options = options.Value;
        }

        public Task<IEnumerable<WeatherInfo>> GetWeatherForecastAsync() =>
            client.GetJsonAsync<IEnumerable<WeatherInfo>>($"{options.BaseAddress}/Weather");
    }
}