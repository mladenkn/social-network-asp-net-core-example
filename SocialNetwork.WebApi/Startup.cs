using ApplicationKernel.Infrastructure;
using ApplicationKernel.Infrastructure.WebApi;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SocialNetwork.DevelopmentUtilities;
using SocialNetwork.Domain;
using SocialNetwork.Domain.UseCases;
using SocialNetwork.Infrastructure;
using Swashbuckle.AspNetCore.Swagger;

namespace SocialNetwork.WebApi
{
    public class Startup
    {
        private readonly IAdditionalStartupConfiguration _additionalConfiguration;
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IAdditionalStartupConfiguration additionalConfiguration = null)
        {
            _additionalConfiguration = additionalConfiguration;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddJsonOptions(config => config.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services
                .AddMediatR(typeof(PublishPost).Assembly)
                .AddAutoMapper(typeof(AutoMapperProfile).Assembly)
                .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new Info { Title = "Social Network Web API" });
                    c.CustomSchemaIds(it => it.FullName);
                })
                .AddDelegateTransient<MapToActionResult, ActionResultMapper>(m => m.Map)
                .AddDelegateTransient<HandleApiRequest, ApiRequestHandler>(handler => handler.Handle)
                .AddSocialNetwork()
                .AddSocialNetworkDevelopmentUtilities();

            if (_additionalConfiguration?.AddDatabase != null)
            {
                _additionalConfiguration.AddDatabase(services);
            }
            else
            {
                //var connection = Configuration.GetSection("Database").GetValue<string>("ConnectionString");
                var connection = Configuration.GetValue<string>("Database:ConnectionString");
                services.AddDatabase<SocialNetworkDbContext>(options => 
                    options
                        .UseSqlServer(connection)
                        .ConfigureWarnings(it => it.Throw(RelationalEventId.QueryClientEvaluationWarning))
                        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
            }

            _additionalConfiguration?.AddMoreServices?.Invoke(services);    
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web API");
            });
        }
    }
}
