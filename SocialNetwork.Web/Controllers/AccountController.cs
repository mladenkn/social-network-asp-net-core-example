using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.DevelopmentUtilities;
using SocialNetwork.Interface.DAL;
using SocialNetwork.Interface.Models;
using SocialNetwork.Interface.Models.Entities;
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
        private readonly IRepository<User> _usersRepository;

        public AccountController(IAuthenticator authenticator, ViewModelFactory viewModelFactory,
                                IRepository<User> usersRepository)
        {
            _authenticator = authenticator;
            _viewModelFactory = viewModelFactory;
            _usersRepository = usersRepository;
        }

        [AllowAnonymous]
        public ViewResult Register() =>
                _viewModelFactory.CreateRegisterViewModel()
                .Let(View);

        [AllowAnonymous]
        public ViewResult Login() =>
                _viewModelFactory.CreateLoginViewModel()
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

                switch (result)
                {
                    case RegistrationSuccess _:
                        await _authenticator.SignIn(user.UserName, model.Password, isPersistent: false);
                        return RedirectToAction(nameof(HomeController.Index), "Home");

                    case RegistrationFailure r:
                        ModelState.AddModelError("", "Invalid registration attempt");
                        if (r.Errors.Contains(RegistrationError.DuplicateUserName))
                            ModelState.AddModelError(nameof(RegisterFormViewModel.UserName), "Provided username allready exists");
                        break;
                }
            }

            return model
                .Let(_viewModelFactory.CreateRegisterViewModel)
                .Let(View);
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

                switch (result)
                {
                    case SignInSuccess _:
                        return RedirectToAction(nameof(HomeController.Index), "Home");

                    case SignInFailure _:
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        break;
                }
            }

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

        [HttpGet]
        [AllowAnonymous]
        public IActionResult CheckUserNameAvailability([Bind(Prefix = "Form.UserName")]string userName)
        {
            var usersOrNull = _usersRepository.GetMany(filter: it => it.UserName == userName).Result;
            return Json(usersOrNull.Count == 0);
        }
    }
}