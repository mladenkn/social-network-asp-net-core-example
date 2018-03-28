using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using SocialNetwork.Models;
using Utilities;
using Xunit;

namespace SocialNetwork.UnitTests
{
    public class GeneralTest
    {
        [Fact]
        public void Start()
        {
            "mate"
                .Capitalize()
                .Equals("Mate")
                .Also(Assert.True);

            IEnumerable<Post> posts = CollectionUtils.NewArray(() => new Post(), 10);
            IEnumerable<Post> posts_ = posts;
        }
    }
}
