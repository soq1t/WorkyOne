using Microsoft.AspNetCore.Identity;
using WorkyOne.Domain.Attributes.Updating;
using WorkyOne.Domain.Entities.Auth;
using WorkyOne.Domain.Interfaces.Common;

namespace WorkyOne.Domain.Entities.Users
{
    public class UserEntity : IdentityUser, IEntity
    {
        [AutoUpdated]
        public override string? UserName
        {
            get => base.UserName;
            set => base.UserName = value;
        }

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

        public List<SessionEntity> Sessions { get; set; }
    }
}
