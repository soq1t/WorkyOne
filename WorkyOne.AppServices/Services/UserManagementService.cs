using WorkyOne.AppServices.Interfaces.Repositories;
using WorkyOne.AppServices.Interfaces.Services;
using WorkyOne.AppServices.Mappers;
using WorkyOne.Contracts.DTOs;
using WorkyOne.Domain.Entities;

namespace WorkyOne.AppServices.Services
{
    /// <summary>
    /// Сервис управления пользователями
    /// </summary>
    public class UserManagementService : IUserManagementService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IUserDatasRepository _userDatasRepository;
        private readonly UserInfoDtoMapper _userInfoDtoMapper;

        public UserManagementService(
            IUsersRepository usersRepository,
            UserInfoDtoMapper userInfoDtoMapper,
            IUserDatasRepository userDatasRepository
        )
        {
            _usersRepository = usersRepository;
            _userInfoDtoMapper = userInfoDtoMapper;
            _userDatasRepository = userDatasRepository;
        }

        public async Task<UserInfoDto?> GetUserInfoAsync(string userId)
        {
            UserEntity? user = await _usersRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                return null;
            }

            UserDataEntity? userData = await _userDatasRepository.GetAsync(userId);

            UserInfoDto userInfo = _userInfoDtoMapper.MapToUserInfoDto(user, userData);

            return userInfo;
        }
    }
}
