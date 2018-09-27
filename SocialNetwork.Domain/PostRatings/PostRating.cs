using ApplicationKernel.Domain;
using SocialNetwork.Domain.Posts;
using SocialNetwork.Domain.Users;

namespace SocialNetwork.Domain.PostRatings
{
    public class PostRating : IEntity
    {
        public long Id { get; set; }
        public long PostId { get; set; }
        public string UserId { get; set; }
        public PostRatingType Type { get; set; }

        public User User { get; set; }
        public Post Post { get; set; }
    }

    public enum PostRatingType { Like = 0, Dislike = 1 }
}