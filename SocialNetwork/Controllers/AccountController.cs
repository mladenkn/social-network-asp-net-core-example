using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.DevelopmentUtilities;
using SocialNetwork.Interface.Models;
using SocialNetwork.Interface.Services;
using SocialNetwork.Web.Services;
using SocialNetwork.Web.ViewModels;
using Utilities;
using SignInResult = SocialNetwork.Interface.Models.SignInResult;

namespace SocialNetwork.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthenticator _authenticator;
        private readonly ViewModelFactory _viewModelFactory;

        public AccountController(IAuthenticator authenticator, ViewModelFactory viewModelFactory)
        {
            _authenticator = authenticator;
            _viewModelFactory = viewModelFactory;
        }

        [AllowAnonymous]
        public ViewResult Register() =>
            new RegisterFormViewModel()
                .Let(_viewModelFactory.CreateRegisterViewModel)
                .Let(View);

        [AllowAnonymous]
        public ViewResult Login() =>
            new LoginFormViewModel()
                .Let(_viewModelFactory.CreateLoginViewModel)
                .Let(View);

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

            return model.Let(_viewModelFactory.CreateRegisterViewModel).Let(View);
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

                    return model
                        .Let(_viewModelFactory.CreateLoginViewModel)
                        .Let(View);
                }
            }
            else
                return model
                    .Let(_viewModelFactory.CreateLoginViewModel)
                    .Let(View);
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