using System;
using System.Collections.Generic;
using System.Text;
using SocialNetwork.Interface.Models.Entities;
using Utilities;

namespace SocialNetwork.DevelopmentUtilities
{
    public static class DevelopmentUtilities
    {
        public static void AddRandomRatings(Post post, IReadOnlyList<User> users, int maxRatingsCount)
        {
            int likesCount = Utils.Random.Next(maxRatingsCount / 2, maxRatingsCount);
            int dislikesCount = maxRatingsCount - likesCount;
            Utils.Assert(likesCount + dislikesCount == maxRatingsCount);

            AddRandomLikes(post, users, likesCount);
            AddRandomDislikes(post, users, dislikesCount);
        }

        public static void AddRandomLikes(Post post, IReadOnlyList<User> users, int maxLikesCount)
        {
            var usedUsers = new HashSet<User>();

            Utils.Loop(Utils.Random.Next(maxLikesCount), delegate
            {
                var user = users.RandomElement(it => !usedUsers.Contains(it));
                usedUsers.Add(user);
                post.LikedBy.Add(user);
            });
        }

        public static void AddRandomDislikes(Post post, IReadOnlyList<User> users, int maxDislikesCount)
        {
            var usedUsers = new HashSet<User>();

            Utils.Loop(Utils.Random.Next(maxDislikesCount), delegate
            {
                var user = users.RandomElement(it => !usedUsers.Contains(it));
                usedUsers.Add(user);
                post.DislikedBy.Add(user);
            });
        }
    }
}
