using Microsoft.AspNetCore.Mvc;

namespace SocialNetwork.Web.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Register()
        {
            return View();
        }
    }
}