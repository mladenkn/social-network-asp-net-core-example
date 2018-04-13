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
        [MinLength(8)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
