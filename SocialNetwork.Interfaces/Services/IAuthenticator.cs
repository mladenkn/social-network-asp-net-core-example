﻿using System.Threading.Tasks;
using SocialNetwork.Interfaces.Models;
using SocialNetwork.Models;
using SignInResult = SocialNetwork.Interfaces.Models.SignInResult;

namespace SocialNetwork.Interfaces.Services
{
    public interface IAuthenticator
    {
        Task<RegistrationResult> Register(User user, string password);
        Task<SignInResult> SignIn(string username, string password, bool isPersistent = true);
        Task SignOut();
    }
}
