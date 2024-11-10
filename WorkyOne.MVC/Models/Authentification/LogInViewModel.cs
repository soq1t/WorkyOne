using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WorkyOne.MVC.Models.Authentification
{
    public class LogInViewModel
    {
        [Required(ErrorMessage = "Введите имя пользователя")]
        [DisplayName("Имя пользователя")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [PasswordPropertyText]
        [DisplayName("Пароль")]
        public string Password { get; set; }

        [DisplayName("Запомнить меня")]
        public bool RememberMe { get; set; }
    }
}
