namespace SocialNetwork.Interface.Models.Entities
{
    public class _Rating
    {
        public enum Type { Like = 0, Dislike = 1 }

        public string UserId { get; set; }
        public long PostId { get; set; }

        public User User { get; set; }
        public Post Post { get; set; }

        public int TypeId { get; set; }

        public Type RatingType
        {
            get => (Type) TypeId;
            set => TypeId = (int) value;
        }
    }
}
