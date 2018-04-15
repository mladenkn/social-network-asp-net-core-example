using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SocialNetwork.Web.Constants;
using Utilities;

namespace SocialNetwork.Web.Models
{
    public class UpdatePostModel : IValidatableObject
    {
        [Required]
        public long? Id { get; set; }

        public string Text { get; set; }

        public int RateActionId { get; set; }

        public PostAction RateAction => (PostAction) RateActionId;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(!RateAction.EqualsAny(PostAction.None, PostAction.Like, PostAction.Dislike, PostAction.UnLike,
                PostAction.UnDislike))
                yield return new ValidationResult("Unallowed action type", 
                                                   new []{nameof(RateActionId), nameof(RateAction)});
        }
    }
}
