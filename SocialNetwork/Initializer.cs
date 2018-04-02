using System.Linq;
using System.Threading.Tasks;
using SocialNetwork.DevelopmentUtilities;
using SocialNetwork.Interfaces.DAL;
using SocialNetwork.Interfaces.Services;
using SocialNetwork.Models;
using Utilities;

namespace SocialNetwork.Web
{
    public class Initializer
    {
        private readonly IRepository<Post> _postsRepository;
        private readonly TestDataContainer _data;
        private readonly IDatabaseOperations _dbOps;
        private readonly IAuthenticator _authenticator;

        public Initializer(IRepository<Post> postsRepository, TestDataContainer data,
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
        }
    }
}
