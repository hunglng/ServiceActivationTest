using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using ServiceActivationTest.Services;
using System;
using System.ComponentModel;

namespace ServiceActivationTest.Tenant
{

    public interface ITenantServiceProvider
    {
        IServiceProvider GetServiceProvider(TenantInfo tenantInfo);
    }

    public class TenantServiceProvider : ITenantServiceProvider
    {
        private readonly Dictionary<string, IServiceProvider> _serviceProviders = new();
        private readonly List<TenantInfo> _tenantInfos = [
            new TenantInfo() {
            TenantId = "1",
        },
        new TenantInfo() {
            TenantId = "2",
        }
        ];
        private readonly IServiceCollection _rootServiceCollection;

        public TenantServiceProvider(IServiceCollection serviceCollection)
        {
            Console.WriteLine("Inittttttttttttttttttttttt");
            _rootServiceCollection = serviceCollection;
            InitializreSerivces();
        }

        private void InitializreSerivces()
        {
            foreach (var tenantInfo in _tenantInfos)
            {
                if (!_serviceProviders.ContainsKey(tenantInfo.TenantId))
                {
                    var tenantServices = new ServiceCollection();

                    foreach (var service in _rootServiceCollection)
                        tenantServices.Add(service);

                    //// Add tenant-specific services
                    //tenantServices.AddDbContext<ApplicationDbContext>(options =>
                    //    options.UseSqlServer(tenantInfo.ConnectionString));

                    tenantServices.AddSingleton<IWeatherForecastService, WeatherForecastService>();
                    tenantServices.AddHostedService<TimerService>();
                    tenantServices.AddHostedService<TimerServiceV2>();

                    var tenantServiceProvider = tenantServices.BuildServiceProvider();
                    _serviceProviders[tenantInfo.TenantId] = tenantServiceProvider;

                    var hostedServices = tenantServiceProvider.GetServices<IHostedService>();
                    foreach (var service in hostedServices)
                    {
                        service.StartAsync(CancellationToken.None);
                    }
                }
            }
        }

        public IServiceProvider GetServiceProvider(TenantInfo tenantInfo)
        {
            _serviceProviders.TryGetValue(tenantInfo.TenantId, out var serviceProvider);
            return serviceProvider;
        }
    }

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddTenantServiceProvider(this IServiceCollection services)
        {
            var tenantProvider = new TenantServiceProvider(services);

            services.AddSingleton<ITenantServiceProvider>(tenantProvider);

            return services;
        }
    }
}
