using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Web.Models
{
    public class UpdatePostModel
    {
        [Required]
        public long? Id { get; set; }

        public string Text { get; set; }

        public string Heading { get; set; }

        public bool Like { get; set; } = false;

        public bool Dislike { get; set; } = false;

        public bool UnLike { get; set; } = false;

        public bool UnDislike { get; set; } = false;
    }
}
