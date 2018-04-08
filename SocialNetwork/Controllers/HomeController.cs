﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.DevelopmentUtilities;
using SocialNetwork.Interface.DAL;
using SocialNetwork.Interface.Models;
using SocialNetwork.Interface.Models.Entities;
using SocialNetwork.Interface.Services;
using SocialNetwork.Web.ServiceInterfaces;
using SocialNetwork.Web.ViewModels;
using SocialNetwork.Web.Constants;
using SocialNetwork.Web.Models;
using Utilities;
using SocialNetwork.Web.Utilities;

namespace SocialNetwork.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IViewRendererService _renderer;
        private readonly IHub _hub;
        private readonly IPostsRepository _posts;
        private readonly IDatabaseOperations _dbOps;
        private readonly IRepository<User> _users;
        private readonly PermissionsCalculator _permissionsCalculator = new PermissionsCalculator();

        public HomeController(IViewRendererService renderer, IHub hub, IRepository<User> users,
                              IPostsRepository posts, IDatabaseOperations dbOps)
        {
            _renderer = renderer;
            _hub = hub;
            _posts = posts;
            _dbOps = dbOps;
            _users = users;
        }

        private PostViewModel CreatePostViewModel(Post post, string currentUserId)
        {
            PostActions permissions = _permissionsCalculator.Calculate(currentUserId, post);

            return new PostViewModel
            {
                PostId = post.Id,
                DislikesCount = post.DislikesCount,
                LikesCount = post.LikesCount,
                Heading = post.Heading,
                PublishedAt = post.CreatedAt,
                Text = post.Text,

                CanEdit = permissions.HasFlag(PostActions.EditContent),
                CanDelete = permissions.HasFlag(PostActions.Delete),
                CanLike = permissions.HasFlag(PostActions.Like),
                CanDislike = permissions.HasFlag(PostActions.Dislike),

                Author = (post.Author.ProfileImageUrl, post.Author.UserName)
            };
        }

        private Task<User> GetCurrentUser() => _users.GetOne(it => it.UserName == User.Identity.Name);

        public async Task<IActionResult> Index()
        {
            User currentUser = await GetCurrentUser();
            var posts = await _posts.GetMany(count: 5, order: PostsOrder.CreatedAtDescending, propsToInclude: "Author");
            var postsViewModels = posts
                .Select(it => CreatePostViewModel(it, currentUser.Id))
                .ToList().AsReadOnly();

            var vm = new HomeViewModel
            {
                Posts = postsViewModels,
                Title = "Social Network",
                ActivePage = Page.Home,
                Username = currentUser.UserName
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(string postText)
        {
            User author = await GetCurrentUser();
            Post post = Generator.GeneratePost(text: postText, createdAt: DateTime.Today, author: author, likesCount: 0, dislikesCount: 0);
            Post storedPost = _posts.Insert(post);

            Task saveChangesTask = _dbOps.SaveChangesAsync();
            var postVm = CreatePostViewModel(storedPost, author.Id);
            string postHtml = await _renderer.RenderPartialView("_Post", postVm);
            await saveChangesTask;
            await _hub.Emit(PostsEvents.PostPublished, postHtml);

            return Ok("");
        }

        [HttpPost]
        public async Task<IActionResult> LoadPostsAjax(int count, int skip, string[] propsToInclude)
        {
            string currentUserId = await GetCurrentUser().Map(it => it.Id);

            var posts = await _posts.GetMany(
                order: PostsOrder.CreatedAtDescending,
                propsToInclude: propsToInclude,
                count: count,
                skip: skip
            );

            IEnumerable<Task<string>> renderingTasks =
                posts
                .Select(it => CreatePostViewModel(it, currentUserId))
                .Select(it => _renderer.RenderPartialView("_Post", it));

            var rendered = await Task.WhenAll(renderingTasks);
            return Ok(rendered);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePost(UpdatePostModel model)
        {
            var currentUserId = await GetCurrentUser().Map(it => it.Id);
            Post post = await _posts.GetOne(it => it.Id == model.Id, "Author");

            if (model.Text != null)
            {
                if (post.AuthorId == currentUserId)
                    post.Text = model.Text;
                else
                    return Forbid();
            }

            if (model.Heading != null)
            {
                if (post.AuthorId == currentUserId)
                    post.Heading = model.Heading;
                else
                    return Forbid();
            }

            if (model.AddLike)
            {
                if (post.AuthorId != currentUserId)
                {
                    post.LikesCount++;
                }
                else
                    return Forbid();
            }
            
            if (model.AddDislike)
            {
                if (post.AuthorId != currentUserId)
                {
                    post.DislikesCount++;
                }
                else
                    return Forbid();
            }

            _posts.Update(post);
    
            Task saveChangesTask = _dbOps.SaveChangesAsync();
            var postVm = CreatePostViewModel(post, currentUserId);
            string postHtml = await _renderer.RenderPartialView("_Post", postVm);
            await saveChangesTask;
            await _hub.Emit(PostsEvents.PostChanged, postHtml);

            return Ok("");
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(long postId)
        {
            var post = await _posts.GetOne(postId);
            var currentUserId = await GetCurrentUser().Map(it => it.Id);

            if (post.AuthorId == currentUserId)
                _posts.Delete(post);
            else
                return Forbid();

            await _dbOps.SaveChangesAsync();
            await _hub.Emit(PostsEvents.PostDeleted, postId);

            return Ok("");
        }
    }
}
