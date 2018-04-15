using System.Collections.Generic;
using System.Linq;
using SocialNetwork.Interface.Models.Entities;
using Utilities;

namespace SocialNetwork.DevelopmentUtilities
{
    public static class DevelopmentUtilities
    {
        public static void AddRandomRatings(Post post, IReadOnlyList<User> users, int maxRatingsCount)
        {
            var usedUsers = new HashSet<User>();

            Utils.Loop(Utils.Random.Next(0, maxRatingsCount), delegate
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

        public static void RemoveRandomRatings(Post post)
        {
            var likersToRemoveIds =
                post.LikedBy
                    .Where(it => Utils.Random.NextBool())
                    .Select(it => it.Id);

            var dislikersToRemoveIds =
                post.DislikedBy
                    .Where(it => Utils.Random.NextBool())
                    .Select(it => it.Id);

            post.LikedBy.RemoveIf(it => likersToRemoveIds.Contains(it.Id));
            post.DislikedBy.RemoveIf(it => dislikersToRemoveIds.Contains(it.Id));
        }
    }
}
