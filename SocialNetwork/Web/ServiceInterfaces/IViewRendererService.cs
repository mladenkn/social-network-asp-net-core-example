using System.Threading.Tasks;

namespace SocialNetwork.Web.ServiceInterfaces
{
    public interface IViewRendererService
    {
        Task<string> RenderPartialView(string viewName, object model);
    }
}
