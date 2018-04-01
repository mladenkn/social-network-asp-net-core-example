using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SocialNetwork.Interfaces.DAL;
using SocialNetwork.Models;
using SocialNetwork.TestingUtilities;
using SocialNetwork.Web.ViewModels;
using SocialNetwork.Web.ServiceInterfaces;
using Utilities;

namespace SocialNetwork.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IViewRendererService _renderer;
        private readonly TestDataContainer _testDataContainer;
        private readonly IHubContext<Hub> _hub;
        private readonly IRepository<Post> _postsRepository;
        private readonly IDatabaseOperations _dbOps;

        public HomeController(IViewRendererService renderer, TestDataContainer testDataContainer, IHubContext<Hub> hub,
                              IRepository<Post> postsRepository, IDatabaseOperations dbOps)
        {
            _renderer = renderer;
            _testDataContainer = testDataContainer;
            _hub = hub;
            _postsRepository = postsRepository;
            _dbOps = dbOps;
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
            Post post = Generator.RandomPost(text: postText, createdAt: DateTime.Today, author: author, likesCount: 0, dislikesCount: 0);
            Post storedPost = _postsRepository.Insert(post);

            Task saveChangesTask = _dbOps.SaveChangesAsync();
            string postHtml = await _renderer.RenderPartialView("_Post", storedPost);
            await saveChangesTask;
            await _hub.Clients.All.SendAsync(Constants.PostsEvents.PostPublished, postHtml);

            return Ok("");
        }

        [HttpPost]
        public async Task<IActionResult> LoadPosts(int count, int skip, string[] propsToInclude)
        {
            var posts = await _postsRepository.GetMany(
                orderBy: it => it.OrderByDescending(post => post.CreatedAt),
                propsToInclude: propsToInclude,
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
    
            Task saveChangesTask = _dbOps.SaveChangesAsync();
            string postHtml = await _renderer.RenderPartialView("_Post", post);
            await saveChangesTask;
            await _hub.Clients.All.SendAsync(Constants.PostsEvents.PostChanged, postHtml);

            return Ok("");
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(long postId)
        {
            await _postsRepository.Delete(it => it.Id == postId);

            await _dbOps.SaveChangesAsync();
            await _hub.Clients.All.SendAsync(Constants.PostsEvents.PostDeleted, postId);

            return Ok("");
        }
    }
}
