using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SocialNetwork.DevelopmentUtilities;

namespace SocialNetwork.WebApi
{
    public class Program
    {
        private static readonly DatabaseSeeder Initializer = new DatabaseSeeder();

        public static async Task Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            if (Initializer != null)
            {
                using (var scope = host.Services.CreateScope())
                {
                    await Initializer.Seed(scope.ServiceProvider);
                }
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
