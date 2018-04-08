using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.Web.ViewModels
{
    public class BaseViewModel
    {
        public string Title { get; set; }
        public Page ActivePage { get; set; }
        public string Username { get; set; }
    }
}
