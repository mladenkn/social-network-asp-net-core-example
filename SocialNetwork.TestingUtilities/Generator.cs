using System;
using Bogus;
using SocialNetwork.Models;
using Utilities;
using Utils = Utilities.Utils;

namespace SocialNetwork.DevelopmentUtilities
{
    public static class Generator
    {
        private static readonly Random Rand = new Random();
        private static readonly Faker _faker = new Faker();

        public static User RandomUser(string id = null, string userName = null, string email = null)
        {
            return new User
            {
                Id = id,
                ProfileImageUrl = _faker.Internet.Avatar(),
                UserName = userName ?? _faker.Internet.UserName(),
                Email = email ?? _faker.Internet.Email()
            };
        }

        public static Post RandomPost(long? id = null, string heading = null, DateTime? createdAt = null, string text = null,
                                      User author = null, int? likesCount = null, int? dislikesCount = null)
        {
            author = author ?? RandomUser();

            var post = new Post
            {
                Id = id ?? 0,
                Text = text ?? _faker.Lorem.Paragraph(),
                Author = author,
                AuthorId = author.Id,
                LikesCount = likesCount?? Rand.Next(10),
                DislikesCount = dislikesCount ?? Rand.Next(10),
                CreatedAt = createdAt ?? DateTime.Today.AddDays(-1 * Rand.Next(100)),
                Heading = heading ?? _faker.Lorem.Sentence(Rand.Next(1, 4))
            };

            return post;
        }
    }
}