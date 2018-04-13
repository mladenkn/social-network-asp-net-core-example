using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SocialNetwork.Interface.Models.Entities
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

        public ObservableCollection<_Rating> _Ratings { get; } = new ObservableCollection<_Rating>();
    }
}
