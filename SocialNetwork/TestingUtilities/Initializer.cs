using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SocialNetwork.Interfaces.DAL;
using SocialNetwork.Models;
using Utilities;

namespace SocialNetwork.TestingUtilities
{
    public class Initializer
    {
        private readonly IRepository<Post> _postsRepository;
        private readonly IRepository<User> _usersRepository;
        private readonly TestDataContainer _data;
        private readonly IDatabaseOperations _dbOps;
        private readonly UserManager<User> _userManager;

        public Initializer(IRepository<Post> postsRepository, IRepository<User> usersRepository, TestDataContainer data,
                        IDatabaseOperations dbOps, UserManager<User> userManager)
        {
            _postsRepository = postsRepository;
            _usersRepository = usersRepository;
            _data = data;
            _dbOps = dbOps;
            _userManager = userManager;
        }

        public async Task Initialize()
        {
            _data.Posts.ForEach(_postsRepository.Insert);
            _data.Users.Values.ForEach(_usersRepository.Insert);
            await _dbOps.SaveChangesAsync();

            var user = new User
            {
                UserName = "Mladen",
                Email = "user@mail.com",
                ProfileImageUrl = Generator.RandomImage()
            };
            await _userManager.CreateAsync(user, "a1234567");
        }
    }
}
