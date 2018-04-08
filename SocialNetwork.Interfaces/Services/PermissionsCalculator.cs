using System;
using SocialNetwork.Interface.Models;
using SocialNetwork.Interface.Models.Entities;

namespace SocialNetwork.Interface.Services
{
    public class PermissionsCalculator
    {
        public PostActions Calculate(string userId, Post post)
        {
            bool isUserPostAuthor = post.AuthorId == userId;

            PostActions ret = PostActions.None;

            if (isUserPostAuthor)
            {
                ret = ret | PostActions.EditContent | PostActions.EditHeading | PostActions.Delete;
            }
            else
            {
                if(!post.IsLikedByUser(userId))
                    ret = ret | PostActions.Like;
                else
                    ret = ret | PostActions.Dislike;
            }

            return ret;
        }
    }
}
