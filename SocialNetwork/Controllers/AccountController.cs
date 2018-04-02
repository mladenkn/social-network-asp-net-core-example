using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Interfaces.Models;
using SocialNetwork.Interfaces.Services;
using SocialNetwork.Models;
using SocialNetwork.Web.ViewModels;
using SignInResult = SocialNetwork.Interfaces.Models.SignInResult;

namespace SocialNetwork.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserManager _userManager2;

        public AccountController(IUserManager userManager2)
        {
            _userManager2 = userManager2;
        }

        [AllowAnonymous]
        public ViewResult Register() => View(new RegisterViewModel());

        [AllowAnonymous]
        public ViewResult Login() => View(new LoginViewModel());

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User { UserName = model.UserName, Email = "user@mail.com" };
                RegistrationResult result = await _userManager2.Register(user, model.Password);
                if (result.HasSucceeded)
                {
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                    // Send an email with this link
                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                    //await _emailSender.SendEmailAsync(model.Email, "Confirm your account",
                    //    "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");
                    await _userManager2.SignIn(user.UserName, model.Password, isPersistent: false);
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
            }
            return View(model);
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                SignInResult result = await _userManager2.SignIn(model.UserName, model.Password, 
                    isPersistent: model.RememberMe);
                if (result.HasSucceeded)
                {
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }
            else
                return View(model);
        }

        //
        // POST: /Account/LogOut
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _userManager2.SignOut();
            return RedirectToAction(nameof(Register));
        }
    }
}