using System;
using System.Collections.Generic;
using System.Text;
using SocialNetwork.Interface.Models.Entities;
using Utilities;

namespace SocialNetwork.DevelopmentUtilities
{
    public static class DevelopmentUtilities
    {
        public static void AddRandomRatings(Post post, IReadOnlyList<User> users, int count)
        {
            var usedUsers = new HashSet<User>();

            Utils.Loop(Utils.Random.Next(count), delegate
            {
                var user = users.RandomElement(it => !usedUsers.Contains(it));
                usedUsers.Add(user);

                Utils.Random
                    .PickOne(post.LikedBy, post.DislikedBy)
                    .Add(user);
            });
        }
    }
}
