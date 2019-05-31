using Orleans;
using Sample.Models;
using System;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace Sample.Grains
{
    public class WeatherGrain : Grain, IWeatherGrain
    {
        private readonly ImmutableList<WeatherInfo> data = ImmutableList.Create(
            new WeatherInfo
            {
                Date = DateTime.Today.AddDays(1),
                TemperatureC = 1,
                Summary = "Freezing",
                TemperatureF = 33
            },
            new WeatherInfo
            {
                Date = DateTime.Today.AddDays(2),
                TemperatureC = 14,
                Summary = "Bracing",
                TemperatureF = 57
            },
            new WeatherInfo
            {
                Date = DateTime.Today.AddDays(3),
                TemperatureC = -13,
                Summary = "Freezing",
                TemperatureF = 9
            },
            new WeatherInfo
            {
                Date = DateTime.Today.AddDays(4),
                TemperatureC = -16,
                Summary = "Balmy",
                TemperatureF = 4
            },
            new WeatherInfo
            {
                Date = DateTime.Today.AddDays(5),
                TemperatureC = -2,
                Summary = "Chilly",
                TemperatureF = 29
            });

        public Task<ImmutableList<WeatherInfo>> GetForecastAsync() => Task.FromResult(data);
    }
}