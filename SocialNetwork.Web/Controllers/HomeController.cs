using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.DevelopmentUtilities;
using SocialNetwork.Interface.Constants;
using SocialNetwork.Interface.DAL;
using SocialNetwork.Interface.Models.Entities;
using SocialNetwork.Interface.Services;
using SocialNetwork.Web.Constants;
using SocialNetwork.Web.ServiceInterfaces;
using SocialNetwork.Web.ViewModels;
using SocialNetwork.Web.Models;
using SocialNetwork.Web.Services;
using SocialNetwork.Web.Utilities;
using Utilities;

namespace SocialNetwork.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IViewRendererService _renderer;
        private readonly IPostsHub _postsHub;
        private readonly IPostsRepository _posts;
        private readonly IDatabaseOperations _dbOps;
        private readonly ViewModelFactory _viewModelFactory;
        private readonly IRepository<User> _users;

        public HomeController(IViewRendererService renderer, IPostsHub postsHub, IRepository<User> users,
                              IPostsRepository posts, IDatabaseOperations dbOps, ViewModelFactory viewModelFactory)
        {
            _renderer = renderer;
            _postsHub = postsHub;
            _posts = posts;
            _dbOps = dbOps;
            _viewModelFactory = viewModelFactory;
            _users = users;
        }

        private Task<User> GetCurrentUser() => _users.GetOne(it => it.UserName == User.Identity.Name);

        public async Task<IActionResult> Index()
        {
            User currentUser = await GetCurrentUser();

            var posts = await _posts.GetMany(
                count: 5,
                order: PostsOrder.CreatedAtDescending, 
                propsToInclude: new []{ nameof(Post.Author), nameof(Post.LikedBy), nameof(Post.DislikedBy) }
            );

            var vm = _viewModelFactory.CreateHomeViewModel(posts.AsReadOnly(), currentUser);

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(string postText)
        {
            User author = await GetCurrentUser();
            Post post = Generator.GeneratePost(text: postText, createdAt: DateTime.Today, author: author, likesCount: 0, dislikesCount: 0);
            Post storedPost = _posts.Insert(post);

            Task saveChangesTask = _dbOps.SaveChangesAsync();
            var postVm = _viewModelFactory.CreatePostViewModel(storedPost, author.Id);
            string postHtml = await _renderer.RenderPartialView("_Post", postVm);
            await saveChangesTask;
            await _postsHub.Emit(PostEvent.PostPublished, postHtml);

            return Ok("");
        }

        [HttpPost]
        public async Task<IActionResult> LoadPostsAjax(int count, int skip, string[] propsToInclude)
        {
            User currentUser = await GetCurrentUser();

            var posts = await _posts.GetMany(
                order: PostsOrder.CreatedAtDescending,
                propsToInclude: propsToInclude,
                count: count,
                skip: skip
            );

            IEnumerable<Task<string>> renderingTasks =
                posts
                .Select(it => _viewModelFactory.CreatePostViewModel(it, currentUser.Id))
                .Select(it => _renderer.RenderPartialView("_Post", it));

            var rendered = await Task.WhenAll(renderingTasks);
            return Ok(rendered);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePost(UpdatePostModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var currentUser = await GetCurrentUser();

            Post post = await _posts.GetOne(
                it => it.Id == model.Id, 
                nameof(Post.Author), nameof(Post.LikedBy), nameof(Post.DislikedBy)
            );

            bool hasCurrentUserLikedPost = post.LikedBy.Any(it => it.Id == currentUser.Id);
            bool hasCurrentUserDislikedPost = post.DislikedBy.Any(it => it.Id == currentUser.Id);

            if (model.Text != null)
            {
                if (post.AuthorId == currentUser.Id)
                    post.Text = model.Text;
                else
                    return Forbid();
            }

            switch (model.RateAction)
            {
                case PostAction.Like:

                    if (post.AuthorId != currentUser.Id  &&  !hasCurrentUserLikedPost)
                    {
                        if (hasCurrentUserDislikedPost)
                        {
                            post.DislikedBy.RemoveIf(it => it.Id == currentUser.Id);
                            _posts.Update(post);
                            await _dbOps.SaveChangesAsync();
                        }

                        post.LikedBy.Add(currentUser);
                    }
                    else
                        return Forbid();

                    break;


                case PostAction.Dislike:

                    if (post.AuthorId != currentUser.Id  &&  !hasCurrentUserDislikedPost)
                    {
                        if (hasCurrentUserLikedPost)
                        {
                            post.LikedBy.RemoveIf(it => it.Id == currentUser.Id);
                            _posts.Update(post);
                            await _dbOps.SaveChangesAsync();
                        }

                        post.DislikedBy.Add(currentUser);
                    }
                    else
                        return Forbid();

                    break;


                case PostAction.UnLike:

                    if (post.AuthorId != currentUser.Id && hasCurrentUserLikedPost)
                        post.LikedBy.RemoveIf(it => it.Id == currentUser.Id);
                    else
                        return Forbid();
                    break;


                case PostAction.UnDislike:

                    if (post.AuthorId != currentUser.Id && hasCurrentUserDislikedPost)
                        post.DislikedBy.RemoveIf(it => it.Id == currentUser.Id);
                    else
                        return Forbid();
                    break;
            }

            _posts.Update(post);
    
            Task saveChangesTask = _dbOps.SaveChangesAsync();
            var postVm = _viewModelFactory.CreatePostViewModel(post, currentUser.Id);
            string postHtml = await _renderer.RenderPartialView("_Post", postVm);
            await saveChangesTask;
            await _postsHub.Emit(PostEvent.PostChanged, postHtml);

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
            await _postsHub.Emit(PostEvent.PostDeleted, postId);

            return Ok("");
        }
    }
}
