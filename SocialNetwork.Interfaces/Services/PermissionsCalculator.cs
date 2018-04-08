using System;
using SocialNetwork.Interface.Models;
using SocialNetwork.Interface.Models.Entities;

namespace SocialNetwork.Interface.Services
{
    public class PermissionsCalculator
    {
        public PostActions Calculate(string userId, Post post)
        {
            if (post.AuthorId == userId)
                return PostActions.EditContent | PostActions.EditHeading | PostActions.Delete;
            else
                return PostActions.Like | PostActions.Dislike;
        }
    }
}
