using System.Collections.Generic;
using ApplicationKernel.Domain;
using SocialNetwork.Domain.Posts;

namespace SocialNetwork.Domain.Users
{
    public class User : IEntity
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string ProfileImageUrl { get; set; }

        public IReadOnlyCollection<Post> Posts { get; set; }
    }
}