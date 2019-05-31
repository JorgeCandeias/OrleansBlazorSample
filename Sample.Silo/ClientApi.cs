using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans;
using Sample.Silo.Api;
using System.Threading;
using System.Threading.Tasks;

namespace Sample.Silo
{
    public class ClientApi : IHostedService
    {
        private readonly IWebHost host;

        public ClientApi(IClusterClient client)
        {
            host = WebHost
                .CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddSingleton(client);
                    services.AddMvc()
                        .SetCompatibilityVersion(CompatibilityVersion.Latest)
                        .AddApplicationPart(typeof(DummyController).Assembly)
                        .AddControllersAsServices();
                    services.AddSwaggerGen();
                })
                .Configure(app =>
                {
                    app.UseMvc();
                    app.UseSwagger();
                    app.UseSwaggerUI();
                })
                .UseUrls("http://localhost:8081")
                .Build();
        }

        public Task StartAsync(CancellationToken cancellationToken) =>
            host.StartAsync(cancellationToken);

        public Task StopAsync(CancellationToken cancellationToken) =>
            host.StopAsync(cancellationToken);
    }

    public static class ClientApiExtensions
    {
        public static IHostBuilder UseClientApi(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services =>
            {
                services.AddHostedService<ClientApi>();
            });
            return hostBuilder;
        }
    }
}