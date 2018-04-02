using System.Threading.Tasks;

namespace SocialNetwork.Interfaces.Services
{
    public interface IHub
    {
        Task Emit(string name, object data);
    }
}
