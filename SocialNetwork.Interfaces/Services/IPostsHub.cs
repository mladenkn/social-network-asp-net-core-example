using System.Threading.Tasks;

namespace SocialNetwork.Interface.Services
{
    public interface IPostsHub
    {
        Task Emit(string name, object data);
    }
}
