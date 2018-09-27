using AutoMapper;
using SocialNetwork.Domain;
using Xunit;

namespace SocialNetwork.Tests
{
    public class AutoMapperTest
    {
        [Fact]
        public void Configuration_should_be_valid()
        {
            var mapperConfig = new MapperConfiguration(c => c.AddProfile<AutoMapperProfile>());
            mapperConfig.AssertConfigurationIsValid();
        }
    }
}
