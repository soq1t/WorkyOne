using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WorkyOne.AppServices.Interfaces.Repositories;
using WorkyOne.Domain.Entities;

namespace WorkyOne.Repositories.Repositories
{
    /// <summary>
    /// Репозиторий пользователей приложения
    /// </summary>
    public class UsersRepository : IUsersRepository
    {
        private readonly UserManager<UserEntity> _userManager;

        public UsersRepository(UserManager<UserEntity> userManager)
        {
            _userManager = userManager;
        }

        /// <inheritdoc/>
        public Task<List<UserEntity>> GetAllAsync()
        {
            Task<List<UserEntity>> users = _userManager.Users.ToListAsync();
            return users;
        }

        /// <inheritdoc/>
        public Task<UserEntity?> GetUserByIdAsync(string id)
        {
            Task<UserEntity?> user = _userManager.FindByIdAsync(id);
            return user;
        }

        /// <inheritdoc/>
        public Task<UserEntity?> GetUserByUsernameAsync(string username)
        {
            Task<UserEntity?> user = _userManager.FindByNameAsync(username);
            return user;
        }
    }
}
