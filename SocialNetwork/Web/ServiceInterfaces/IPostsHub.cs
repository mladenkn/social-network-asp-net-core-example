using System.Threading.Tasks;

namespace SocialNetwork.Web.ServiceInterfaces
{
    public interface IPostsHub
    {
        Task EmitPostChanged(string postHtml);
        Task EmitPostDeleted(long postId);
        Task EmitPostPublished(string postHtml);
    }
}