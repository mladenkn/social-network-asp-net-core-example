using System.Collections.Generic;
using SocialNetwork.Models;

namespace SocialNetwork.Web.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public IReadOnlyCollection<Post> Posts { get; set; }
    }
}
