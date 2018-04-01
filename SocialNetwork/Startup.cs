﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SocialNetwork.DAL;
using SocialNetwork.DAL.Repositories;
using SocialNetwork.Interfaces.DAL;
using SocialNetwork.Models;
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
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<SocialNetworkDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                // If the LoginPath isn't set, ASP.NET Core defaults 
                // the path to /Account/Login.
                options.LoginPath = "/Account/Login";
                // If the AccessDeniedPath isn't set, ASP.NET Core defaults 
                // the path to /Account/AccessDenied.
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

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
            services.AddTransient<Initializer>();
            services.AddMvc(config => config.Filters.Add(typeof(ExceptionHandler)));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();

                var seeder = serviceProvider.GetService<Initializer>();
                seeder.Seed().Wait();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseSignalR(routes => routes.MapHub<Hub>("/posts"));
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
