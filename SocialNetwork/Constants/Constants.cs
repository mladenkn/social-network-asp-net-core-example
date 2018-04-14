namespace SocialNetwork.Web.Constants
{
    public static class PostsEvents
    {
        public const string PostPublished = nameof(PostPublished);
        public const string PostChanged = nameof(PostChanged);
        public const string PostDeleted = nameof(PostDeleted);
    }

    public static class PostActions
    {
        public const string Published = nameof(Published);
        public const string Edit = nameof(Edit);
        public const string Delete = nameof(Delete);
        public const string Like = nameof(Like);
        public const string Dislike = nameof(Dislike);
        public const string UnLike = nameof(UnLike);
        public const string UnDislike = nameof(UnDislike);
    }
}
