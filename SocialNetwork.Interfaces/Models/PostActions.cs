using System;

namespace SocialNetwork.Interfaces.Models
{
    [Flags]
    public enum PostActions
    {
        Like = 1,
        Dislike = 2,
        Edit = 4,
        Delete = 8
    }
}
