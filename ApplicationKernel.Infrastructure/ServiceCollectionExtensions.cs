using System;
using System.Linq;
using System.Reflection;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationKernel.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDelegateTransient<TDelegate, TImplementor>(
            this IServiceCollection services, Func<TImplementor, TDelegate> getDelegate)
            where TDelegate : class where TImplementor : class
        {
            if (!services.Contains<TImplementor>())
                services.AddTransient<TImplementor>();

            services.AddTransient(serviceProvider =>
            {
                var implementor = serviceProvider.GetService<TImplementor>();
                return getDelegate(implementor);
            });
            return services;
        }

        public static bool Contains<TService>(this IServiceCollection services)
        {
            return services.Any(s => s.ServiceType == typeof(TService));
        }

        public static IServiceCollection AddQueryableTransient<TEntity>(this IServiceCollection services)
            where TEntity : class
        {
            services.AddTransient<IQueryable<TEntity>>(serviceProvider =>
            {
                var db = serviceProvider.GetService<DbContext>();
                return db.Set<TEntity>();
            });
            return services;
        }

        public static IServiceCollection AddDatabase<TDbContext>(
            this IServiceCollection services,
            Action<DbContextOptionsBuilder> builderAction,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where TDbContext : DbContext
        {
            services.AddDbContext<TDbContext>(builderAction, lifetime);
            services.AddTransient<DbContext>(sp => sp.GetService<TDbContext>());
            return services;
        }

        public static IServiceCollection AddRequestValidators(this IServiceCollection services, Assembly assembly)
        {
            AssemblyScanner.FindValidatorsInAssembly(assembly)
                .ForEach(pair =>
                {
                    services.AddTransient(typeof(IValidator), pair.ValidatorType);
                    services.AddTransient(pair.InterfaceType, pair.ValidatorType);
                });
            services.AddTransient<IValidatorProvider, ValidatorProvider>();
            return services;
        }
    }
}
