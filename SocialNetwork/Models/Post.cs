using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialNetwork.Models
{
    public class Post : IEntity<long>
    {
        public long Id { get; set; }

        public string Heading { get; set; }

        public string Text { get; set; }

        public long AuthorId { get; set; }

        public int LikesCount { get; set; }

        public int DislikesCount { get; set; }

        public DateTime CreatedAt { get; set; }

        [ForeignKey("AuthorId")]
        public User Author { get; set; }
    }
}
