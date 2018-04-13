using System.Collections.Generic;
using SocialNetwork.Interface.Models.Entities;
using Utilities;
using Xunit;

namespace SocialNetwork.UnitTests
{
    public class GeneralTest
    {
        [Fact]
        public void Something()
        {
            "mate"
                .Capitalize()
                .Equals("Mate")
                .Also(Assert.True);

            IEnumerable<Post> posts = CollectionUtils.NewArray(() => new Post(), 10);
        }
    }
}
