using System;
using Bogus;
using SocialNetwork.Interface.Models.Entities;

namespace SocialNetwork.DevelopmentUtilities
{
    public static class Generator
    {
        private static readonly Random Rand = new Random();
        private static readonly Faker _faker = new Faker();

        public static User GenerateUser(string id = null, string userName = null, string email = null,
            string profileImgUrl = null)
        {
            return new User
            {
                Id = id,
                ProfileImageUrl = profileImgUrl ?? _faker.Internet.Avatar(),
                UserName = userName ?? _faker.Internet.UserName(),
                Email = email ?? _faker.Internet.Email()
            };
        }

        public static Post GeneratePost(long id = 0, string heading = null, DateTime? createdAt = null,
            string text = null,
            User author = null, int? likesCount = null, int? dislikesCount = null)
        {
            author = author ?? GenerateUser();

            var post = new Post
            {
                Id = id,
                Text = text ?? _faker.Lorem.Paragraph(),
                Author = author,
                AuthorId = author.Id,
                LikesCount = likesCount ?? Rand.Next(10),
                DislikesCount = dislikesCount ?? Rand.Next(10),
                CreatedAt = createdAt ?? DateTime.Today.AddDays(-1 * Rand.Next(100)),
                Heading = heading ?? _faker.Lorem.Sentence(Rand.Next(1, 4))
            };

            return post;
        }

        public static T RandomEnumValue<T>() where T : struct => _faker.PickRandom<T>();
    }
}