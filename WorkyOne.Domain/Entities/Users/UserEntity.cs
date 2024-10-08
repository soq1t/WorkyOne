using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WorkyOne.Domain.Entities.Schedule;
using WorkyOne.Domain.Interfaces.Common;

namespace WorkyOne.Domain.Entities.Users
{
    public class UserEntity : IdentityUser, IEntity, IUpdatable<UserEntity>
    {
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Активирован ли пользователь
        /// </summary>
        public bool IsActivated { get; set; }

        public void UpdateFields(UserEntity entity)
        {
            FirstName = entity.FirstName;
            IsActivated = entity.IsActivated;
            UserName = entity.UserName;
            Email = entity.Email;
        }
    }
}
