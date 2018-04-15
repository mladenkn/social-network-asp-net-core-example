using SocialNetwork.Interface.Models.Entities;

namespace SocialNetwork.Interface.Models
{
    public class PostRating
    {
        public enum Type { Like, Dislike }

        public long PostId { get; set; }
        public string UserId { get; set; }
        public Type RatingType { get; set; }

        public User User { get; set; }
        public Post Post { get; set; }
    }
}
