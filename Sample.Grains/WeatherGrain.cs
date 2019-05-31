using Orleans;
using Sample.Grains.Models;
using System;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace Sample.Grains
{
    public class WeatherGrain : Grain, IWeatherGrain
    {
        private readonly ImmutableList<WeatherInfo> data = ImmutableList.Create(
            new WeatherInfo(new DateTime(2018, 5, 6), 1, "Freezing", 33),
            new WeatherInfo(new DateTime(2018, 5, 7), 14, "Bracing", 57),
            new WeatherInfo(new DateTime(2018, 5, 8), -13, "Freezing", 9),
            new WeatherInfo(new DateTime(2018, 5, 9), -16, "Balmy", 4),
            new WeatherInfo(new DateTime(2018, 5, 10), -2, "Chilly", 29));

        public Task<ImmutableList<WeatherInfo>> GetLatestWeatherAsync() => Task.FromResult(data);
    }
}