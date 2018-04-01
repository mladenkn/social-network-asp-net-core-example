using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SocialNetwork.DAL;
using SocialNetwork.DAL.Repositories;
using SocialNetwork.Interfaces.DAL;
using SocialNetwork.Models;
using SocialNetwork.Services;
using SocialNetwork.TestingUtilities;
using SocialNetwork.Web.ServiceInterfaces;
using SocialNetwork.Web.Services;
using Utilities;

namespace SocialNetwork
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SocialNetworkDbContext>(opt => opt.UseInMemoryDatabase("social-network-db"),
                                                            contextLifetime: ServiceLifetime.Singleton);
            services.AddSignalR();
            services.AddSingleton<IRepository<Post>>(serviceProvider =>
            {
                return serviceProvider.GetService<SocialNetworkDbContext>()
                    .Let(it => new Repository<Post>(it.Posts));
            });
            services.AddSingleton<IRepository<User>>(serviceProvider =>
            {
                return serviceProvider.GetService<SocialNetworkDbContext>()
                    .Let(it => new Repository<User>(it.Users));
            });
            services.AddSingleton<TestDataContainer>();
            services.AddSingleton<IDatabaseOperations, DatabaseOperations>();
            services.AddTransient<IViewRendererService, ViewRendererService>();
            services.AddTransient<DbSeeder>();
            services.AddMvc(config => config.Filters.Add(typeof(ExceptionHandler)));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseSignalR(routes => routes.MapHub<Hub>("/posts"));
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Home}/{id?}");
            });

            var seeder = serviceProvider.GetService<DbSeeder>();
            seeder.Seed().Wait();
        }
    }
}
