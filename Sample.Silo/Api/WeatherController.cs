using Microsoft.AspNetCore.Mvc;
using Orleans;

namespace Sample.Silo.Api
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly IGrainFactory factory;

        public WeatherController(IGrainFactory factory)
        {
            this.factory = factory;
        }
    }
}