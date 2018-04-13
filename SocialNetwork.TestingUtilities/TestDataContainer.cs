using System;
using System.Collections.Generic;
using System.Linq;
using SocialNetwork.Interface.Models.Entities;
using Utilities;

namespace SocialNetwork.DevelopmentUtilities
{
    public class TestDataContainer
    {
        public IReadOnlyDictionary<string, User> Users { get; set; }
        public IReadOnlyList<Post> Posts { get; set; }
        
        public string DummyPassword => "a1234567";

        public TestDataContainer()
        {
            Users = 
                new[] {"Frane", "Mate", "Ante", "Mladen"}
                    .Select(it => Generator.GenerateUser(userName: it))
                    .Select(it => new KeyValuePair<string, User>(it.UserName, it))
                    .Let(it => new Dictionary<string, User>(it));

            Posts = 
                new []
                {
                    Generator.GeneratePost(createdAt: new DateTime(2018, 3, 18), author: Users["Mladen"]),
                    Generator.GeneratePost(createdAt: new DateTime(2018, 3, 17), author: Users["Frane"]),
                    Generator.GeneratePost(createdAt: new DateTime(2018, 3, 20), author: Users["Ante"]),
                    Generator.GeneratePost(createdAt: new DateTime(2018, 2, 10), author: Users["Mate"]),
                    Generator.GeneratePost(createdAt: new DateTime(2018, 2, 8), author: Users["Frane"]),
                    Generator.GeneratePost(createdAt: new DateTime(2018, 2, 7), author: Users["Ante"]),
                    Generator.GeneratePost(createdAt: new DateTime(2018, 1, 18), author: Users["Mladen"]),
                    Generator.GeneratePost(createdAt: new DateTime(2018, 1, 17), author: Users["Mladen"]),
                }
                .ToList();
        }
    }
}
