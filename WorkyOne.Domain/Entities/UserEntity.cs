using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WorkyOne.Domain.Entities.Schedule;

namespace WorkyOne.Domain.Entities
{
    public class UserEntity : IdentityUser
    {
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Активирован ли пользователь
        /// </summary>
        public bool IsActivated { get; set; }
    }
}
