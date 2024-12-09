using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WorkyOne.MVC.Models.Authentification
{
    /// <summary>
    /// Модель данных для регистрации пользователей
    /// </summary>
    public class RegistrationViewModel
    {
        /// <summary>
        /// Логин
        /// </summary>
        [DisplayName("Логин")]
        [Required(ErrorMessage = "Необходимо ввести имя пользователя")]
        public string Username { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        [Required(ErrorMessage = "Необходимо ввести пароль")]
        [DisplayName("Пароль")]
        public string Password { get; set; }

        /// <summary>
        /// Повторение пароля
        /// </summary>
        [Required(ErrorMessage = "Необходимо повторно ввести пароль")]
        [Compare(nameof(Password), ErrorMessage = "Пароли должны совпадать")]
        [DisplayName("Повторите пароль")]
        public string RepeatedPassword { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        [Required(ErrorMessage = "Необходимо ввести Ваше имя")]
        [DisplayName("Имя")]
        public string FirstName { get; set; }
    }
}
