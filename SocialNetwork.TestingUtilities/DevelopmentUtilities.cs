using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SocialNetwork.Interface.Models.Entities;
using Utilities;

namespace SocialNetwork.DevelopmentUtilities
{
    public static class DevelopmentUtilities
    {
        public static void AddRandomRatings(Post post, IReadOnlyList<User> users, int maxRatingsCount)
        {
            var usedUsers = new HashSet<User>();

            Utils.Loop(maxRatingsCount, delegate
            {
                var user = users.RandomElement(it => !usedUsers.Contains(it));
                usedUsers.Add(user);

                Utils.Random
                    .PickOne(post.LikedBy, post.DislikedBy)
                    .Add(user);
            });

            Utils.Assert(users.ContainsAll(post.LikedBy));
            Utils.Assert(users.ContainsAll(post.DislikedBy));

            IEnumerable<User> allRatings = post.LikedBy.Concat(post.DislikedBy);
            IEnumerable<User> allRatingsDistinct = allRatings.Distinct();
            allRatings.SequenceEqual(allRatingsDistinct).Also(Utils.Assert);
        }
    }
}
