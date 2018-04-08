using System.Threading.Tasks;

namespace SocialNetwork.Interface.DAL
{
    public interface IDatabaseOperations
    {
        Task SaveChangesAsync();
    }
}
