using SocialNetwork.Domain.PostRatings;
using System.Collections.Generic;

namespace SocialNetwork.Domain.Posts
{
    public delegate IReadOnlyCollection<PostAction> GetUserActionsForPost(string userId, Post post, PostRating currentRating);
}
