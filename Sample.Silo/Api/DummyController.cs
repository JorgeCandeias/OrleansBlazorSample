using Microsoft.AspNetCore.Mvc;

namespace Sample.Silo.Api
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/[controller]")]
    public class DummyController : ControllerBase
    {
    }
}