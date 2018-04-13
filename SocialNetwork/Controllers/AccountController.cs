using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.DevelopmentUtilities;
using SocialNetwork.Interface.Models;
using SocialNetwork.Interface.Services;
using SocialNetwork.Web.ViewModels;
using Utilities;
using SignInResult = SocialNetwork.Interface.Models.SignInResult;

namespace SocialNetwork.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthenticator _authenticator;

        public AccountController(IAuthenticator authenticator)
        {
            _authenticator = authenticator;
        }

        private ViewResult CreateRegisterView(RegisterFormViewModel formModel) =>
                new RegisterViewModel
                {
                    ActivePage = Page.Account_Register,
                    Form = formModel,
                    Title = "Register"
                }
                .Let(View);

        private ViewResult CreateLoginView(LoginFormViewModel formModel) =>
            new LoginViewModel
            {
                ActivePage = Page.Account_Login,
                Form = formModel,
                Title = "Login"
            }
            .Let(View);

        [AllowAnonymous]
        public ViewResult Register() => CreateRegisterView(new RegisterFormViewModel());

        [AllowAnonymous]
        public ViewResult Login() => CreateLoginView(new LoginFormViewModel());

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind(Prefix = "Form")]RegisterFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = Generator.GenerateUser(userName: model.UserName, email: "someone@mailservice.com");
                RegistrationResult result = await _authenticator.Register(user, model.Password);
                if (result.HasSucceeded)
                {
                    await _authenticator.SignIn(user.UserName, model.Password, isPersistent: false);
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
            }

            return CreateRegisterView(model);
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind(Prefix = "Form")]LoginFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                SignInResult result = await _authenticator.SignIn(model.UserName, model.Password, 
                    isPersistent: model.RememberMe);
                if (result.HasSucceeded)
                {
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return CreateLoginView(model);
                }
            }
            else
                return CreateLoginView(model);
        }

        //
        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _authenticator.SignOut();
            return RedirectToAction(nameof(Login));
        }
    }
}