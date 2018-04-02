using System.Threading.Tasks;
using SocialNetwork.Interfaces.DAL;
using SocialNetwork.Models;

namespace SocialNetwork.Interfaces.Services
{
    public interface IUserManager : IRepository<User>
    {
        Task<User> GetOneByUsernameAsync(string username);
    }
}
