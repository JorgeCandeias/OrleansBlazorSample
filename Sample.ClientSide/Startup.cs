using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using Sample.ClientSide.Api;
using System;

namespace Sample.ClientSide
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ApiClient>()
                .Configure<ApiClientOptions>(options =>
                {
                    options.BaseAddress = new Uri("http://localhost:8081/api");
                });
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}