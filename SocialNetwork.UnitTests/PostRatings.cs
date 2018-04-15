using System.Collections.Generic;
using System.Linq;
using SocialNetwork.DevelopmentUtilities;
using SocialNetwork.Interface.Models.Entities;
using Utilities;
using Xunit;
using static Utilities.Utils;
using Assert = Xunit.Assert;

namespace SocialNetwork.UnitTests
{
    public class PostRatings
    {
        [Fact]
        public void LikesAndDislikesToRatings()
        {
            var users = CollectionUtils.NewArray(() => Generator.GenerateUser(), 1000);
            var post = Generator.GeneratePost();

            DevelopmentUtilities.DevelopmentUtilities.AddRandomRatings(post, users, 500);
            DevelopmentUtilities.DevelopmentUtilities.RemoveRandomRatings(post);

            CheckRatingsLikesAndDislikes(post);
        }

        [Fact]
        public void RatingsToLikesAndDislikes()
        {
            IReadOnlyList<User> users = CollectionUtils.NewArray(() => Generator.GenerateUser(), 1000);
            var post = Generator.GeneratePost();

            var usedUsers = new HashSet<User>();

            Loop(Random.Next(500), delegate
            {
                var user = users.RandomElement(it => !usedUsers.Contains(it));
                usedUsers.Add(user);
                var rating = new _Rating
                {
                    Post = post,
                    PostId = post.Id,
                    RatingType = Generator.RandomEnumValue<_Rating.Type>(),
                    User = user,
                    UserId = user.Id
                };
                post._Ratings.Add(rating);
            });

            CheckRatingsLikesAndDislikes(post);
        }

        private void CheckRatingsLikesAndDislikes(Post post)
        {
            var userThatLikedIds =
                post._Ratings
                    .Where(it => it.RatingType == _Rating.Type.Like)
                    .Select(it => it.UserId);

            var userThatDislikedIds =
                post._Ratings
                    .Where(it => it.RatingType == _Rating.Type.Dislike)
                    .Select(it => it.UserId);

            var usersThatLikedIds2 = post.LikedBy.Select(it => it.Id);
            var usersThatDislikedIds2 = post.DislikedBy.Select(it => it.Id);

            usersThatLikedIds2.HasSameContentAs(userThatLikedIds).Also(Assert.True);
            usersThatDislikedIds2.HasSameContentAs(userThatDislikedIds).Also(Assert.True);
        }
    }
}
