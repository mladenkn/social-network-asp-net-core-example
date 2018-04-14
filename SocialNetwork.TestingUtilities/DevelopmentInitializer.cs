using System.Linq;
using System.Threading.Tasks;
using SocialNetwork.Interface.DAL;
using SocialNetwork.Interface.Models.Entities;
using SocialNetwork.Interface.Services;
using Utilities;

namespace SocialNetwork.DevelopmentUtilities
{
    public class DevelopmentInitializer
    {
        private readonly IRepository<Post> _postsRepository;
        private readonly TestDataContainer _data;
        private readonly IDatabaseOperations _dbOps;
        private readonly IAuthenticator _authenticator;

        public DevelopmentInitializer(IRepository<Post> postsRepository, TestDataContainer data,
                           IDatabaseOperations dbOps, IAuthenticator authenticator)
        {
            _postsRepository = postsRepository;
            _data = data;
            _dbOps = dbOps;
            _authenticator = authenticator;
        }

        public async Task Initialize()
        {
            await _data.Users.Values
                .Select(it => _authenticator.Register(it, _data.DummyPassword))
                .Let(Task.WhenAll);

            _data.Posts.ForEach(_postsRepository.Insert);

            await _dbOps.SaveChangesAsync();

            foreach (var post in _data.Posts)
                DevelopmentUtilities.AddRandomRatings(post, _data.Users.Values.ToList(), 7);
        }
    }
}
