using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkyOne.Domain.Entities.Users;
using WorkyOne.Domain.Requests.Common;

namespace WorkyOne.Domain.Requests.Users
{
    public sealed class UserDataRequest : EntityRequest<UserDataEntity>
    {
        public UserDataRequest(string? userId = null)
        {
            UserId = userId;
        }

        private string? _userId;

        /// <summary>
        /// Идентификатора <see cref="UserEntity"/>, для которого запрашивается <see cref="UserDataEntity"/>
        /// </summary>
        public string? UserId
        {
            get => _userId;
            set
            {
                _userId = value;

                if (value == null)
                {
                    Predicate = (x) => true;
                }
                else
                {
                    Predicate = (x) => x.UserId == value;
                }
            }
        }
    }
}
