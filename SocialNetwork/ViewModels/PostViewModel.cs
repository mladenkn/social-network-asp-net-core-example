using System;

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

        public (string ProfileImgUrl, string Username) Author { get; set; }

        public RateActionArgs LikeActionArgs { get; set; }
        public RateActionArgs DislikeActionArgs { get; set; }

        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }

        public class RateActionArgs
        {
            public bool ShowBtn { get; set; }
            public bool Enabled { get; set; }
        }
    }
}
