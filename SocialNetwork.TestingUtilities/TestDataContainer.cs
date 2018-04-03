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
                new []
                {
                    (new DateTime(2018, 3, 18), "Mladen"),
                    (new DateTime(2018, 3, 17), "Frane"),
                    (new DateTime(2018, 3, 20), "Ante"),
                    (new DateTime(2018, 2, 10), "Mate"),
                    (new DateTime(2018, 2, 8), "Mladen"),
                    (new DateTime(2018, 2, 7), "Frane"),
                    (new DateTime(2018, 1, 18), "Ante"),
                    (new DateTime(2018, 1, 17), "Mladen"),
                }
                .Select(it => Generator.RandomPost(createdAt: it.Item1, author: Users[it.Item2]))
                .ToList();
        }
    }
}
