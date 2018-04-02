using System.Threading.Tasks;

namespace SocialNetwork.Interfaces.DAL
{
    public interface IDatabaseOperations
    {
        Task SaveChangesAsync();
    }
}
