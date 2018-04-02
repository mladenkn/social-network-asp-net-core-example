using System;
using System.Collections.Generic;
using SocialNetwork.Models;
using static SocialNetwork.TestingUtilities.Generator;

namespace SocialNetwork.TestingUtilities
{
    public class TestDataContainer
    {
        public IReadOnlyDictionary<string, User> Users { get; set; }
        public IReadOnlyList<Post> Posts { get; set; }

        public TestDataContainer()
        {
            Users = new Dictionary<string, User>
            {
                ["Frane"] = RandomUser(userName: "Frane"),
                ["Mate"] = RandomUser(userName: "Mate"),
                ["Ante"] = RandomUser(userName: "Ante"),
            };

            Posts = new[]
            {
                RandomPost(createdAt: DateTime.Parse("2018-03-18"), author: Users["Mate"]),
                RandomPost(createdAt: DateTime.Parse("2018-03-17"), author: Users["Mate"]),
                RandomPost(createdAt: DateTime.Parse("2018-03-10"), author: Users["Frane"]),
                RandomPost(createdAt: DateTime.Parse("2018-02-20"), author: Users["Frane"]),
                RandomPost(createdAt: DateTime.Parse("2018-02-10"), author: Users["Ante"]),
                RandomPost(createdAt: DateTime.Parse("2018-02-09"), author: Users["Mate"]),
                RandomPost(createdAt: DateTime.Parse("2018-01-18"), author: Users["Ante"]),
                RandomPost(createdAt: DateTime.Parse("2018-01-17"), author: Users["Mate"]),  
            };
        }
    }
}
