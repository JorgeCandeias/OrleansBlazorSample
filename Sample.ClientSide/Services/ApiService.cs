﻿using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Sample.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sample.ClientSide.Services
{
    public class ApiService
    {
        private readonly HttpClient client;
        private readonly ApiServiceOptions options;

        public ApiService(IOptions<ApiServiceOptions> options, HttpClient client)
        {
            this.client = client;
            this.options = options.Value;
        }

        public Task<IEnumerable<WeatherInfo>> GetWeatherForecastAsync() =>
            client.GetJsonAsync<IEnumerable<WeatherInfo>>($"{options.BaseAddress}/Weather");
    }

    public static class ApiServiceBuilderExtensions
    {
        public static IServiceCollection AddApiService(this IServiceCollection services, Action<ApiServiceOptions> configureOptions = null)
        {
            services.AddSingleton<ApiService>();
            if (configureOptions != null)
            {
                services.Configure(configureOptions);
            }
            return services;
        }
    }
}