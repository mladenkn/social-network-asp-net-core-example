using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace SocialNetwork.Models
{
    public class User : IdentityUser, IEntity<string>
    {
        public string ProfileImageUrl { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}
