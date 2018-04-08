using System;

namespace SocialNetwork.Interface.Models
{
    [Flags]
    public enum PostActions
    {
        None = 0,
        Like = 1,
        Dislike = 2,
        EditContent = 4,
        EditHeading = 8,
        Delete = 16
    }
}
