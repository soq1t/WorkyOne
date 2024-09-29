using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkyOne.AppServices.Interfaces.Repositories;
using WorkyOne.Domain.Entities;
using WorkyOne.Repositories.Contextes;

namespace WorkyOne.Repositories.Repositories
{
    /// <summary>
    /// Репозиторий пользовательских данных
    /// </summary>
    public class UserDatasRepository : IUserDatasRepository
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ApplicationDbContext _context;

        public UserDatasRepository(IUsersRepository usersRepository, ApplicationDbContext context)
        {
            _usersRepository = usersRepository;
            _context = context;
        }

        public async Task<UserDataEntity?> GetAsync(string userId)
        {
            Task<UserEntity?> user = _usersRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                return null;
            }

            UserDataEntity? userData = await _context.UserDatas.FirstOrDefaultAsync(d =>
                d.UserId == userId
            );

            if (userData == null)
            {
                userData = new UserDataEntity() { UserId = userId };
                _context.UserDatas.Add(userData);
                await _context.SaveChangesAsync();
            }

            return userData;
        }

        public async Task UpdateAsync(UserDataEntity data)
        {
            UserDataEntity? updatedData = await _context.UserDatas.FirstOrDefaultAsync(d =>
                d.Id == data.Id
            );

            if (updatedData != null)
            {
                updatedData.Templates = data.Templates;
                _context.Update(updatedData);
                await _context.SaveChangesAsync();
            }
        }
    }
}
