using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WorkyOne.AppServices.Interfaces.Services.Users
{
    /// <summary>
    /// Cервиса, отвечающий за авторизацию и аутентификацию пользователей
    /// </summary>
    public class AuthService : IAuthService
    {
        public bool IsUserInRoles(ClaimsPrincipal user, params string[] roles)
        {
            if (user == null)
                return false;

            if (user.IsInRole("God"))
                return true;

            foreach (var item in roles)
            {
                if (user.IsInRole(item))
                    return true;
            }

            return false;
        }
    }
}
