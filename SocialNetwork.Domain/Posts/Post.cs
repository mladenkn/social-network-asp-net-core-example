using System;
using System.Collections.Generic;
using ApplicationKernel.Domain;
using SocialNetwork.Domain.PostRatings;
using SocialNetwork.Domain.Users;

namespace SocialNetwork.Domain.Posts
{ 
    public class Post : IEntity, IDeletable
    {
        public long Id { get; set; }
        public string AuthorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Text { get; set; }

        public User Author { get; set; }
        public IReadOnlyCollection<PostRating> Ratings { get; set; }

        public bool IsDeleted { get; set; }
    }
}