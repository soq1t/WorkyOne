using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkyOne.AppServices.DTOs
{
    /// <summary>
    /// DTO данных о пользователе
    /// </summary>
    public class UserInfoDto
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Идентификатор пользовательских данных
        /// </summary>
        public string UserDataId { get; set; }

        /// <summary>
        /// Пользовательский юзернейм
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Шаблоны расписаний пользователя
        /// </summary>
        public List<TemplateDto> Templates { get; set; }
    }
}
