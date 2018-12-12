using ApplicationKernel.Domain;
using FluentAssertions;
using SocialNetwork.Domain.Posts;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Utilities.Tests
{
    public class Various
    {
        [Fact]
        public void Implements()
        {
            var doesImplement = typeof(Post).Implements<IDeletable>();
            doesImplement.Should().BeTrue();
        }
    }
}
