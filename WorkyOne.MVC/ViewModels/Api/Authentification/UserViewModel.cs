using System.ComponentModel.DataAnnotations;

namespace WorkyOne.MVC.ViewModels.Api.Authentification
{
    public class UserViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
