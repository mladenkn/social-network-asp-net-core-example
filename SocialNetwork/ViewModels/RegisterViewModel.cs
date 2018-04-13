using System.ComponentModel.DataAnnotations;

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
        public string UserName { get; set; }

        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        [Display(Name = "Repeat password")]
        [Compare("Password", ErrorMessage = "Password doesn't match.")]
        public string RepeatedPassword { get; set; }
    }
}
