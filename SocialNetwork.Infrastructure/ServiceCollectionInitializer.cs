using ApplicationKernel.Domain.DataQueries;
using ApplicationKernel.Infrastructure;
using ApplicationKernel.Infrastructure.Database;
using Microsoft.Extensions.DependencyInjection;
using SocialNetwork.Domain.Posts;
using SocialNetwork.Domain.UseCases;
using SocialNetwork.Domain.Users;

namespace SocialNetwork.Infrastructure
{
    public static class ServiceCollectionInitializer
    {
        public static IServiceCollection AddSocialNetwork(this IServiceCollection services)
        {
            services
                .AddTransient<IQuery, Query>()
                .AddRequestValidators(typeof(RatePost.RequestValidator).Assembly)
                .AddTransient<IPostPermissionProvider, PostPermissionProvider>()
                .AddDelegateTransient<GetUserActionsForPost, IPostPermissionProvider>(it => it.GetAllowedPostActionsForUser)
                .AddDelegateTransient<GetCurrentUserId, CurrentUserAccessor>(it => it.GetId)
                .AddTransient<Domain.IUseCaseExecutorTools, UseCaseExecutorTools>()
                .AddQueryableTransient<User>()
                ;
            return services;
        }
    }
}
