using System.Collections.Generic;
using SocialNetwork.Domain.PostRatings;

namespace SocialNetwork.Domain.Posts
{
    public class PostPermissionProvider : IPostPermissionProvider
    {
        public bool CanPublish(string userId) => true;

        public bool CanEdit(string userId, Post post) => post.AuthorId == userId;

        public bool CanDelete(string userId, Post post) => post.AuthorId == userId;

        public bool CanLike(string userId, Post post, PostRating usersRating = null)
        {
            return CanRate(userId, post, usersRating);
        }

        public bool CanDislike(string userId, Post post, PostRating usersRating = null)
        {
            return CanRate(userId, post, usersRating);
        }

        public bool CanRate(string userId, Post post, PostRating usersRating = null)
        {
            return post.AuthorId != userId && usersRating != null;
        }

        public bool CanUnlike(string userId, Post post, PostRating usersRating = null)
        {
            var hasLikedAllready = usersRating != null && usersRating.Type == PostRatingType.Like;
            return post.AuthorId != userId && hasLikedAllready;
        }

        public bool CanUndislike(string userId, Post post, PostRating usersRating = null)
        {
            var hasUndisliked = usersRating != null && usersRating.Type == PostRatingType.Dislike;
            return post.AuthorId != userId && hasUndisliked;
        }

        public bool CanSee(string userId, Post post) => true;

        public IReadOnlyCollection<PostAction> GetAllowedPostActionsForUser(string userId, Post post, PostRating currentRating)
        {
            var ret = new HashSet<PostAction>();

            bool hasDisliked = false, hasLiked = false;

            switch (currentRating?.Type)
            {
                case PostRatingType.Like:
                    hasLiked = true;
                    break;
                case PostRatingType.Dislike:
                    hasDisliked = true;
                    break;
            }

            if (CanPublish(userId))
                ret.Add(PostAction.Publish);

            if (CanEdit(userId, post))
                ret.Add(PostAction.Edit);

            if (CanDelete(userId, post))
                ret.Add(PostAction.Delete);

            if ((!hasLiked || !hasDisliked) && post.AuthorId != userId)
                ret.Add(PostAction.Like);

            if ((!hasLiked || !hasDisliked) && post.AuthorId != userId)
                ret.Add(PostAction.Dislike);

            if (hasLiked && post.AuthorId != userId)
                ret.Add(PostAction.UnLike);

            if (hasDisliked && post.AuthorId != userId)
                ret.Add(PostAction.UnDislike);

            ret.Add(PostAction.View);

            return ret;
        }
    }
}
