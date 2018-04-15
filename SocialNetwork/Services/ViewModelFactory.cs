using System.Collections.Generic;
using System.Linq;
using SocialNetwork.Interface.Constants;
using SocialNetwork.Interface.Models.Entities;
using SocialNetwork.Web.Constants;
using SocialNetwork.Web.ViewModels;
using Utilities;

namespace SocialNetwork.Web.Services
{
    public class ViewModelFactory
    {
        public RegisterViewModel CreateRegisterViewModel(RegisterFormViewModel formModel = null) =>
            new RegisterViewModel
                {
                    ActivePage = Page.Account_Register,
                    Form = formModel ?? new RegisterFormViewModel(),
                    Title = "Register"
                };

        public LoginViewModel CreateLoginViewModel(LoginFormViewModel formModel = null) =>
            new LoginViewModel
                {
                    ActivePage = Page.Account_Login,
                    Form = formModel ?? new LoginFormViewModel(),
                    Title = "Login"
                };

        public PostViewModel CreatePostViewModel(Post post, string currentUserId)
        {
            bool isCurrentUserAuthor = post.AuthorId == currentUserId;

            bool hasUserLikedPost = post.LikedBy.Any(it => it.Id == currentUserId);
            bool hasUserDislikePost = post.DislikedBy.Any(it => it.Id == currentUserId);

            var allowedActions_ =
                new HashSet<PostAction>()
                    .Also(it =>
                    {
                        if (isCurrentUserAuthor)
                        {
                            it.Add(PostAction.Edit);
                            it.Add(PostAction.Delete);
                        }
                        else
                        {
                            it.Add(!hasUserLikedPost ? PostAction.Like : PostAction.UnLike);
                            it.Add(!hasUserDislikePost ? PostAction.Dislike : PostAction.UnDislike);
                        }
                    });

            return new PostViewModel
            {
                PostId = post.Id,
                Heading = post.Heading,
                PublishedAt = post.CreatedAt,
                Text = post.Text,
                LikedBy = post.LikedBy.AsReadOnly(),
                DislikedBy = post.DislikedBy.AsReadOnly(),
                AllowedActions = allowedActions_,
                Author = (post.Author.ProfileImageUrl, post.Author.UserName)
            };
        }
    }
}
