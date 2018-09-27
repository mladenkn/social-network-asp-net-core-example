using ApplicationKernel.Infrastructure;
using SocialNetwork.Tests.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using SocialNetwork.Domain.UseCases;
using FluentAssertions;

namespace SocialNetwork.Tests
{
    public class RegistratedServicesTest : IntegrationTestBase
    {
        [Fact]
        public void MasterValidator_should_validate()
        {
            Initialize();
            var validator = Services.GetService<IMasterRequestValidator>();
            validator.Should().NotBeNull();
            validator.Validate(new DeletePost.Request()).Should().NotBeNull();
        }
    }
}
