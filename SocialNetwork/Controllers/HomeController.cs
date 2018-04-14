using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.DevelopmentUtilities;
using SocialNetwork.Interface.DAL;
using SocialNetwork.Interface.Models.Entities;
using SocialNetwork.Interface.Services;
using SocialNetwork.Web.ServiceInterfaces;
using SocialNetwork.Web.ViewModels;
using SocialNetwork.Web.Constants;
using SocialNetwork.Web.Models;
using SocialNetwork.Web.Services;
using SocialNetwork.Web.Utilities;

namespace SocialNetwork.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IViewRendererService _renderer;
        private readonly IHub _hub;
        private readonly IPostsRepository _posts;
        private readonly IDatabaseOperations _dbOps;
        private readonly ViewModelFactory _viewModelFactory;
        private readonly IRepository<User> _users;

        public HomeController(IViewRendererService renderer, IHub hub, IRepository<User> users,
                              IPostsRepository posts, IDatabaseOperations dbOps, ViewModelFactory viewModelFactory)
        {
            _renderer = renderer;
            _hub = hub;
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

            var postsViewModels = posts
                .Select(it => _viewModelFactory.CreatePostViewModel(it, currentUser.Id))
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
            var postVm = _viewModelFactory.CreatePostViewModel(storedPost, author.Id);
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
                .Select(it => _viewModelFactory.CreatePostViewModel(it, currentUserId))
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
                new[] { nameof(Post.Author), nameof(Post.LikedBy), nameof(Post.DislikedBy)}
            );

            if (model.Text != null)
            {
                if (post.AuthorId == currentUser.Id)
                    post.Text = model.Text;
                else
                    return Forbid();
            }

            if (model.Heading != null)
            {
                if (post.AuthorId == currentUser.Id)
                    post.Heading = model.Heading;
                else
                    return Forbid();
            }

            if (model.AddLike)
            {
                if (post.AuthorId != currentUser.Id)
                {
                    post.LikesCount++;
                    post.LikedBy.Add(currentUser);
                }
                else
                    return Forbid();
            }
            
            if (model.AddDislike)
            {
                if (post.AuthorId != currentUser.Id)
                {
                    post.DislikesCount++;
                    post.DislikedBy.Add(currentUser);
                }
                else
                    return Forbid();
            }

            _posts.Update(post);
    
            Task saveChangesTask = _dbOps.SaveChangesAsync();
            var postVm = _viewModelFactory.CreatePostViewModel(post, currentUser.Id);
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
