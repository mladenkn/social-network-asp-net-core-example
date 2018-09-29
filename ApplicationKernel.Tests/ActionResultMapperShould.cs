using ApplicationKernel.Domain.MediatorSystem;
using ApplicationKernel.Infrastructure.WebApi;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace ApplicationKernel.Tests
{
    public class ActionResultMapperShould
    {
        private readonly ActionResultMapper _mapper = new ActionResultMapper();

        [Fact]
        public void Return_OkObjectResult_when_receiving_successfull_ResponseT()
        {
            var responsePayload = "tralal";
            _mapper.Map(Responses.Success(responsePayload))
                .Should().BeAssignableTo<OkObjectResult>().Which
                .Value.Should().Be(responsePayload);
        }

        [Fact]
        public void Return_OkResult_when_receiving_successfull_Response()
        {
            _mapper.Map(Responses.Success()).Should().BeAssignableTo<OkResult>();
        }
    }
}
