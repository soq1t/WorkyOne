using Microsoft.AspNetCore.Identity;
using WorkyOne.Domain.Attributes.Updating;
using WorkyOne.Domain.Interfaces.Common;

namespace WorkyOne.Domain.Entities.Users
{
    public class UserEntity : IdentityUser, IEntity
    {
        /// <summary>
        /// Имя пользователя
        /// </summary>
        [AutoUpdated]
        public string? FirstName { get; set; }

        /// <summary>
        /// Активирован ли пользователь
        /// </summary>
        [AutoUpdated]
        public bool IsActivated { get; set; }
    }
}
