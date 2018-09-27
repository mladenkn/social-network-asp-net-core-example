using System.Collections.Generic;
using SocialNetwork.Domain.PostRatings;
namespace SocialNetwork.Domain.Posts
{
    public interface IPostPermissionProvider
    {
        bool CanPublish(string userId);
        bool CanEdit(string userId, Post post);
        bool CanDelete(string userId, Post post);
        bool CanLike(string userId, Post post, PostRating currentRating = null);
        bool CanDislike(string userId, Post post, PostRating currentRating = null);
        bool CanUnlike(string userId, Post post, PostRating currentRating = null);
        bool CanUndislike(string userId, Post post, PostRating currentRating = null);
        bool CanSee(string userId, Post post);
        IReadOnlyCollection<PostAction> GetAllowedPostActionsForUser(string userId, Post post, PostRating currentRating = null);
    }
}