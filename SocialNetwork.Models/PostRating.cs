namespace SocialNetwork.Models
{
    public class PostRating
    {
        public enum Type { Like, Dislike }

        public long PostId { get; set; }
        public string UserId { get; set; }
        public Type RatingType { get; set; }
    }
}
