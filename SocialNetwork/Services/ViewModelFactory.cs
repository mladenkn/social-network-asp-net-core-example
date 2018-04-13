using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Interface.Models.Entities;
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

            return new PostViewModel
            {
                PostId = post.Id,
                DislikesCount = post.DislikesCount,
                LikesCount = post.LikesCount,
                Heading = post.Heading,
                PublishedAt = post.CreatedAt,
                Text = post.Text,

                CanEdit = isCurrentUserAuthor,
                CanDelete = isCurrentUserAuthor,
                CanLike = !isCurrentUserAuthor,
                CanDislike = !isCurrentUserAuthor,

                Author = (post.Author.ProfileImageUrl, post.Author.UserName)
            };
        }
    }
}
