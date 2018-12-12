using ApplicationKernel.Domain;
using ApplicationKernel.Domain.MediatorSystem;
using Bogus;
using MediatR;
using Microsoft.AspNetCore.TestHost;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SocialNetwork.Infrastructure;
using ApplicationKernel.Domain.DataPersistance;
using System;

namespace SocialNetwork.Tests.Abstract
{
    public abstract class IntegrationTestBase : IServiceProvider
    {
        private readonly TestServerBuilder _testServerBuilder = new TestServerBuilder();
        private TestServer _server;
        private System.IServiceProvider _services;

        public void Mock<TService>(TService mockInstance) where TService : class
        {
            _testServerBuilder.Mock<TService>(mockInstance);
        }

        public IServiceProvider Services
        {
            get
            {
                _services = _services ?? _server.Host.Services.CreateScope().ServiceProvider;
                return _services;
            }
        }

        public void Initialize()
        { 
            _server = _testServerBuilder.Build();
        }

        protected async Task SaveToDatabase(params IEntity[] entities)
        {
            var db = Services.GetService<SocialNetworkDbContext>();
            db.AddRange(entities);
            await db.SaveChangesAsync();
        }

        protected Faker<T> Generate<T>()
            where T : class
        {
            return new Faker<T>();
        }

        protected Faker Faker { get; } = new Faker();

        protected Task<Response> SendRequest(ApplicationKernel.Domain.MediatorSystem.IRequest request)
        {
            var mediator = _server.Host.Services.GetService<IMediator>();
            return mediator.Send(request);
        }

        protected IQueryable<T> Query<T>() where T : class
        {
            var db = Services.GetService<SocialNetworkDbContext>();
            return db.Set<T>();
        }

        public object GetService(Type serviceType) => Services.GetService(serviceType);
    }
}
