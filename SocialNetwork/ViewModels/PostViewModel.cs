using System;
using System.Collections.Generic;
using SocialNetwork.Interface.Models.Entities;

namespace SocialNetwork.Web.ViewModels
{
    public class PostViewModel
    {
        public long PostId { get; set; }
        public string Text { get; set; }
        public DateTime PublishedAt { get; set; }
        public int LikesCount { get; set; }
        public int DislikesCount { get; set; }
        public string Heading { get; set; }
        public IReadOnlyCollection<User> LikedBy { get; set; }
        public IReadOnlyCollection<User> DislikedBy { get; set; }

        public (string ProfileImgUrl, string Username) Author { get; set; }

        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool CanLike { get; set; }
        public bool CanDislike { get; set; }
    }
}
