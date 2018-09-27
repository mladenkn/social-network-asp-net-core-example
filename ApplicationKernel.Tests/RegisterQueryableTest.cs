using ApplicationKernel.Infrastructure;
using ApplicationKernel.Infrastructure.WebApi;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace ApplicationKernel.Tests
{
    public class RegisterQueryableTest
    {
        [Fact]
        public void Run()
        {
            var services = new ServiceCollection();
            services.AddDelegateTransient<GetString, Service>(m => m.GetString);

            services.Count.Should().Be(2);

            var sp = services.BuildServiceProvider();
            var func = sp.GetService<GetString>();

            func.Should().NotBeNull();
            func().Should().Be("1");
        }

        private delegate string GetString();

        private class Service
        {
            public string GetString() => "1";
        }
    }
}
