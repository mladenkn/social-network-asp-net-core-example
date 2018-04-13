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
            _Ratings.CollectionChanged += (sender, args) =>
            {
                foreach (_Rating newItem in args.NewItems)
                {
                    if(newItem.RatingType == _Rating.Type.Like)
                        LikedBy.Add(newItem.User);

                    if (newItem.RatingType == _Rating.Type.Dislike)
                        DislikedBy.Add(newItem.User);
                }
            };
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
