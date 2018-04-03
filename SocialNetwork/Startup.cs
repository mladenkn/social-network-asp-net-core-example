using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SocialNetwork.DAL;
using SocialNetwork.DevelopmentUtilities;
using SocialNetwork.Interfaces.DAL;
using SocialNetwork.Interfaces.Services;
using SocialNetwork.Models;
using SocialNetwork.Web.ServiceInterfaces;
using SocialNetwork.Services;
using SocialNetwork.Web.Services;
using Utilities;

namespace SocialNetwork.Web
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
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<SocialNetworkDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                options.User.RequireUniqueEmail = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);

                options.LoginPath = "/Account/Login";

                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            services.AddSignalR();

            services.AddSingleton<DbSet<Post>>(provider => provider.GetService<SocialNetworkDbContext>().Posts);
            services.AddSingleton<DbSet<User>>(provider => provider.GetService<SocialNetworkDbContext>().Users);

            services.AddTransient<IRepository<Post>, Repository<Post>>();
            services.AddTransient<IRepository<User>, Repository<User>>();

            services.AddTransient<UserManager<User>>();

            services.AddSingleton<TestDataContainer>();
            services.AddTransient<IHub, Hub>();
            services.AddTransient<IDatabaseOperations, DatabaseOperations>();
            services.AddTransient<IViewRendererService, ViewRendererService>();
            services.AddTransient<Initializer>();
            services.AddTransient<IAuthenticator, Authenticator>();

            services.AddMvc(config =>
            {
                config.Filters.Add(typeof(ExceptionHandler));

                new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build()
                    .Let(it => new AuthorizeFilter(it))
                    .Also(config.Filters.Add);
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();

                serviceProvider
                    .GetService<Initializer>()
                    .Initialize().Wait();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseSignalR(routes => routes.MapHub<Microsoft.AspNetCore.SignalR.Hub>("/posts"));
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
