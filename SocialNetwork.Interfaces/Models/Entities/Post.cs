using System;
using Utilities;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SocialNetwork.Interface.Models.Entities
{
    public class Post : IEntity<long>
    {
        public Post()
        {
            _Ratings.SyncWith(
                c2: (ObservableCollection<User>)LikedBy, 
                c1Map: it => it.User, 
                c2Map: it => new _Rating
                {
                    Post = this,
                    PostId = Id,
                    User = it,
                    UserId = it.Id,
                    RatingType = _Rating.Type.Like
                },
                c1Filter: it => it.RatingType == _Rating.Type.Like
            );

            _Ratings.SyncWith(
                c2: (ObservableCollection<User>)DislikedBy,
                c1Map: it => it.User,
                c2Map: it => new _Rating
                {
                    Post = this,
                    PostId = Id,
                    User = it,
                    UserId = it.Id,
                    RatingType = _Rating.Type.Dislike
                },
                c1Filter: it => it.RatingType == _Rating.Type.Dislike
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

        public ObservableCollection<_Rating> _Ratings { get; } = new ObservableCollection<_Rating>();

        public ICollection<User> LikedBy { get; } = new ObservableCollection<User>();

        public ICollection<User> DislikedBy { get; } = new ObservableCollection<User>();
    }
}
