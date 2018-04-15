using System.Threading.Tasks;
using SocialNetwork.Interface.Constants;

namespace SocialNetwork.Interface.Services
{
    public interface IPostsHub
    {
        Task Emit(PostEvent e, object data);
    }
}
