using System;

namespace SocialNetwork.Interface.Models
{
    [Flags]
    public enum PostActions
    {
        None = 0,
        Like = 1,
        Dislike = 2,
        Edit = 4,
        Delete = 8
    }
}
