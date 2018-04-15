using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Web.Controllers;

namespace SocialNetwork.Web.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        public RegisterFormViewModel Form { get; set; }
    }

    public class RegisterFormViewModel
    {
        [Required]
        [Display(Name = "Username")]
        //[Remote(action: nameof(AccountController.CheckUserNameAvailability), controller: "Account",
        //        ErrorMessage = "Provided username is not available")]
        public string UserName { get; set; }

        [Required]
        [MinLength(length: 8, ErrorMessage = "Password must be atleast 8 characters long")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Repeat password")]
        [Compare("Password", ErrorMessage = "Password doesn't match.")]
        public string RepeatedPassword { get; set; }
    }
}
