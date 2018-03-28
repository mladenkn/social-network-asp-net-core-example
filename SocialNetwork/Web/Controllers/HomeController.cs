using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.DAL.Repositories;
using SocialNetwork.Interfaces.DAL;
using SocialNetwork.Models;
using SocialNetwork.TestingUtilities;
using SocialNetwork.Web.Services;
using SocialNetwork.Web.ViewModels;
using SocialNetwork.Utilities;
using SocialNetwork.Web.ServiceInterfaces;
using Utilities;

namespace SocialNetwork.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly PostsHub _postsHub;
        private readonly IViewRendererService _renderer;
        private readonly TestDataContainer _testDataContainer;
        private readonly IRepository<Post> _postsRepository;
        private readonly IDatabaseOperations _databaseOperations;

        public HomeController(PostsHub postsHub, IViewRendererService renderer, TestDataContainer testDataContainer,
                              IRepository<Post> postsRepository, IDatabaseOperations databaseOperations)
        {
            _postsHub = postsHub;
            _renderer = renderer;
            _testDataContainer = testDataContainer;
            _postsRepository = postsRepository;
            _databaseOperations = databaseOperations;
        }

        public async Task<IActionResult> Home()
        {
            var a = await _postsRepository.GetMany(
                orderBy: it => it.OrderByDescending(post => post.CreatedAt),
                propsToInclude: "Author",
                count: 5
            );

            var vm = new HomeViewModel {Posts = a.AsReadOnly() };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(string postText)
        {
            User author = _testDataContainer.Users.Values.ToList().RandomElement();
            Post post = Generator.RandomPost(text: postText, createdAt: DateTime.Today, author: author);
            Post storedPost = _postsRepository.Insert(post);

            Task saveChangesTask = _databaseOperations.SaveChangesAsync();
            string postHtml = await _renderer.RenderPartialView("_Post", storedPost);
            await saveChangesTask;
            await _postsHub.EmitPostPublished(postHtml);

            return Ok("");
        }

        [HttpPost]
        public async Task<IActionResult> LoadPosts(int count, int skip, string propsToInclude = "Author")
        {
            var posts = await _postsRepository.GetMany(
                orderBy: it => it.OrderByDescending(post => post.CreatedAt),
                propsToInclude: "Author",
                count: count,
                skip: skip
            );
            var renderingTasks = posts.Select(it => _renderer.RenderPartialView("_Post", it));
            var rendered = await Task.WhenAll(renderingTasks);
            return Ok(rendered);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePost(long id, string text = null, string heading = null,
                                                    bool addLike = false, bool addDislike = false)
        {
            Post post = await _postsRepository.GetOne(it => it.Id == id, "Author");

            post.Text = text ?? post.Text;
            post.Heading = heading ?? post.Heading;
            if (addLike)
                post.LikesCount++;
            if (addDislike)
                post.DislikesCount++;

            _postsRepository.Update(post);
    
            Task saveChangesTask = _databaseOperations.SaveChangesAsync();
            string postHtml = await _renderer.RenderPartialView("_Post", post);
            await saveChangesTask;
            await _postsHub.EmitPostChanged(postHtml);

            return Ok("");
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(long postId)
        {
            await _postsRepository.Delete(it => it.Id == postId);

            await _databaseOperations.SaveChangesAsync();
            await _postsHub.EmitPostDeleted(postId);

            return Ok("");
        }
    }
}
