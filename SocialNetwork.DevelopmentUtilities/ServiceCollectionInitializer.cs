using ApplicationKernel.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using SocialNetwork.Domain.Users;

namespace SocialNetwork.DevelopmentUtilities
{
    public static class ServiceCollectionInitializer
    {
        public static IServiceCollection AddSocialNetworkDevelopmentUtilities(this IServiceCollection services)
        {
            services
                .AddTransient<Settings>()
                .AddDelegateTransient<GetCurrentUserId, CurrentUserAccessor>(s => s.GetCurrentUserId);
            return services;
        }
    }
}
