using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Utilities;

namespace SocialNetwork.Models
{
    public class Post : IEntity<long>
    {
        public long Id { get; set; }

        public string Heading { get; set; }

        public string Text { get; set; }

        public string AuthorId { get; set; }

        public int LikesCount { get; set; }

        public int DislikesCount { get; set; }

        public DateTime CreatedAt { get; set; }

        public User Author { get; set; }

        public ICollection<PostRating> Ratings { get; } = new HashSet<PostRating>();

        public bool IsRatedByUser(string userId) => Ratings.Any(it => it.UserId == userId);

        public void AddRating(string userId, PostRating.Type type)
        {
            new PostRating
            {
                Post = this,
                PostId = Id,
                RatingType = type,
                UserId = userId
            }
            .Also(Ratings.Add);
        }
    }
}
