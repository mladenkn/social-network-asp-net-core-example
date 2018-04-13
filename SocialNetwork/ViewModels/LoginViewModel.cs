using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Web.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public LoginFormViewModel Form { get; set; }
    }

    public class LoginFormViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(length: 8, ErrorMessage = "Password must be atleast 8 characters long")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
