using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SocialNetwork.Models
{
    public class Post : IEntity<long>
    {
        public Post()
        {
            LikesBy = new PivotTableWrapper<PostRating, User>(
                Ratings,
                rating => rating.User, 
                rating => rating.RatingType == PostRating.Type.Like
            );

            DislikesBy = new PivotTableWrapper<PostRating, User>(
                Ratings,
                rating => rating.User,
                rating => rating.RatingType == PostRating.Type.Dislike
            );
        }

        public long Id { get; set; }

        public string Heading { get; set; }

        public string Text { get; set; }

        public string AuthorId { get; set; }

        public int LikesCount { get; set; }

        public int DislikesCount { get; set; }

        public DateTime CreatedAt { get; set; }

        public User Author { get; set; }

        public ICollection<PostRating> Ratings { get; } = new HashSet<PostRating>();

        public ICollection<User> LikesBy { get; }

        public ICollection<User> DislikesBy { get; set; }
    }
}
