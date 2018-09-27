namespace SocialNetwork.Domain.Posts
{
    public enum PostEvent
    {
        PostPublished, PostChanged, PostDeleted
    }

    public enum PostAction
    {
        None = 0,
        Publish = 1,
        Edit = 2,
        Delete = 3,
        Like = 4,
        Dislike = 5,
        UnLike = 6,
        UnDislike = 7,
        View = 8
    }
}
