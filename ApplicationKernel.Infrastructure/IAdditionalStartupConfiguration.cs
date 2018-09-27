using System;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationKernel.Infrastructure
{
    public interface IAdditionalStartupConfiguration
    {
        Action<IServiceCollection> AddDatabase { get; }
        Action<IServiceCollection> AddMoreServices { get; }
    }
}
