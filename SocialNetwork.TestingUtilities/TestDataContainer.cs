using System;
using System.Collections.Generic;
using SocialNetwork.Models;

namespace SocialNetwork.DevelopmentUtilities
{
    public class TestDataContainer
    {
        public IReadOnlyDictionary<string, User> Users { get; set; }
        public IReadOnlyList<Post> Posts { get; set; }
        
        public string DummyPassword { get; } = "a1234567";

        public TestDataContainer()
        {
            Users = new Dictionary<string, User>
            {
                ["Frane"] = Generator.RandomUser(userName: "Frane"),
                ["Mate"] = Generator.RandomUser(userName: "Mate"),
                ["Ante"] = Generator.RandomUser(userName: "Ante"),
            };

            Posts = new[]
            {
                Generator.RandomPost(createdAt: DateTime.Parse("2018-03-18"), author: Users["Mate"]),
                Generator.RandomPost(createdAt: DateTime.Parse("2018-03-17"), author: Users["Mate"]),
                Generator.RandomPost(createdAt: DateTime.Parse("2018-03-10"), author: Users["Frane"]),
                Generator.RandomPost(createdAt: DateTime.Parse("2018-02-20"), author: Users["Frane"]),
                Generator.RandomPost(createdAt: DateTime.Parse("2018-02-10"), author: Users["Ante"]),
                Generator.RandomPost(createdAt: DateTime.Parse("2018-02-09"), author: Users["Mate"]),
                Generator.RandomPost(createdAt: DateTime.Parse("2018-01-18"), author: Users["Ante"]),
                Generator.RandomPost(createdAt: DateTime.Parse("2018-01-17"), author: Users["Mate"]),  
            };
        }
    }
}
