using System;
using System.Collections.Generic;
using SocialNetwork.Interface.Models.Entities;
using SocialNetwork.Web.Constants;

namespace SocialNetwork.Web.ViewModels
{
    public class PostViewModel
    {
        public long PostId { get; set; }
        public string Text { get; set; }
        public DateTime PublishedAt { get; set; }
        public string Heading { get; set; }
        public IReadOnlyCollection<User> LikedBy { get; set; }
        public IReadOnlyCollection<User> DislikedBy { get; set; }

        public (string ProfileImgUrl, string Username) Author { get; set; }

        public IReadOnlyCollection<PostAction> AllowedActions_ { get; set; }
    }
}
