using SocialNetwork.Domain.PostRatings;
using SocialNetwork.Domain.Users;
using System;
using System.Collections.Generic;

namespace SocialNetwork.Domain.Posts
{
    public class PostDtoLarge
    {
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserDtoBasic Author { get; set; }
        public string Text { get; set; }
        public IReadOnlyCollection<PostRatingDtoBasic> Ratings { get; set; }
        public IReadOnlyCollection<PostAction> Actions { get; set; }
    }
}
