using System.Threading.Tasks;

namespace SocialNetwork.Interface.Services
{
    public interface IHub
    {
        Task Emit(string name, object data);
    }
}
