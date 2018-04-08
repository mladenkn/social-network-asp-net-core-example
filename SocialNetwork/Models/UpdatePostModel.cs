using System;
using SocialNetwork.Interface.Models;

namespace SocialNetwork.Web.Models
{
    public class UpdatePostModel
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public string Heading { get; set; }
        public bool AddLike { get; set; } = false;
        public bool AddDislike { get; set; } = false;

        public PostActions GetActions()
        {
            throw new NotImplementedException();
        }
    }
}
