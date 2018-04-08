using System.Collections.Generic;

namespace SocialNetwork.Web.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public IReadOnlyCollection<PostViewModel> Posts { get; set; }
    }
}
