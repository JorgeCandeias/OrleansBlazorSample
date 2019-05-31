using System;

namespace Sample.Models
{
    public class WeatherInfo
    {
        public WeatherInfo()
        {
        }

        public WeatherInfo(DateTime date, int temperatureC, string summary, int temperatureF)
        {
            Date = date;
            TemperatureC = temperatureC;
            Summary = summary;
            TemperatureF = temperatureF;
        }

        public DateTime Date { get; set; }
        public int TemperatureC { get; set; }
        public string Summary { get; set; }
        public int TemperatureF { get; set; }
    }
}