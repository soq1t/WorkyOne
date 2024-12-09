using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WorkyOne.AppServices.Interfaces.Repositories.Context;
using WorkyOne.AppServices.Interfaces.Services.Auth;
using WorkyOne.Domain.Entities.Users;

namespace WorkyOne.AppServices.Services.Auth
{
    /// <summary>
    /// Сервис по работе с ролями в приложении
    /// </summary>
    public class RolesService : IRolesService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IUsersContextService _contextService;

        public RolesService(
            RoleManager<IdentityRole> roleManager,
            UserManager<UserEntity> userManager,
            IUsersContextService contextService
        )
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _contextService = contextService;
        }

        public async Task<List<string>> GetRolesAsync(CancellationToken cancellation = default)
        {
            var roles = await _roleManager.Roles.ToListAsync(cancellation);

            if (roles == null || roles.Count == 0)
            {
                return [];
            }
            else
            {
                return roles.Select(role => role.Name).ToList();
            }
        }

        public async Task<bool> SetRolesToUserAsync(
            string userId,
            CancellationToken cancellation = default,
            params string[] roles
        )
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            bool useTransactions = !_contextService.TransactionCreated();

            if (useTransactions)
            {
                await _contextService.CreateTransactionAsync(cancellation);
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var result = await _userManager.RemoveFromRolesAsync(user, userRoles);

            if (!result.Succeeded)
            {
                if (useTransactions)
                {
                    await _contextService.RollbackTransactionAsync(cancellation);
                }

                return false;
            }

            result = await _userManager.AddToRolesAsync(user, roles);

            if (result.Succeeded)
            {
                if (useTransactions)
                {
                    await _contextService.CommitTransactionAsync(cancellation);
                }

                return true;
            }
            else
            {
                if (useTransactions)
                {
                    await _contextService.RollbackTransactionAsync(cancellation);
                }
                return false;
            }
        }
    }
}
