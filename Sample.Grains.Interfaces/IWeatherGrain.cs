using Orleans;
using Sample.Grains.Models;
using System.Collections.Immutable;

namespace Sample.Grains
{
    public interface IWeatherGrain : IGrainWithStringKey
    {
        ImmutableList<WeatherInfo> GetLatestWeatherAsync();
    }
}