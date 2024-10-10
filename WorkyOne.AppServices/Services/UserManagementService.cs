using AutoMapper;
using WorkyOne.AppServices.Interfaces.Repositories.Users;
using WorkyOne.AppServices.Interfaces.Services;
using WorkyOne.Contracts.DTOs.Common;
using WorkyOne.Contracts.Requests.Common;
using WorkyOne.Domain.Entities.Users;

namespace WorkyOne.AppServices.Services
{
    /// <summary>
    /// Сервис управления пользователями
    /// </summary>
    public class UserManagementService : IUserManagementService
    {
        private readonly IUsersRepository _usersRepo;
        private readonly IUserDatasRepository _usersDataRepo;
        private readonly IMapper _mapper;

        public UserManagementService(
            IUsersRepository usersRepo,
            IUserDatasRepository usersDataRepo,
            IMapper mapper
        )
        {
            _usersRepo = usersRepo;
            _usersDataRepo = usersDataRepo;
            _mapper = mapper;
        }

        public async Task<UserInfoDto?> GetUserInfoAsync(string userId)
        {
            UserEntity? user = await _usersRepo.GetAsync(new UserRequest { Id = userId });

            if (user == null)
            {
                return null;
            }

            UserDataEntity? userData = await _usersDataRepo.GetAsync(
                new UserDataRequest { UserId = userId }
            );

            if (userData == null)
            {
                userData = new UserDataEntity(userId);
                await _usersDataRepo.CreateAsync(userData);
            }
            UserInfoDto dto = new UserInfoDto();
            _mapper.Map(user, dto);
            _mapper.Map(userData, dto);

            return dto;
        }
    }
}
