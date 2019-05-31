using Orleans;
using Sample.Grains;
using Sample.Models;
using System;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace Sample.ServerSide.Data
{
    public class WeatherForecastService
    {
        private readonly IClusterClient client;

        public WeatherForecastService(IClusterClient client)
        {
            this.client = client;
        }

        public Task<ImmutableList<WeatherInfo>> GetForecastAsync() =>

            client.GetGrain<IWeatherGrain>(Guid.Empty)
                .GetForecastAsync();
    }
}