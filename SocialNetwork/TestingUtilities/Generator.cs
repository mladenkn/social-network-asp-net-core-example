using System;
using System.Text;
using SocialNetwork.Models;
using Utilities;
using static Utilities.CollectionUtils;

namespace SocialNetwork.TestingUtilities
{
    public static class Generator
    {
        private static readonly Random Rand = new Random();

        private static readonly char[] AllowedChars = "qwertzuiopšđžćčlkjhgfddsayxcvbnm1234567890".ToCharArray();

        private static readonly string[] AllowedImageUrls = {
            "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRF8PAScCitwtHRTvbPQukVsaqwi5Ko1x-5iIzn7CHY45NhIeDA",
            "https://vignette.wikia.nocookie.net/mrmen/images/5/52/Small.gif/revision/latest?cb=20100731114437",
            "http://blog.graphisoftus.com/wp-content/uploads/2012-blog-bim-too-big.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/f/f9/Wiktionary_small.svg/350px-Wiktionary_small.svg.png",
            "https://www.va.gov/OSDBU/images/business.png"
        };

        public static char RandomChar() => AllowedChars.RandomElement();

        public static string RandomImage() => AllowedImageUrls.RandomElement();

        public static string RandomString(int len) =>
            NewEnumerable(RandomChar, len)
                .Let(string.Concat);

        public static string RandomString(int minLength, int maxLength) =>
            Utils.Random.Next(minLength, maxLength)
                .Let(RandomString);

        public static string RandomParagraph(int? wordsCount = null)
        {
            wordsCount = wordsCount ?? Utils.Random.Next(10, 50);
            string RandomText() => RandomString(3, 15) + " ";
            return NewEnumerable(RandomText, wordsCount.Value)
                .Let(string.Concat);
        }

        public static User RandomUser(string id = null, string userName = null)
        {
            return new User
            {
                Id = id,
                ProfileImageUrl = RandomImage(),
                UserName = userName ?? RandomString(5, 15)
            };
        }

        public static Post RandomPost(long? id = null, string heading = null, DateTime? createdAt = null, string text = null,
                                      User author = null, int? likesCount = null, int? dislikesCount = null)
        {
            author = author ?? RandomUser();

            var post = new Post
            {
                Id = id ?? 0,
                Text = text ?? RandomParagraph(),
                Author = author,
                AuthorId = author.Id,
                LikesCount = likesCount?? Rand.Next(10),
                DislikesCount = dislikesCount ?? Rand.Next(10),
                CreatedAt = createdAt ?? DateTime.Today.AddDays(-1 * Rand.Next(100)),
                Heading = heading ?? RandomString(5, 15)
            };

            return post;
        }
    }
}