using System;
using System.Collections.Generic;
using System.Linq;
using SocialNetwork.Models;
using Utilities;

namespace SocialNetwork.DevelopmentUtilities
{
    public class TestDataContainer
    {
        public IReadOnlyDictionary<string, User> Users { get; set; }
        public IReadOnlyList<Post> Posts { get; set; }
        
        public string DummyPassword { get; } = "a1234567";

        public TestDataContainer()
        {
            Users = 
                new[] {"Frane", "Mate", "Ante", "Mladen"}
                    .Select(it => Generator.RandomUser(userName: it))
                    .Select(it => new KeyValuePair<string, User>(it.UserName, it))
                    .Let(it => new Dictionary<string, User>(it));

            Posts =
                new[]
                {
                    (DateTime.Parse("2018-03-18"), Users["Mladen"]),
                    (DateTime.Parse("2018-03-17"), Users["Mate"]),
                    (DateTime.Parse("2018-03-10"), Users["Frane"]),
                    (DateTime.Parse("2018-02-20"), Users["Frane"]),
                    (DateTime.Parse("2018-02-10"), Users["Mladen"]),
                    (DateTime.Parse("2018-02-09"), Users["Mate"]),
                    (DateTime.Parse("2018-01-18"), Users["Ante"]),
                    (DateTime.Parse("2018-01-17"), Users["Mate"])
                }
                .Select(it => Generator.RandomPost(createdAt: it.Item1, author: it.Item2))
                .ToList();
        }
    }
}
