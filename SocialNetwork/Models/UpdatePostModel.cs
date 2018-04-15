using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SocialNetwork.Web.Constants;
using Utilities;

namespace SocialNetwork.Web.Models
{
    public class UpdatePostModel
    {
        [Required]
        public long? Id { get; set; }

        public string Text { get; set; }

        public int RateActionId { get; set; }

        public PostAction RateAction => (PostAction) RateActionId;
    }
}
