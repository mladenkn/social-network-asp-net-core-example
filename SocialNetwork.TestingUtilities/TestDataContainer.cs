using System;
using System.Collections.Generic;
using System.Linq;
using SocialNetwork.Interface.Models.Entities;
using Utilities;

namespace SocialNetwork.DevelopmentUtilities
{
    public class TestDataContainer
    {
        public IReadOnlyDictionary<string, User> Users { get; }
        public IReadOnlyList<User> UsersList { get; }
        public IReadOnlyList<Post> Posts { get; }
        
        public string DummyPassword => "a1234567";

        public TestDataContainer()
        {
            Users = 
                new[] {"Frane", "Mate", "Ante", "Mladen", "Josip", "Stipe", "Luka", "Ivan"}
                    .Select(it => Generator.GenerateUser(userName: it))
                    .Select(it => new KeyValuePair<string, User>(it.UserName, it))
                    .Let(it => new Dictionary<string, User>(it));

            UsersList = Users.Values.ToList();

            DateTime lastKnowDate = new DateTime(2018, 1, 18);
            DateTime GenerateDate() => Generator.DateBefore(lastKnowDate);

            Posts = 
                new []
                {
                    Generator.GeneratePost(createdAt: new DateTime(2018, 3, 18), author: Users["Mladen"]),
                    Generator.GeneratePost(createdAt: new DateTime(2018, 3, 17), author: Users["Frane"]),
                    Generator.GeneratePost(createdAt: new DateTime(2018, 3, 20), author: Users["Ante"]),
                    Generator.GeneratePost(createdAt: new DateTime(2018, 2, 10), author: Users["Mate"]),
                    Generator.GeneratePost(createdAt: new DateTime(2018, 2, 8), author: Users["Frane"]),
                    Generator.GeneratePost(createdAt: new DateTime(2018, 2, 7), author: Users["Ante"]),
                    Generator.GeneratePost(createdAt: lastKnowDate, author: Users["Mladen"]),

                    Generator.GeneratePost(createdAt: GenerateDate(), author: UsersList.RandomElement()),
                    Generator.GeneratePost(createdAt: GenerateDate(), author: UsersList.RandomElement()),
                    Generator.GeneratePost(createdAt: GenerateDate(), author: UsersList.RandomElement()),
                    Generator.GeneratePost(createdAt: GenerateDate(), author: UsersList.RandomElement()),
                    Generator.GeneratePost(createdAt: GenerateDate(), author: UsersList.RandomElement()),
                    Generator.GeneratePost(createdAt: GenerateDate(), author: UsersList.RandomElement()),
                    Generator.GeneratePost(createdAt: GenerateDate(), author: UsersList.RandomElement()),
                    Generator.GeneratePost(createdAt: GenerateDate(), author: UsersList.RandomElement()),
                    Generator.GeneratePost(createdAt: GenerateDate(), author: UsersList.RandomElement()),
                    Generator.GeneratePost(createdAt: GenerateDate(), author: UsersList.RandomElement()),
                    Generator.GeneratePost(createdAt: GenerateDate(), author: UsersList.RandomElement()),
                    Generator.GeneratePost(createdAt: GenerateDate(), author: UsersList.RandomElement()),
                    Generator.GeneratePost(createdAt: GenerateDate(), author: UsersList.RandomElement()),
                    Generator.GeneratePost(createdAt: GenerateDate(), author: UsersList.RandomElement()),
                    Generator.GeneratePost(createdAt: GenerateDate(), author: UsersList.RandomElement()),
                    Generator.GeneratePost(createdAt: GenerateDate(), author: UsersList.RandomElement()),
                    Generator.GeneratePost(createdAt: GenerateDate(), author: UsersList.RandomElement()),
                    Generator.GeneratePost(createdAt: GenerateDate(), author: UsersList.RandomElement()),
                    Generator.GeneratePost(createdAt: GenerateDate(), author: UsersList.RandomElement()),
                    Generator.GeneratePost(createdAt: GenerateDate(), author: UsersList.RandomElement()),
                    Generator.GeneratePost(createdAt: GenerateDate(), author: UsersList.RandomElement()),
                    Generator.GeneratePost(createdAt: GenerateDate(), author: UsersList.RandomElement()),
                    Generator.GeneratePost(createdAt: GenerateDate(), author: UsersList.RandomElement()),
                    Generator.GeneratePost(createdAt: GenerateDate(), author: UsersList.RandomElement()),
                }
                .ToList();
        }
    }
}
