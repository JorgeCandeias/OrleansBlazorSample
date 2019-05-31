using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Sample.Models;
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

        public Task<WeatherInfo[]> GetWeatherForecastAsync() =>
            client.GetJsonAsync<WeatherInfo[]>($"{options.BaseAddress}/Weather");
    }
}