using ApplicationKernel.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SocialNetwork.Infrastructure;
using SocialNetwork.WebApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace SocialNetwork.Tests.Abstract
{
    public class TestServerBuilder
    {
        private readonly ICollection<(Type type, object instance)> _mockedServices
            = new List<(Type serviceType, object instance)>();

        public TestServerBuilder Mock<TService>(TService mockInstance) where TService : class
        {
            _mockedServices.Add((typeof(TService), mockInstance));
            return this;
        }

        public TestServer Build()
        {
            var integrationTestsPath = Assembly.GetExecutingAssembly().Location;
            var applicationPath = Path.GetFullPath(Path.Combine(integrationTestsPath, "../../../../../SocialNetwork.WebApi"));

            var startupConfig = new AdditionalStartupConfiguration();

            startupConfig.AddMoreServices = services =>
            {
                foreach (var (type, instance) in _mockedServices)
                    services.AddSingleton(type, instance);
            };
            
            startupConfig.AddDatabase = services =>
                services.AddDatabase<SocialNetworkDbContext>(
                    options => options.UseInMemoryDatabase("social-network-test-db"), ServiceLifetime.Singleton);

            var serverBuilder = new WebHostBuilder()
                .UseContentRoot(applicationPath)
                .ConfigureServices(services => services.AddSingleton<IAdditionalStartupConfiguration>(startupConfig))
                .UseStartup<Startup>();

            return new TestServer(serverBuilder);
        }

        private class AdditionalStartupConfiguration : IAdditionalStartupConfiguration
        {
            public Action<IServiceCollection> AddDatabase { get; set; }
            public Action<IServiceCollection> AddMoreServices { get; set; }
        }
    }
}
