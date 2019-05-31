using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sample.ServerSide.Services
{
    public class ClusterService : IHostedService
    {
        private readonly ILogger<ClusterService> logger;

        public ClusterService(ILogger<ClusterService> logger)
        {
            this.logger = logger;

            Client = new ClientBuilder()
                .UseLocalhostClustering()
                .Build();
        }

        public Task StartAsync(CancellationToken cancellationToken) =>

            Client.Connect(async error =>
            {
                logger.LogError(error, error.Message);
                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
                return true;
            });

        public Task StopAsync(CancellationToken cancellationToken) => Client.Close();

        public IClusterClient Client { get; }
    }

    public static class ClusterServiceBuilderExtensions
    {
        public static IServiceCollection AddClusterService(this IServiceCollection services)
        {
            services.AddSingleton<ClusterService>();
            services.AddSingleton(_ => _.GetService<ClusterService>().Client);
            services.AddSingleton<IHostedService>(_ => _.GetService<ClusterService>());
            return services;
        }
    }
}