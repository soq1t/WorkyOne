using System.ComponentModel.DataAnnotations;

namespace WorkyOne.MVC.ViewModels.Authentification
{
    public class LogInViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
