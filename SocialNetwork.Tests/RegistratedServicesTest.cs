using ApplicationKernel.Infrastructure;
using SocialNetwork.Tests.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using FluentAssertions;

namespace SocialNetwork.Tests
{
    public class RegistratedServicesTest : IntegrationTestBase
    {
        [Fact]
        public void MasterValidator_should_validate()
        {
            Initialize();
            var validator = Services.GetService<IValidatorProvider>();
            validator.Should().NotBeNull();
        }
    }
}
